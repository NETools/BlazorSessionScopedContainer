using BlazorSessionScopedContainer.Contracts.Services;
using BlazorSessionScopedContainer.Contracts.Sessions;
using BlazorSessionScopedContainer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSessionScopedContainer.Models.Session
{
    internal class ServiceEntry<T> : IServiceEntry
	{
		private Lazy<T> _service;
		public Type ConcreteType { get; set; }
		public ServiceEntry(ISessionManager handler, Func<T> serviceInit)
		{
			ConcreteType = typeof(T);

			_service = new Lazy<T>(() =>
			{
				return serviceInit();
			});
		}

		public virtual bool AreServicesEqual<Service>()
		{
			return typeof(Service).Equals(ConcreteType);
		}

		public virtual bool AreServicesEqual(Type type)
		{
			return type.Equals(ConcreteType);
		}

		public object GetServiceInstance()
		{
			return _service.Value;
		}
	}
}
