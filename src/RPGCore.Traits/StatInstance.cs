using System.Reflection;

namespace RPGCore.Traits
{
	public class StatInstance : IFixedElement, ITemplatedElement<StatTemplate>
	{
		public float CurrentValue;

		public string Name { get => null; set {} }
		public string Identifier { get; set; }

		public StatTemplate Template;

		public void SetTemplate(StatTemplate template)
		{
			Template = template;
		}

		public override string ToString()
		{
			return Identifier;
		}
	}
}
