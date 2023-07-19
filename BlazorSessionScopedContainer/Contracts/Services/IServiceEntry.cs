using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSessionScopedContainer.Contracts.Services
{
    public interface IServiceEntry
    {
        bool AreServicesEqual<Service>();
        bool AreServicesEqual(Type type);
        object GetServiceInstance();
    }
}
