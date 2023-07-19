using BlazorSessionScopedContainer.Contracts.Migration;
using BlazorSessionScopedContainer.Contracts.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSessionScopedContainer.Core.SessionManager.MigrationContext
{
    internal class BasicMigrationContext : IMigrationContext
    {
        public T RetrieveData<T>(SessionType sessionType, Guid sessionId, object[] dependencies)
        {
            return (T)Activator.CreateInstance(typeof(T), dependencies);
        }

        public void Save<T>(T instance, string propertyName, SessionType sessionType, Guid sessionId)
        {
            // Discard any changes
        }
    }
}
