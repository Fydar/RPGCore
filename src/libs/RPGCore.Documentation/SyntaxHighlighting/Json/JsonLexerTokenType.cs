namespace RPGCore.Documentation.SyntaxHighlighting.Json
{
	public enum JsonLexerTokenType
	{
		String,
		Numeric,

		Whitespace,
		Newline,
		MultiLineComment,
		LineComment,

		OpenObject,
		CloseObject,
		OpenArray,
		CloseArray,
		Comma,
		Colon,

		Null,
		True,
		False
	}
}
