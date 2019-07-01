using System;
using System.Collections.Generic;
using System.Reflection;

namespace RPGCore.Behaviour.Manifest
{
	public struct TypeInformation
	{
		public string Description;
		public Dictionary<string, TypeConversion> ImplicitConversions;

		public static TypeInformation Construct (Type type)
		{
			var typeInformation = new TypeInformation ();
			var conversions = new Dictionary<string, TypeConversion> ();

			var methods = type.GetMethods (BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);

			foreach (var method in methods)
			{
				if (method.Name == "op_implicit")
				{
					conversions.Add (method.ReturnType.FullName, TypeConversion.Construct (method));
				}
			}

			typeInformation.ImplicitConversions = conversions;
			return typeInformation;
		}
	}
}
