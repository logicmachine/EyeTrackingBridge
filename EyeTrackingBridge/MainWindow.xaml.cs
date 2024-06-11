using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace EyeTrackingBridge
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModel _viewModel = new ViewModel();

        private AvatarParametersSenderSettings _senderSettings = new AvatarParametersSenderSettings();
        private BrokenEyeSettings _trackerSettings = new BrokenEyeSettings();
        private AvatarParametersCalculator _calculator = new AvatarParametersCalculator();

        private CancellationTokenSource _tokenSource;
        private Task _currentWorkerTask;

        public MainWindow()
        {
            InitializeComponent();

            var src = Properties.Settings.Default;
            var dst = _viewModel.Settings;
            dst.OscSettings.Address = src.OscHostname;
            dst.OscSettings.Port = src.OscPort;
            dst.BrokenEyeSettings.Address = src.BrokenEyeHostname;
            dst.BrokenEyeSettings.Port = src.BrokenEyePort;
            dst.DirectionLimits.In = src.DirectionLimitIn;
            dst.DirectionLimits.Out = src.DirectionLimitOut;
            dst.DirectionLimits.Up = src.DirectionLimitUp;
            dst.DirectionLimits.Down = src.DirectionLimitDown;
            dst.OpennessThresholds.Close = src.OpennessThresholdClose;
            dst.OpennessThresholds.Open = src.OpennessThresholdOpen;
            dst.TransitionTimes.Close = src.TransitionTimeClose;
            dst.TransitionTimes.Open = src.TransitionTimeOpen;
            dst.BlinkSettings.Threshold = src.BlinkThreshold;

            LoadSettings();
            DataContext = _viewModel;

            _tokenSource = new CancellationTokenSource();
            _currentWorkerTask = Task.Run(() => RunWorkerAsync(_tokenSource.Token));
        }

        private void LoadSettings()
        {
            var settings = Properties.Settings.Default;
            _senderSettings.Hostname = settings.OscHostname;
            _senderSettings.Port = settings.OscPort;
            _trackerSettings.Hostname = settings.BrokenEyeHostname;
            _trackerSettings.Port = settings.BrokenEyePort;
            _calculator.MaxInAngle = settings.DirectionLimitIn;
            _calculator.MaxOutAngle = settings.DirectionLimitOut;
            _calculator.MaxUpAngle = settings.DirectionLimitUp;
            _calculator.MaxDownAngle = settings.DirectionLimitDown;
            _calculator.CloseThreshold = settings.OpennessThresholdClose;
            _calculator.OpenThreshold = settings.OpennessThresholdOpen;
            _calculator.CloseTransitionTime = settings.TransitionTimeClose;
            _calculator.OpenTransitionTime = settings.TransitionTimeOpen;
            _calculator.BlinkThreshold = settings.BlinkThreshold;
        }

        private void SaveSettings()
        {
            var src = _viewModel.Settings;
            var dst = Properties.Settings.Default;
            dst.OscHostname = src.OscSettings.Address;
            dst.OscPort = src.OscSettings.Port;
            dst.BrokenEyeHostname = src.BrokenEyeSettings.Address;
            dst.BrokenEyePort = src.BrokenEyeSettings.Port;
            dst.DirectionLimitIn = src.DirectionLimits.In;
            dst.DirectionLimitOut = src.DirectionLimits.Out;
            dst.DirectionLimitUp = src.DirectionLimits.Up;
            dst.DirectionLimitDown = src.DirectionLimits.Down;
            dst.OpennessThresholdClose = src.OpennessThresholds.Close;
            dst.OpennessThresholdOpen = src.OpennessThresholds.Open;
            dst.TransitionTimeClose = src.TransitionTimes.Close;
            dst.TransitionTimeOpen = src.TransitionTimes.Open;
            dst.BlinkThreshold = src.BlinkSettings.Threshold;
            dst.Save();
        }

        private async Task RunWorkerAsync(CancellationToken cancellationToken)
        {
            try
            {
                var calculator = _calculator;
                using (var tracker = new BrokenEyeTracker(_trackerSettings))
                // using (var tracker = new DummyEyeTracker())
                using (var sender = new AvatarParametersSender(_senderSettings))
                {
                    await tracker.ConnectAsync(cancellationToken);
                    var timeQueue = new List<DateTime>();
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        var message = await tracker.ReadAsync(cancellationToken);
                        var parameters = calculator.Calculate(message);
                        await sender.SendAsync(parameters, cancellationToken);

                        timeQueue.Add(DateTime.UtcNow);
                        var duration = timeQueue.Last() - timeQueue.First();
                        var fps = (float)((timeQueue.Count - 1) / duration.TotalSeconds);
                        if (timeQueue.Count > 50) { timeQueue.RemoveAt(0); }

                        Dispatcher.Invoke(new Action(() =>
                        {
                            _viewModel.Monitor.LeftEye.X = message.LeftDirection.X;
                            _viewModel.Monitor.LeftEye.Y = message.LeftDirection.Y;
                            _viewModel.Monitor.LeftEye.Openness = message.LeftOpenness;
                            _viewModel.Monitor.RightEye.X = message.RightDirection.X;
                            _viewModel.Monitor.RightEye.Y = message.RightDirection.Y;
                            _viewModel.Monitor.RightEye.Openness = message.RightOpenness;
                            _viewModel.Monitor.FrameRate = fps;
                        }));
                    }
                }
            }
            catch (TaskCanceledException) { }
        }

        private async Task StopWorker()
        {
            if (_currentWorkerTask == null) { return; }
            try
            {
                _tokenSource.Cancel();
                var waitToken = new CancellationTokenSource();
                await _currentWorkerTask.WaitAsync(waitToken.Token);
            }
            catch (OperationCanceledException) { }
            catch (IOException) { }
        }

        private async void Window_Closed(object sender, EventArgs e)
        {
            await StopWorker();
        }

        private async void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            await StopWorker();
            SaveSettings();
            LoadSettings();
            _tokenSource = new CancellationTokenSource();
            _currentWorkerTask = Task.Run(() => RunWorkerAsync(_tokenSource.Token));
        }
    }
}
