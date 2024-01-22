namespace HandyCook.Application.Services
{
    public class CountdownTimerService
    {
        private Timer _timer;
        public TimeSpan? TimeLeft;
        public event Action<TimeSpan?> OnTick;
        public event Action OnCompleted;
        private int period = 1000;

        public void StartTimer(TimeSpan? duration = null)
        {
            if (duration is not null)
                TimeLeft = duration;
            _timer = new Timer(Callback, null, period, period);
        }

        private void Callback(object state)
        {
            if (TimeLeft.Value.TotalSeconds > 0)
            {
                TimeLeft = TimeLeft.Value.Subtract(TimeSpan.FromMilliseconds(period * 60));
                OnTick?.Invoke(TimeLeft);
            }
            else
            {
                _timer?.Dispose();
                OnCompleted?.Invoke();
            }
        }

        public TimeSpan? StopTimer()
        {
            _timer?.Change(Timeout.Infinite, Timeout.Infinite);
            return TimeLeft;
        }

        ~CountdownTimerService()
        {
            _timer?.Dispose();
        }
    }
}
