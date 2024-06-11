using System;
using System.Threading;
using System.Threading.Tasks;

namespace EyeTrackingBridge
{
    internal interface IEyeTracker : IDisposable
    {
        public Task ConnectAsync(CancellationToken cancellationToken);
        public Task<EyeTrackingMessage> ReadAsync(CancellationToken cancellationToken);
    }
}
