namespace EyeTrackingBridge
{
    internal class BrokenEyeSettings
    {
        public string Hostname { get; set; }
        public int Port { get; set; }

        public BrokenEyeSettings()
        {
            Hostname = "127.0.0.1";
            Port = 5555;
        }

        public BrokenEyeSettings(string hostname, int port)
        {
            Hostname = hostname;
            Port = port;
        }
    }
}
