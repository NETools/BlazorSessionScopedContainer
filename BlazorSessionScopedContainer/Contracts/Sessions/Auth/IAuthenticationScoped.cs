using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSessionScopedContainer.Contracts.Sessions.Auth
{
    public interface IAuthenticationScoped : INotifyPropertyChanged
    {
        bool IsAuthorized(IUser user);
    }
}
