namespace RPGCore.Traits
{
	public class StatTemplate
	{
		public string Name { get; set; }
		public string Description { get; set; }

		public float? MinValue { get; set; }
		public float? MaxValue { get; set; }

		public override string ToString ()
		{
			return $"StatTemplate({Name})";
		}
	}
}
