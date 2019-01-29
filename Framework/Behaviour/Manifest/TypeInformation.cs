using System;
using System.Collections.Generic;

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
			
			//var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);

			typeInformation.Conversions = conversions.ToArray();
			return typeInformation;
		}
	}
}
