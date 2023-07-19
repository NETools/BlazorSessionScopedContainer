using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSessionScopedContainer.Attributes
{

	[AttributeUsage(AttributeTargets.Property)]
	internal class BufferedCollection : Attribute
	{

	}
}
