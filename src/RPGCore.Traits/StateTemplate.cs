namespace RPGCore.Traits
{
	public class StateTemplate
	{
		public float MaxValue;

		public string Name { get; set; }
		public string Identifier { get; set; }

		public override string ToString ()
		{
			return Name;
		}
	}
}
