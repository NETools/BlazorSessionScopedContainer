using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSessionScopedContainer.Core.SessionManager.MigrationContext.Synchronization
{
    public class NSMutex
    {
        private SemaphoreSlim _semaphore;

        private Thread? _enteredThread;
        private int _count;

        public NSMutex()
        {
            _semaphore = new SemaphoreSlim(1, 1);
        }

        public MutexLockObject GetLock()
        {
            if (_enteredThread != Thread.CurrentThread)
                _semaphore.Wait();
            _count++;
            _enteredThread = Thread.CurrentThread;
            var mutexLock = new MutexLockObject();
            mutexLock.Disposed += MutexLockDisposed;
            return mutexLock;
        }

        private void MutexLockDisposed()
        {
            if (_enteredThread == Thread.CurrentThread)
                _count--;

            if (_count == 0)
            {
                _enteredThread = null;
                _semaphore.Release();
            }
        }
    }
}
