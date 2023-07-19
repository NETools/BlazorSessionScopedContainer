using BlazorSessionScopedContainer.Contracts.Services;
using BlazorSessionScopedContainer.Contracts.Sessions;
using BlazorSessionScopedContainer.Contracts.Sessions.Auth;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.Diagnostics;

namespace BlazorSessionScopedContainer.Core
{
    public class NSession
    {
        private IHttpContextAccessor _httpContext;

        private bool _sessionNewlySet;
        private Guid _sessionGuid;

        public NSession(IHttpContextAccessor _httpContext)
        {
            this._httpContext = _httpContext;
        }

        public T? GetVolatileService<T>() where T : class, INotifyPropertyChanged
        {
            var session = GetSession();
            if (!session.HasValue)
                return default;

            return RetrieveService<T>(SessionType.Volatile, session.Value);
        }

        public T? GetGlobalService<T>() where T : class, INotifyPropertyChanged
        {
            return RetrieveService<T>(SessionType.Global, Guid.Empty);
        }

        public T? GetAuthService<T, User>() where T : class, IAuthenticationScoped where User : class, IUser
        {
            var session = GetSession();
            if (!session.HasValue)
                return default;

            var sessionId = new SessionId(session);

            var userManagementService = GetGlobalService<INAuthenticationService<User>>();
            if (userManagementService == null)
                return null;

            if(userManagementService.IsSessionLoggedIn(sessionId))
            {
                var currentUser = userManagementService.CurrentUser(sessionId);
                NSessionHandler.Default().SessionManager.AddAuthService<T>(sessionId, new SessionId(currentUser.UserGuid));
                
                var authenticatedService = RetrieveService<T>(SessionType.Authenticated, currentUser.UserGuid);
                
                if (authenticatedService == null)
                    return null;

                if (authenticatedService.IsAuthorized(currentUser))
                {
                    return authenticatedService;
                }
            }

            return null;
        }

        private T? RetrieveService<T>(SessionType sessionType, Guid id) where T : class, INotifyPropertyChanged
        {
            return (T?)NSessionHandler.Default().SessionManager.GetService<T>(sessionType, new SessionId(id));
        }

        public void StartSession(Action<SessionId, NSessionHandler> initRoutine)
        {
            if (_httpContext.HttpContext?.Request.Cookies.ContainsKey("session") == false)
            {
                _sessionGuid = Guid.NewGuid();
                _httpContext.HttpContext.Response.Cookies.Append("session", $"{_sessionGuid}");
                _sessionNewlySet = true;
            }
            InitializeSession(initRoutine);
        }

        private void InitializeSession(Action<SessionId, NSessionHandler> routine)
        {
            var session = GetSession();
            if (!session.HasValue)
                return;

			var sessionId = new SessionId(session);
            var handler = NSessionHandler.Default();
            routine(sessionId, handler);
        }


        public Guid? GetSession()
        {
            if (_httpContext.HttpContext?.Request.Cookies.ContainsKey("session") == true)
            {
                return new Guid(_httpContext.HttpContext.Request?.Cookies["session"]);
            }

            if (_sessionNewlySet)
                return _sessionGuid;

            return null;
        }
    }
}
