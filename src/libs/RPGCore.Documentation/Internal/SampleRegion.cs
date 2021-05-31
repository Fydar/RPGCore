namespace RPGCore.Documentation.Internal
{
	internal class SampleRegion
	{
		public string Name { get; }
		public string[] Lines { get; }

		public SampleRegion(string name, string[] lines)
		{
			Name = name;
			Lines = lines;
		}
	}
}
