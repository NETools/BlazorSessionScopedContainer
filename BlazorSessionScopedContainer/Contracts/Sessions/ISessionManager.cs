using BlazorSessionScopedContainer.Contracts.Migration;
using BlazorSessionScopedContainer.Contracts.Services;
using BlazorSessionScopedContainer.Contracts.Sessions.Auth;
using BlazorSessionScopedContainer.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSessionScopedContainer.Contracts.Sessions
{
	public enum SessionType
	{
		Global,
		Volatile,
		Authenticated
	}
	public interface ISessionManager
	{
		void AddGlobalService<T>(params object[] args) where T : class, INotifyPropertyChanged;
		void AddVolatileService<T>(SessionId sessionId,  params object[] args) where T : class, INotifyPropertyChanged;
		void AddAuthService<T>(SessionId sessionId, SessionId authId) where T : class, IAuthenticationScoped;

		void AddGlobalService<T, U>(params object[] args) where T : class, INotifyPropertyChanged where U : class, T;
		void AddVolatileService<T, U>(SessionId sessionId,  params object[] args) where T : class, INotifyPropertyChanged where U : class, T;

		void SetCurrentMigrationContext(IMigrationContext context);

		T? GetService<T>(SessionType sessionType, SessionId sessionId) where T : class, INotifyPropertyChanged;
	}
}
