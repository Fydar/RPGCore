namespace RPGCore.World.CommandLine.Components
{
	public struct UnitComponent
	{
		public int InnerValue;

		public override string ToString()
		{
			return $"U:{InnerValue}";
		}
	}
}
