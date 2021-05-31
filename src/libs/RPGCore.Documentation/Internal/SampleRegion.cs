namespace RPGCore.Documentation.Internal
{
	internal class SampleRegion
	{
		public string Name { get; }
		public CodeSpan[][] Lines { get; }

		public SampleRegion(string name, CodeSpan[][] lines)
		{
			Name = name;
			Lines = lines;
		}
	}
}
