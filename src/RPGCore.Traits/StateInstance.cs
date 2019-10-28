namespace RPGCore.Traits
{
	public class StateInstance
	{
		public float CurrentValue;

		public string Name { get => null; set { } }
		public string Identifier { get; set; }

		public StateTemplate Template;

		public override string ToString ()
		{
			return Identifier;
		}
	}
}
