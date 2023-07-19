using BlazorSessionScopedContainer.Attributes;
using BlazorSessionScopedContainer.Contracts.Migration;
using BlazorSessionScopedContainer.Contracts.Services;
using BlazorSessionScopedContainer.Contracts.Sessions;
using BlazorSessionScopedContainer.Contracts.Sessions.Auth;
using BlazorSessionScopedContainer.Core.SessionManager.MigrationContext;
using BlazorSessionScopedContainer.Models.Session;
using BlazorSessionScopedContainer.Statics;
using System.ComponentModel;

namespace BlazorSessionScopedContainer.Core.SessionManager
{
	internal class BasicSessionManager : ISessionManager
	{
		private readonly Dictionary<SessionType, SessionType[]> _sessionTypeMappings = new Dictionary<SessionType, SessionType[]>()
		{
			{ SessionType.Volatile, new SessionType[]{ SessionType.Volatile, SessionType.Global } },
			{ SessionType.Global, new SessionType[]{ SessionType.Global } },
			{ SessionType.Authenticated, new SessionType[]{ SessionType.Authenticated, SessionType.Global, SessionType.Volatile } }
		};

		private Dictionary<SessionType, Dictionary<Guid, List<IServiceEntry>>> _serviceContainer = new();
		private IMigrationContext _migrationContext = new BasicMigrationContext();

		public void AddAuthService<T>(SessionId sessionId, SessionId authId) where T : class, IAuthenticationScoped
		{
			var sessionGuid = InternalExposure.RetrieveGuid(sessionId);
			var authenticationGuid = InternalExposure.RetrieveGuid(authId);

			if (!sessionGuid.HasValue)
				return;

			if (!authenticationGuid.HasValue)
				return;

			PrepareSession(SessionType.Authenticated, authenticationGuid.Value);

			if (CheckExistence<T>(SessionType.Authenticated, authenticationGuid.Value))
				return;

			_serviceContainer
				[SessionType.Authenticated]
				[authenticationGuid.Value]
				.Add(new ServiceEntry<T>(this, () =>
				{
					return GetSessionInstance<T>(SessionType.Volatile,
						new()
						{
							{ SessionType.Authenticated, authenticationGuid.Value },
							{ SessionType.Volatile, sessionGuid.Value },
							{ SessionType.Global, Guid.Empty }
						}
						, _migrationContext, new object[0]);
				}));
		}

		public void AddGlobalService<T>(params object[] args) where T : class, INotifyPropertyChanged
        {
			PrepareSession(SessionType.Global, Guid.Empty);

			if (CheckExistence<T>(SessionType.Global, Guid.Empty))
				return;

			_serviceContainer
				[SessionType.Global]
				[Guid.Empty]
				.Add(new ServiceEntry<T>(this, () =>
				{
					return GetSessionInstance<T>(SessionType.Volatile,
						new()
						{
							{ SessionType.Global, Guid.Empty }
						}
						, _migrationContext, args);
				}));
		}

		public void AddGlobalService<T, U>(params object[] args) where T : class, INotifyPropertyChanged where U : class, T
		{
			PrepareSession(SessionType.Global, Guid.Empty);

			if (CheckExistence<T>(SessionType.Global, Guid.Empty))
				return;

			_serviceContainer
				[SessionType.Global]
				[Guid.Empty]
				.Add(new ServiceInterfaceEntry<T, U>(this, () =>
				{
					return GetSessionInstance<U>(SessionType.Volatile,
						new()
						{
							{ SessionType.Global, Guid.Empty }
						}
						, _migrationContext, args);
				}));
		}

