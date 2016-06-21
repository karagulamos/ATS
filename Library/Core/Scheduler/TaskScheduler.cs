using System;
using System.Threading;

namespace Library.Core.Scheduler
{
    public abstract class TaskScheduler
    {
        private Thread _thread;
        private CancellationTokenSource _cancellation;
        private ManualResetEvent _exited;

        protected abstract TimeSpan FirstInterval { get; }

        protected abstract TimeSpan SubsequentInterval { get; }

        protected abstract void PeriodicTask();

        protected virtual void LastTask() { }

        public bool IsRunning { get { return _cancellation != null && !_cancellation.IsCancellationRequested; } }

        public virtual void Start(bool backgroundThread = false)
        {
            if (_thread != null)
                throw new InvalidOperationException(string.Format("\"Start\" called multiple times ({0})", GetType().Name));

            _exited = new ManualResetEvent(false);
            _cancellation = new CancellationTokenSource();
            _thread = new Thread(ThreadProc) { IsBackground = backgroundThread };
            _thread.Start();
        }

        private volatile bool _periodicTaskRunning = false;

        public virtual bool Shutdown(bool waitForExit)
        {
            if (waitForExit && _periodicTaskRunning && Thread.CurrentThread.ManagedThreadId == _thread.ManagedThreadId)
                throw new InvalidOperationException("Cannot call Shutdown(true) from within PeriodicTask() on the same thread (this would cause a deadlock).");

            if (_cancellation == null || _cancellation.IsCancellationRequested)
                return false;

            _cancellation.Cancel();

            if (waitForExit) 
                _exited.WaitOne();

            return true;
        }

        private void ThreadProc()
        {
            try
            {
                _cancellation.Token.WaitHandle.WaitOne(FirstInterval);

                while (!_cancellation.IsCancellationRequested)
                {
                    _periodicTaskRunning = true;
                    PeriodicTask();
                    _periodicTaskRunning = false;
                    _cancellation.Token.WaitHandle.WaitOne(SubsequentInterval);
                }
            }
            finally
            {
                try { LastTask(); }
                finally { _exited.Set(); }
            }
        }
    }
}
