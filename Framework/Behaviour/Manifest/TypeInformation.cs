using System;
using System.Collections.Generic;
using System.Reflection;

namespace Behaviour.Manifest
{
	public struct TypeInformation
	{
		public string Name;
		public string Description;
		public TypeConversion[] Conversions;

		public static TypeInformation Construct(Type type)
		{
			var typeInformation = new TypeInformation
			{
				Name = type.Name
			};
			var conversions = new List<TypeConversion>();
			
			var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);

			foreach(var method in methods)
			{
				if (method.Name == "op_Implicit")
				{
					conversions.Add(TypeConversion.Construct(method));
				}
				else if (method.Name == "op_Explicit")
				{
					conversions.Add(TypeConversion.Construct(method));
				}
			}

			typeInformation.Conversions = conversions.ToArray();
			return typeInformation;
		}
	}
}
