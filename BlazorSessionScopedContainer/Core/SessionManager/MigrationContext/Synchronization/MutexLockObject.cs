using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSessionScopedContainer.Core.SessionManager.MigrationContext.Synchronization
{
    public class MutexLockObject : IDisposable
    {
        public event Action? Disposed;

        public void Dispose()
        {
            Disposed?.Invoke();
        }
    }
}
