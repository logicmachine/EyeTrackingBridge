using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EyeTrackingBridge
{
    internal class EyeMonitorViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private float _x = 0;
        private float _y = 0;
        private float _openness = 0;

        public float X
        {
            get { return _x; }
            set
            {
                if (_x == value) { return; }
                _x = value;
                OnPropertyChanged();
                OnPropertyChanged("XString");
            }
        }

        public float Y
        {
            get { return _y; }
            set
            {
                if (_y == value) { return; }
                _y = value;
                OnPropertyChanged();
                OnPropertyChanged("YString");
            }
        }

        public float Openness
        {
            get { return _openness; }
            set
            {
                if (_openness == value) { return; }
                _openness = value;
                OnPropertyChanged();
                OnPropertyChanged("OpennessString");
            }
        }

        public string XString
        {
            get { return _x.ToString("0.00"); }
        }

        public string YString
        {
            get { return _y.ToString("0.00"); }
        }

        public string OpennessString
        {
            get { return _openness.ToString("0.00"); }
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    internal class MonitorViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public EyeMonitorViewModel LeftEye { get; } = new EyeMonitorViewModel();
        public EyeMonitorViewModel RightEye { get; } = new EyeMonitorViewModel();

        private float _frameRate = 0;

        public float FrameRate
        {
            get { return _frameRate; }
            set
            {
                if (_frameRate == value) { return; }
                _frameRate = value;
                OnPropertyChanged();
                OnPropertyChanged("FrameRateString");
            }
        }

        public string FrameRateString
        {
            get { return _frameRate.ToString("0.00"); }
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    internal class OscSettingsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private string _address = "127.0.0.1";
        private int _port = 9000;

        public string Address
        {
            get { return _address; }
            set
            {
                if (_address == value) { return; }
                _address = value;
                OnPropertyChanged();
            }
        }

        public int Port {
            get { return _port; }
            set
            {
                if (_port == value) { return; }
                _port = value;
                OnPropertyChanged();
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    internal class BrokenEyeSettingsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private string _address = "127.0.0.1";
        private int _port = 5555;

        public string Address
        {
            get { return _address; }
            set
            {
                if (_address == value) { return; }
                _address = value;
                OnPropertyChanged();
            }
        }

        public int Port {
            get { return _port; }
            set
            {
                if (_port == value) { return; }
                _port = value;
                OnPropertyChanged();
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    internal class DirectionLimitsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private float _in = 40.0f;
        private float _out = 40.0f;
        private float _up = 25.0f;
        private float _down = 25.0f;

        public float In
        {
            get { return _in; }
            set
            {
                if (_in == value) { return; }
                _in = value;
                OnPropertyChanged();
            }
        }

        public float Out
        {
            get { return _out; }
            set
            {
                if (_out == value) { return; }
                _out = value;
                OnPropertyChanged();
            }
        }

        public float Up
        {
            get { return _up; }
            set
            {
                if (_up == value) { return; }
                _up = value;
                OnPropertyChanged();
            }
        }

        public float Down
        {
            get { return _down; }
            set
            {
                if (_down == value) { return; }
                _down = value;
                OnPropertyChanged();
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    internal class OpennessThresholdsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private float _close = 0.25f;
        private float _open = 0.75f;

        public float Close
        {
            get { return _close; }
            set
            {
                if (_close == value) { return; }
                _close = value;
                OnPropertyChanged();
            }
        }

        public float Open
        {
            get { return _open; }
            set
            {
                if (_open == value) { return; }
                _open = value;
                OnPropertyChanged();
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    internal class TransitionTimesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private float _close = 0.03f;
        private float _open = 0.05f;

        public float Close
        {
            get { return _close; }
            set
            {
                if (_close == value) { return; }
                _close = value;
                OnPropertyChanged();
            }
        }

        public float Open
        {
            get { return _open; }
            set
            {
                if (_open == value) { return; }
                _open = value;
                OnPropertyChanged();
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    internal class BlinkSettingsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private float _threshold = 0.2f;

        public float Threshold
        {
            get { return _threshold; }
            set
            {
                if (_threshold == value) { return; }
                _threshold = value;
                OnPropertyChanged();
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    internal class SettingsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public OscSettingsViewModel OscSettings { get; } = new OscSettingsViewModel();
        public BrokenEyeSettingsViewModel BrokenEyeSettings { get; } = new BrokenEyeSettingsViewModel();
        public DirectionLimitsViewModel DirectionLimits { get; } = new DirectionLimitsViewModel();
        public OpennessThresholdsViewModel OpennessThresholds { get; } = new OpennessThresholdsViewModel();
        public TransitionTimesViewModel TransitionTimes { get; } = new TransitionTimesViewModel();
        public BlinkSettingsViewModel BlinkSettings { get; } = new BlinkSettingsViewModel();
    }

    internal class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public MonitorViewModel Monitor { get; } = new MonitorViewModel();
        public SettingsViewModel Settings { get; } = new SettingsViewModel();
    }
}
