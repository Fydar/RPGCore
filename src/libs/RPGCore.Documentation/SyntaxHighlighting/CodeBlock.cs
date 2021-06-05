namespace RPGCore.Documentation.SyntaxHighlighting
{
	public class CodeBlock
	{
		public string Name { get; }
		public CodeSpan[][] Lines { get; }

		public CodeBlock(string name, CodeSpan[][] lines)
		{
			Name = name;
			Lines = lines;
		}
	}
}
