namespace EyeTrackingBridge
{
    internal struct AvatarParameters
    {
        public float LeftInOut = 0.0f;
        public float RightInOut = 0.0f;
        public float AverageYaw = 0.0f;
        public float Pitch = 0.0f;
        public bool LocalIsOpenLeft = true;
        public bool LocalIsOpenRight = true;
        public float LocalLeftOpenness = 1.0f;
        public float LocalRightOpenness = 1.0f;
        public bool RemoteIsOpenLeft = true;
        public bool RemoteIsOpenRight = true;
        public int BlinkCounter = 0;

        public AvatarParameters() { }
    }
}
