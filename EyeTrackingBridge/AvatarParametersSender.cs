using CoreOSC;
using CoreOSC.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace EyeTrackingBridge
{
    internal class AvatarParametersSender : IDisposable
    {
        private static readonly BytesConverter bytesConverter = new BytesConverter();
        private static readonly IntConverter intConverter = new IntConverter();
        private static readonly StringConverter stringConverter = new StringConverter();
        private static readonly TimetagConverter timetagConverter = new TimetagConverter();
        private static readonly OscMessageConverter messageConverter = new OscMessageConverter();
        // private static readonly OscBundleConverter bundleConverter = new OscBundleConverter();

        private readonly UdpClient _client;

        public AvatarParametersSender(AvatarParametersSenderSettings settings)
        {
            _client = new UdpClient(settings.Hostname, settings.Port);
        }

        public async Task SendAsync(AvatarParameters parameters, CancellationToken cancellationToken)
        {
            var messages = new[] {
                // v2 format
                new OscMessage(
                    new Address("/avatar/parameters/EyeTrackingController/v2/LInOut"),
                    new object[] { parameters.LeftInOut }),
                new OscMessage(
                    new Address("/avatar/parameters/EyeTrackingController/v2/RInOut"),
                    new object[] { parameters.RightInOut }),
                new OscMessage(
                    new Address("/avatar/parameters/EyeTrackingController/v2/Pitch"),
                    new object[] { parameters.Pitch }),
                new OscMessage(
                    new Address("/avatar/parameters/EyeTrackingController/v2/LCloseness"),
                    new object[] { 1.0f - parameters.LocalLeftOpenness }),
                new OscMessage(
                    new Address("/avatar/parameters/EyeTrackingController/v2/RCloseness"),
                    new object[] { 1.0f - parameters.LocalRightOpenness }),
                new OscMessage(
                    new Address("/avatar/parameters/EyeTrackingController/v2/LCloseRemote"),
                    new object[] { parameters.RemoteIsOpenLeft ? 0 : 1 }),
                new OscMessage(
                    new Address("/avatar/parameters/EyeTrackingController/v2/RCloseRemote"),
                    new object[] { parameters.RemoteIsOpenRight ? 0 : 1 }),
                new OscMessage(
                    new Address("/avatar/parameters/EyeTrackingController/v2/Blink0"),
                    new object[] { parameters.BlinkCounter & 1 }),
                new OscMessage(
                    new Address("/avatar/parameters/EyeTrackingController/v2/Blink1"),
                    new object[] { (parameters.BlinkCounter >> 1) & 1 }),
                // Legacy format
                new OscMessage(
                    new Address("/avatar/parameters/EyeTracking_Yaw"),
                    new object[] { parameters.AverageYaw }),
                new OscMessage(
                    new Address("/avatar/parameters/EyeTracking_Pitch"),
                    new object[] { parameters.Pitch }),
                new OscMessage(
                    new Address("/avatar/parameters/EyeTracking_LClose"),
                    new object[] { parameters.LocalIsOpenLeft ? 0 : 1 }),
                new OscMessage(
                    new Address("/avatar/parameters/EyeTracking_RClose"),
                    new object[] { parameters.LocalIsOpenRight ? 0 : 1 }),
                new OscMessage(
                    new Address("/avatar/parameters/EyeTracking_LCloseRemote"),
                    new object[] { parameters.RemoteIsOpenLeft ? 0 : 1 }),
                new OscMessage(
                    new Address("/avatar/parameters/EyeTracking_RCloseRemote"),
                    new object[] { parameters.RemoteIsOpenRight ? 0 : 1 }),
                new OscMessage(
                    new Address("/avatar/parameters/EyeTracking_Blink0"),
                    new object[] { parameters.BlinkCounter & 1 }),
                new OscMessage(
                    new Address("/avatar/parameters/EyeTracking_Blink1"),
                    new object[] { (parameters.BlinkCounter >> 1) & 1 }),
            };

            // CoreOSC's bundle implementation is broken
            /*
            var bundle = new OscBundle(Timetag.FromDateTime(DateTime.Now), messages);
            var dwords = bundleConverter.Serialize(bundle);
            */
            var dwords = new List<DWord>();
            dwords.AddRange(stringConverter.Serialize("#bundle"));
            dwords.AddRange(timetagConverter.Serialize(Timetag.FromDateTime(DateTime.Now)));
            foreach(var message in messages)
            {
                var serialized = messageConverter.Serialize(message);
                dwords.AddRange(intConverter.Serialize(serialized.Count() * 4));
                dwords.AddRange(serialized);
            }

            _ = bytesConverter.Deserialize(dwords, out var bytes);
            var datagram = new ReadOnlyMemory<byte>(bytes.ToArray());
            await _client.SendAsync(datagram, cancellationToken);
        }

        public void Dispose()
        {
            _client?.Close();
            _client?.Dispose();
        }
    }
}
