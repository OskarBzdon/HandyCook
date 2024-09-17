namespace HandyCook.Application.Services
{
    public class CountdownTimerService
    {
        private Timer _timer;
        private bool _isRunning = false;
        public TimeSpan? TimeLeft;
        public event Action<TimeSpan?> OnTick;
        public event Action OnCompleted;
        private int period = 1000;

        public void StartTimer(TimeSpan? duration = null)
        {
            if (duration is not null)
                TimeLeft = duration;
            if (_timer is null)
                _timer = new Timer(Callback, null, Timeout.Infinite, Timeout.Infinite);
            _timer.Change(period, period);
            _isRunning = true;
        }

        private void Callback(object state)
        {
            if (TimeLeft.Value.TotalSeconds > 0 && _isRunning)
            {
                TimeLeft = TimeLeft.Value.Subtract(TimeSpan.FromMilliseconds(period * 60));
                OnTick?.Invoke(TimeLeft);
            }
            else if (_isRunning)
            {
                _timer?.Dispose();
                _isRunning = false;
                OnCompleted?.Invoke();
            }
        }

        public TimeSpan? StopTimer()
        {
            _timer?.Change(Timeout.Infinite, Timeout.Infinite);
            _isRunning = false;
            return TimeLeft;
        }

        ~CountdownTimerService()
        {
            _timer?.Dispose();
            _isRunning = false;
        }
    }
}
