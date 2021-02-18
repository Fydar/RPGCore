namespace RPGCore.World.CommandLine.Components
{
	public struct TransformComponent
	{
		public int InnerValue;

		public override string ToString()
		{
			return $"T:{InnerValue}";
		}
	}
}
