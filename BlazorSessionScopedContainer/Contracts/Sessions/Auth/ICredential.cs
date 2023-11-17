using BlazorSessionScopedContainer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSessionScopedContainer.Contracts.Sessions.Auth
{
    public interface ICredential 
	{
        List<CredentialSlot> UserData { get; set; }
		string CredentialId { get; }
		bool IsActivated { get; }

        CredentialSlot FindSlot(string credentialName);

    }
}
