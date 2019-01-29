using System;
using System.Reflection;

namespace Behaviour.Manifest
{
	public struct TypeConversion
	{
		public string Name;
		public string Description;
		public string Type;

		public static TypeConversion Construct(Type type, MethodInfo method)
		{
			var typeConversion = new TypeConversion
			{
				Name = type.Name + " -> " + method.ReturnType.Name,
				Description = method.Name,
				Type = method.ReturnType.Name
			};

			return typeConversion;
		}
	}
}
