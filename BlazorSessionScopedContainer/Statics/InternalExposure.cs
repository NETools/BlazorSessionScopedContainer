using BlazorSessionScopedContainer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSessionScopedContainer.Statics
{
	public static class InternalExposure
	{
		public static Guid? RetrieveGuid(SessionId sessionId) => sessionId.Guid;

		public static IEnumerable<Type> GetDependencies<T>()
		{
			var serviceType = typeof(T);

			var ctors = serviceType.GetConstructors();
			if (ctors.Length > 1)
			{
				throw new InvalidOperationException();
			}

			var ctor = ctors[0];
			var ctorParams = ctor.GetParameters();

            foreach (var argument in ctorParams)
            {
				yield return argument.ParameterType;
            }
        }
	}
}
