using System.Collections.Generic;
using System.Reflection;

namespace RPGCore.Traits
{
	public class StateTemplate : IFixedElement
	{
		public float MaxValue;
		
		public string Name { get; set; }
		public string Identifier { get; set; }

		public override string ToString()
		{
			return Name;
		}
	}
}
