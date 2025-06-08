using System;
using System.Diagnostics;
using System.Threading;

namespace UnityODE
{
    class HighPrecisionTimer : IDisposable
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private readonly long _intervalTicks;
        private readonly Action _callback;
        private Thread _timerThread;
        private bool _running;

        public HighPrecisionTimer(TimeSpan interval, Action callback, ThreadPriority priority = ThreadPriority.Highest)
        {
            _intervalTicks = interval.Ticks;
            _callback = callback;

            _running = true;
            _timerThread = new Thread(TimerLoop) { Priority = priority };
            _stopwatch.Start();
            _timerThread.Start();
        }

        public void Dispose()
        {
            _running = false;
            if (!_timerThread.Join(TimeSpan.FromTicks(_intervalTicks * 2)))
                _timerThread.Abort();

            _stopwatch.Stop();
            _stopwatch.Reset();
        }

        private void TimerLoop()
        {
            long nextTrigger = _stopwatch.Elapsed.Ticks + _intervalTicks;

            while (_running)
            {
                long currentTicks = _stopwatch.Elapsed.Ticks;

                if (currentTicks >= nextTrigger)
                {
                    _callback.Invoke();
                    nextTrigger += _intervalTicks;

                    while (nextTrigger < currentTicks)
                    {
                        nextTrigger += _intervalTicks;
                    }
                }

                if (nextTrigger - currentTicks > TimeSpan.TicksPerMillisecond / 10)
                    Thread.Sleep(1);
                else
                    Thread.SpinWait(100);
            }
        }
    }
}