		public void AddVolatileService<T>(SessionId sessionId,  params object[] args) where T : class, INotifyPropertyChanged
        {
			var sessionGuid = InternalExposure.RetrieveGuid(sessionId);
			if (!sessionGuid.HasValue)
				return;

			PrepareSession(SessionType.Volatile, sessionGuid.Value);

			if (CheckExistence<T>(SessionType.Volatile, sessionGuid.Value))
				return;

			_serviceContainer
				[SessionType.Volatile]
				[sessionGuid.Value]
				.Add(new ServiceEntry<T>(this, () =>
				{
					return GetSessionInstance<T>(SessionType.Volatile,
						new()
						{
							{ SessionType.Volatile, sessionGuid.Value },
							{ SessionType.Global, Guid.Empty }
						}
						, _migrationContext, args);
				}));
		}

		public void AddVolatileService<T, U>(
			SessionId sessionId,
			params object[] args) where T : class, INotifyPropertyChanged where U : class, T 
		{
			var sessionGuid = InternalExposure.RetrieveGuid(sessionId);
			if (!sessionGuid.HasValue)
				return;

			PrepareSession(SessionType.Volatile, sessionGuid.Value);

			if (CheckExistence<T>(SessionType.Volatile, sessionGuid.Value))
				return;

			_serviceContainer
				[SessionType.Volatile]
				[sessionGuid.Value]
				.Add(new ServiceInterfaceEntry<T, U>(this, () =>
			{
				return GetSessionInstance<U>(SessionType.Volatile, 
					new () 
					{ 
						{ SessionType.Volatile, sessionGuid.Value },
						{ SessionType.Global, Guid.Empty }
					}
					, _migrationContext, args);
			}));
		}

		private T GetSessionInstance<T>(SessionType sessionType, Dictionary<SessionType, Guid> sessionIds, IMigrationContext migrationContext, object[] args) where T : INotifyPropertyChanged
		{
			var serviceType = typeof(T);
			var dependentTypes = InternalExposure.GetDependencies<T>();
			var requiredServiceInstances = new List<object>();

			foreach (var type in dependentTypes)
			{
				foreach (var searchId in _sessionTypeMappings[sessionType])
				{
					var serviceInstance = _serviceContainer[searchId][sessionIds[searchId]].Find(p => p.AreServicesEqual(type));
					if (serviceInstance != null)
						requiredServiceInstances.Add(serviceInstance.GetServiceInstance());
				}
			}

			var instance = migrationContext.RetrieveData<T>(sessionType, sessionIds[sessionType], requiredServiceInstances.ToArray());
 
			var appropiateMethods = ReflectionHelpers.GetMethodsWithAttribute(serviceType, typeof(OnSessionInitialize));
			if (appropiateMethods.Any())
				appropiateMethods.First().Invoke(instance, args);

			instance.PropertyChanged += (sender, e) =>
			{
				migrationContext.Save<T>(instance, e.PropertyName, sessionType, sessionIds[sessionType]);
			};

			return instance;
		}

		private void PrepareSession(SessionType sessionType, Guid sessionId)
		{
			if (!_serviceContainer.ContainsKey(sessionType))
			{
				_serviceContainer.Add(sessionType, new Dictionary<Guid, List<IServiceEntry>>());
			}
			if (!_serviceContainer[sessionType].ContainsKey(sessionId))
			{
				_serviceContainer[sessionType].Add(sessionId, new List<IServiceEntry>());
			}
		}

		private bool CheckExistence<T>(SessionType sessionType, Guid id)
		{
			return _serviceContainer[sessionType][id].Exists(p => p.AreServicesEqual<T>());
		}

		public void SetCurrentMigrationContext(IMigrationContext context)
		{
			_migrationContext = context;
		}

		public T? GetService<T>(SessionType sessionType, SessionId sessionId) where T : class, INotifyPropertyChanged
		{
            var sessionGuid = InternalExposure.RetrieveGuid(sessionId);
            if (!sessionGuid.HasValue)
                return null;

            if (!_serviceContainer.ContainsKey(sessionType))
				return null;
			if (!_serviceContainer[sessionType].ContainsKey(sessionGuid.Value))
				return null;

			return (T?)_serviceContainer[sessionType][sessionGuid.Value].Find(p => p.AreServicesEqual<T>())?.GetServiceInstance();
		}
	}
}
