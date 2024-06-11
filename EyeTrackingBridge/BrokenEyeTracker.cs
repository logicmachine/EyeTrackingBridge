using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace EyeTrackingBridge
{
    internal class BrokenEyeTracker : IEyeTracker
    {
        public class EyeData
        {
            [JsonInclude]
            [JsonPropertyName("gaze_direction_is_valid")]
            public bool GazeDirectionIsValid = false;

            [JsonInclude]
            [JsonPropertyName("gaze_direction")]
            public float[] GazeDirection = new float[] { };

            [JsonInclude]
            [JsonPropertyName("pupil_diameter_is_valid")]
            public bool PupilDiameterIsValid = false;

            [JsonInclude]
            [JsonPropertyName("pupil_diameter")]
            public float PupilDiameter = 0.0f;

            [JsonInclude]
            [JsonPropertyName("pupil_position_on_image_is_valid")]
            public bool PupilPositionOnImageIsValid = false;

            [JsonInclude]
            [JsonPropertyName("pupil_position_on_image")]
            public float[] PupilPositionOnImage = new float[] { };

            [JsonInclude]
            [JsonPropertyName("openness_is_valid")]
            public bool OpennessIsValid = false;

            [JsonInclude]
            [JsonPropertyName("openness")]
            public float Openness = 0.0f;
        }

        public class EyeDataPair
        {
            [JsonInclude]
            [JsonPropertyName("left")]
            public EyeData Left = new EyeData();

            [JsonInclude]
            [JsonPropertyName("right")]
            public EyeData Right = new EyeData();
        }

        private readonly string _hostname;
        private readonly int _port;
        private readonly TcpClient _client;
        private EyeTrackingMessage _lastMessage;

        public BrokenEyeTracker(BrokenEyeSettings settings)
        {
            _hostname = settings.Hostname;
            _port = settings.Port;
            _client = new TcpClient();
            _lastMessage = new EyeTrackingMessage();
        }

        public async Task ConnectAsync(CancellationToken cancellationToken)
        {
            while(!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await _client.ConnectAsync(_hostname, _port, cancellationToken);
                }
                catch (SocketException)
                {
                    await Task.Delay(1000, cancellationToken);
                    continue;
                }
                break;
            }

            var stream = _client.GetStream();
            stream.ReadTimeout = 10000;
            stream.WriteTimeout = 10000;

            // Send request (0x00: request for eye tracking data in JSON format)
            var request = new Memory<byte>(new[] { (byte)0x00 });
            await stream.WriteAsync(request, cancellationToken);
        }

        public async Task<EyeTrackingMessage> ReadAsync(CancellationToken cancellationToken)
        {
            var stream = _client.GetStream();
            stream.ReadTimeout = 10000;
            stream.WriteTimeout = 10000;

            // Read response header (ID: u8, Length: u32)
            var rawHeader = new byte[5];
            var readHeaderBytes = await stream.ReadAsync(rawHeader, cancellationToken);
            Debug.Assert(readHeaderBytes == rawHeader.Length);

            var responseId = (int)rawHeader[0];
            Debug.Assert(responseId == 0x00);

            var length = BitConverter.ToUInt32(rawHeader, 1);
            var rawData = new byte[length];
            var readDataBytes = await stream.ReadAsync(rawData, cancellationToken);
            Debug.Assert(readDataBytes == rawData.Length);

            var strData = Encoding.UTF8.GetString(rawData);
            var data = JsonSerializer.Deserialize<EyeDataPair>(strData);

            var message = new EyeTrackingMessage();
            if (data != null)
            {
                message.Timestamp = DateTime.Now;
                if (data.Left.GazeDirectionIsValid)
                {
                    message.LeftDirection = new Vector3(data.Left.GazeDirection);
                }
                else
                {
                    message.LeftDirection = _lastMessage.LeftDirection;
                }
                if (data.Right.GazeDirectionIsValid)
                {
                    message.RightDirection = new Vector3(data.Right.GazeDirection);
                }
                else
                {
                    message.RightDirection = _lastMessage.RightDirection;
                }
                var leftOpennessValid = data.Left.OpennessIsValid && data.Left.GazeDirectionIsValid;
                var rightOpennessValid = data.Right.OpennessIsValid && data.Right.GazeDirectionIsValid;
                message.LeftOpenness = leftOpennessValid ? data.Left.Openness : 0.0f;
                message.RightOpenness = rightOpennessValid ? data.Right.Openness : 0.0f;
                _lastMessage = message;
            }
            else
            {
                _lastMessage.Timestamp = DateTime.Now;
            }
            return _lastMessage;
        }

        public void Dispose()
        {
            _client?.Close();
            _client?.Dispose();
        }
    }
}
