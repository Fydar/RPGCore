using System.Collections.Generic;
using System.Reflection;

namespace RPGCore.Traits
{
	public class StateInstance : IFixedElement, ITemplatedElement<StateTemplate>
	{
		public float CurrentValue;

		public string Name { get => null; set {} }
		public string Identifier { get; set; }

		public StateTemplate Template;

		public void SetTemplate(StateTemplate template)
		{
			Template = template;
		}

		public override string ToString()
		{
			return Identifier;
		}
	}
}
