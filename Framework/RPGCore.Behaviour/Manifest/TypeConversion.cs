using System;
using System.Reflection;

namespace RPGCore.Behaviour.Manifest
{
	public struct TypeConversion
	{
		public string Description;
		public string Type;

		public static TypeConversion Construct (MethodInfo method)
		{
			var typeConversion = new TypeConversion
			{
				Description = method.Name,
				Type = method.ReturnType.Name
			};

			return typeConversion;
		}
	}
}
