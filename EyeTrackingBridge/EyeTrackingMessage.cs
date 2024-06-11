using System;
using System.Numerics;

namespace EyeTrackingBridge
{
    internal struct EyeTrackingMessage
    {
        public DateTime Timestamp = new DateTime();
        public Vector3 LeftDirection = Vector3.UnitZ;
        public Vector3 RightDirection = Vector3.UnitZ;
        public float LeftOpenness = 1.0f;
        public float RightOpenness = 1.0f;

        public EyeTrackingMessage() { }
    }
}
