namespace RPGCore.Documentation.SyntaxHighlighting
{
	public readonly struct CodeSpan
	{
		public string Content { get; }
		public string? Style { get; }
		public string? LinkURL { get; }

		public CodeSpan(string content)
		{
			Content = content;
			Style = null;
			LinkURL = null;
		}

		public CodeSpan(string content, string? style)
		{
			Content = content;
			Style = style;
			LinkURL = null;
		}

		public CodeSpan(string content, string? style, string? linkUrl)
		{
			Content = content;
			Style = style;
			LinkURL = linkUrl;
		}
	}
}
