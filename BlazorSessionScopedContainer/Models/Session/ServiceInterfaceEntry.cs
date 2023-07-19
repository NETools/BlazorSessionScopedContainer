using BlazorSessionScopedContainer.Contracts.Sessions;
using BlazorSessionScopedContainer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSessionScopedContainer.Models.Session
{
	internal class ServiceInterfaceEntry<Interface, Concrete> : ServiceEntry<Concrete>
		where Concrete : Interface
	{
		Type InterfaceType { get; set; }
		public ServiceInterfaceEntry(ISessionManager sessionManager, Func<Concrete> serviceInit) : base(sessionManager, serviceInit)
		{
			InterfaceType = typeof(Interface);
		}

		public override bool AreServicesEqual<Service>()
		{
			return typeof(Service).Equals(InterfaceType);
		}

		public override bool AreServicesEqual(Type type)
		{
			return type.Equals(InterfaceType);
		}
	}
}
