using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSessionScopedContainer.Contracts.Sessions.Auth
{
    public interface IUser : INotifyPropertyChanged
	{
        ICredential UserCredential { get; }
        Guid UserGuid { get; set; }
        string Role { get; set; }

        void Initialize(ICredential credential, Guid userGuid);
    }
}
