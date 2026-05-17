using System;
using System.Windows.Forms;

namespace SudokuGame
{
    public class SudokuTimer
    {
        private readonly Timer _timer;
        private DateTime _startTime;
        private TimeSpan _elapsed = TimeSpan.Zero;
        private bool _isRunning;

        public TimeSpan Elapsed => _isRunning
            ? _elapsed + (DateTime.Now - _startTime)
            : _elapsed;

        public event Action Tick;

        public SudokuTimer()
        {
            _timer = new Timer { Interval = 1000 };
            _timer.Tick += (s, e) => Tick?.Invoke();
        }

        public void Start()
        {
            if (_isRunning) return;
            _startTime = DateTime.Now;
            _isRunning = true;
            _timer.Start();
        }

        public void Stop()
        {
            if (!_isRunning) return;
            _elapsed += DateTime.Now - _startTime;
            _isRunning = false;
            _timer.Stop();
        }

        public void Reset()
        {
            Stop();
            _elapsed = TimeSpan.Zero;
        }

        public override string ToString()
        {
            var t = Elapsed;
            return $"{(int)t.TotalMinutes:D2}:{t.Seconds:D2}";
        }
    }
}
