using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSessionScopedContainer.Contracts.Sessions.Auth
{
    public interface ICredential 
	{
        Dictionary<string, string> UserData { get; }
		string CredentialId { get; }
		bool IsActivated { get; }

        void Activate();
        bool ValidateCredentials(ICredential credential);

    }
}
