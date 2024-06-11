using System;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace EyeTrackingBridge
{
    internal class DummyEyeTracker : IEyeTracker
    {
        public DummyEyeTracker()
        {
        }

        public Task ConnectAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task<EyeTrackingMessage> ReadAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(1, cancellationToken);
            var message = new EyeTrackingMessage();
            message.Timestamp = DateTime.Now;
            message.LeftDirection = new Vector3(0.3f, 0.1f, 0.5f);
            message.RightDirection = new Vector3(0.3f, 0.1f, 0.5f);
            return message;
        }

        public void Dispose()
        {
        }
    }
}
