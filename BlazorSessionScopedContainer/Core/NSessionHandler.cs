using BlazorSessionScopedContainer.Contracts.Migration;
using BlazorSessionScopedContainer.Contracts.Sessions;
using BlazorSessionScopedContainer.Core.SessionManager;
using BlazorSessionScopedContainer.Core.SessionManager.MigrationContext;
using System.ComponentModel;

namespace BlazorSessionScopedContainer.Core
{
    public class NSessionHandler
    {
        internal ISessionManager SessionManager { get; private set; } = new BasicSessionManager();

        public Action<string> Logger { get; set; } = (message) =>
        {
            Console.WriteLine(message);
        };

        private NSessionHandler()
        {
        }

        public void SetSessionManager(ISessionManager sessionManager)
        {
			SessionManager = sessionManager;
        }

        private static NSessionHandler _sessionHandler;
        internal static NSessionHandler Default()
        {
            if (_sessionHandler == null)
            {
                _sessionHandler = new NSessionHandler();

            }
            return _sessionHandler;
        }


        public void AddVolatileService<T>(SessionId sessionId, params object[] args) where T : class, INotifyPropertyChanged
        {
            SessionManager.AddVolatileService<T>(sessionId, args);
        }

        public void AddGlobalService<T>(params object[] args) where T : class, INotifyPropertyChanged
        {
            SessionManager.AddGlobalService<T>(args);
        }

        public void AddVolatileService<T, U>(SessionId sessionId, params object[] args) where T : class, INotifyPropertyChanged where U : class, T
        {
            SessionManager.AddVolatileService<T, U>(sessionId, args);
        }

        public void AddGlobalService<T, U>(params object[] args) where T : class, INotifyPropertyChanged where U : class, T
        {
            SessionManager.AddGlobalService<T, U>(args);
        }

        public void SetCurrentMigrationContext(IMigrationContext context)
        {
            if (context == null)
                SessionManager.SetCurrentMigrationContext(new BasicMigrationContext());
            else
                SessionManager.SetCurrentMigrationContext(context);
        }


    }
}
