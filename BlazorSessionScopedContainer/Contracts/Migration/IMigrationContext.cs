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
		T RetrieveData<T>(Dictionary<string, dynamic> arguments, object[] dependencies);
		void Save<T>(T instance, string propertyName, Dictionary<string, dynamic> arguments);
	}
}
