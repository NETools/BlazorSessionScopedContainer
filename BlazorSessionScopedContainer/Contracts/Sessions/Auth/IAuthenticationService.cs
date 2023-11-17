using BlazorSessionScopedContainer.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSessionScopedContainer.Contracts.Sessions.Auth
{
	public interface INAuthenticationService<User> where User : class, IUser
	{
		public User CurrentUser(Guid? sessionId);
        public bool IsSessionLoggedIn(Guid? session);
    }
}
