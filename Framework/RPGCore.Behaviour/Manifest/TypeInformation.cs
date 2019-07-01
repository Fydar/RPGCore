using System.Collections.Generic;

namespace RPGCore.Behaviour.Manifest
{
	public class TypeInformation
	{
		public string Name;
		public string Description;
		public Dictionary<string, TypeConversion> ImplicitConversions;
	}
}
