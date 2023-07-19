using BlazorSessionScopedContainer.Contracts.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSessionScopedContainer.Contracts.Migration
{
	public interface IMigrationContext
	{
		T RetrieveData<T>(SessionType sessionType, Guid sessionId, object[] dependencies);
		void Save<T>(T instance, string propertyName, SessionType sessionType, Guid sessionId);
	}
}
