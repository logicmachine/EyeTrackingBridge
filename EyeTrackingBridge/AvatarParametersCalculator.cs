using System;
using System.Numerics;

namespace EyeTrackingBridge
{
    internal class AvatarParametersCalculator
    {
        private const float RAD2DEG = 180.0f / MathF.PI;

        public float MaxInAngle { get; set; } = 40.0f;
        public float MaxOutAngle { get; set; } = 40.0f;
        public float MaxUpAngle { get; set; } = 25.0f;
        public float MaxDownAngle { get; set; } = 25.0f;
        public float CloseThreshold { get; set; } = 0.25f;
        public float OpenThreshold { get; set; } = 0.75f;
        public float BlinkThreshold { get; set; } = 0.2f;
        public float CloseTransitionTime { get; set; } = 0.03f;
        public float OpenTransitionTime { get; set; } = 0.05f;

        private bool _lastIsOpenLeft = true;
        private bool _lastIsOpenRight = true;
        private bool _lastIsOpenAny = true;
        private DateTime _closeTimeLeft = DateTime.MinValue;
        private DateTime _closeTimeRight = DateTime.MinValue;
        private DateTime _closeTimeBoth = DateTime.MinValue;
        private DateTime _openTimeLeft = DateTime.MinValue;
        private DateTime _openTimeRight = DateTime.MinValue;
        private DateTime _lastTime = DateTime.MinValue;
        private int _blinkCounter = 0;

        public AvatarParametersCalculator() { }

        private float CalculateYaw(Vector3 dir)
        {
            return MathF.Atan2(dir.X, dir.Z) * -RAD2DEG;
        }

        private float CalculatePitch(Vector3 dir)
        {
            return MathF.Atan2(dir.Y, MathF.Sqrt(dir.X * dir.X + dir.Z * dir.Z)) * RAD2DEG;
        }

        private float RemapInOut(float yaw, float sign)
        {
            yaw *= sign;
            return yaw > 0.0f
                ? MathF.Min(MaxOutAngle, yaw) / MaxOutAngle
                : MathF.Max(-MaxInAngle, yaw) / MaxInAngle;
        }

        private float RemapUpDown(float pitch)
        {
            return pitch > 0.0f
                ? MathF.Min(MaxUpAngle, pitch) / MaxUpAngle
                : MathF.Max(-MaxDownAngle, pitch) / MaxDownAngle;
        }

        private float RemapAverageYaw(float leftYaw, float rightYaw)
        {
            if (leftYaw == 0.0f) { leftYaw = rightYaw; }
            if (rightYaw == 0.0f) { rightYaw = leftYaw; }
            var average = (leftYaw + rightYaw) * 0.5f;
            return average > 0.0f
                ? MathF.Min(MaxOutAngle, average) / MaxOutAngle
                : MathF.Max(-MaxOutAngle, average) / MaxOutAngle;
        }

        public AvatarParameters Calculate(EyeTrackingMessage message)
        {
            var leftYaw = CalculateYaw(message.LeftDirection);
            var rightYaw = CalculateYaw(message.RightDirection);
            var leftPitch = CalculatePitch(message.LeftDirection);
            var rightPitch = CalculatePitch(message.RightDirection);

            if (message.LeftOpenness < CloseThreshold)
            {
                if (_lastIsOpenLeft) { _closeTimeLeft = message.Timestamp; }
                _lastIsOpenLeft = false;
            }
            else if (message.LeftOpenness > OpenThreshold)
            {
                if (!_lastIsOpenLeft) { _openTimeLeft = message.Timestamp; }
                _lastIsOpenLeft = true;
            }

            if (message.RightOpenness < CloseThreshold)
            {
                if (_lastIsOpenRight) { _closeTimeRight = message.Timestamp; }
                _lastIsOpenRight = false;
            }
            else if (message.RightOpenness > OpenThreshold)
            {
                if (!_lastIsOpenRight) { _openTimeRight = message.Timestamp; }
                _lastIsOpenRight = true;
            }

            if (!_lastIsOpenLeft && !_lastIsOpenRight)
            {
                if (_lastIsOpenAny) { _closeTimeBoth = message.Timestamp; }
                _lastIsOpenAny = false;
            }
            else
            {
                if (!_lastIsOpenAny)
                {
                    var duration = message.Timestamp - _closeTimeBoth;
                    if ((float)duration.TotalSeconds < BlinkThreshold) { ++_blinkCounter; }
                }
                _lastIsOpenAny = true;
            }

            var closeDurationLeft = message.Timestamp - _closeTimeLeft;
            var closeDurationRight = message.Timestamp - _closeTimeRight;
            var remoteOpenLeft = _lastIsOpenLeft || closeDurationLeft.TotalSeconds < BlinkThreshold;
            var remoteOpenRight = _lastIsOpenRight || closeDurationRight.TotalSeconds < BlinkThreshold;
            if (!remoteOpenLeft && !_lastIsOpenRight) { remoteOpenRight = false; }
            if (!remoteOpenRight && !_lastIsOpenLeft) { remoteOpenLeft = false; }

            var frameTime = (float)(message.Timestamp - _lastTime).TotalSeconds;
            var localLeftOpenness = _lastIsOpenLeft
                ? ((float)(message.Timestamp - _openTimeLeft).TotalSeconds + frameTime) / OpenTransitionTime
                : 1.0f - ((float)(message.Timestamp - _closeTimeLeft).TotalSeconds + frameTime) / CloseTransitionTime;
            var localRightOpenness = _lastIsOpenRight
                ? ((float)(message.Timestamp - _openTimeRight).TotalSeconds + frameTime) / OpenTransitionTime
                : 1.0f - ((float)(message.Timestamp - _closeTimeRight).TotalSeconds + frameTime) / CloseTransitionTime;
            localLeftOpenness = MathF.Max(0.0f, MathF.Min(localLeftOpenness, 1.0f));
            localRightOpenness = MathF.Max(0.0f, MathF.Min(localRightOpenness, 1.0f));
            _lastTime = message.Timestamp;

            return new AvatarParameters
            {
                LeftInOut = RemapInOut(leftYaw, -1.0f),
                RightInOut = RemapInOut(rightYaw, 1.0f),
                AverageYaw = RemapAverageYaw(leftYaw, rightYaw),
                Pitch = RemapUpDown((leftPitch + rightPitch) * 0.5f),
                LocalIsOpenLeft = _lastIsOpenLeft,
                LocalIsOpenRight = _lastIsOpenRight,
                LocalLeftOpenness = localLeftOpenness,
                LocalRightOpenness = localRightOpenness,
                RemoteIsOpenLeft = remoteOpenLeft,
                RemoteIsOpenRight = remoteOpenRight,
                BlinkCounter = _blinkCounter,
            };
        }
    }
}
