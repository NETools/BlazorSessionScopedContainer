using BlazorSessionScopedContainer.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSessionScopedContainer.Statics
{
	internal static class ReflectionHelpers
	{
		public static IEnumerable<MethodInfo> GetMethodsWithAttribute(Type type, Type attributeType)
		{
			var currentMethods = type.GetMethods();
			foreach (var method in currentMethods)
			{
				if (method.CustomAttributes.Any(p => p.AttributeType.Equals(attributeType)))
				{
					yield return method;
				}
			}
		}
	}
}
