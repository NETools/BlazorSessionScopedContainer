using BlazorSessionScopedContainer.Contracts.Migration;
using BlazorSessionScopedContainer.Contracts.Sessions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSessionScopedContainer.Core.SessionManager.MigrationContext
{
    internal class BasicMigrationContext : IMigrationContext
    {
        public T RetrieveData<T>(Dictionary<string, dynamic> arguments, object[] dependencies)
        {
            return (T)Activator.CreateInstance(typeof(T), dependencies);
        }
		public void Save<T>(T instance, string propertyName, Dictionary<string, dynamic> arguments)
		{
			
		}
	}
}
