namespace RPGCore.Traits
{
	public class StatInstance
	{
		public float CurrentValue;

		public string Name { get => null; set {} }
		public string Identifier { get; set; }

		public StatTemplate Template;

		public override string ToString()
		{
			return Identifier;
		}
	}
}
