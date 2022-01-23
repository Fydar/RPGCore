using RPGCore.Documentation.SyntaxHighlighting.Json.Parsing;
using System;

namespace RPGCore.Documentation.SyntaxHighlighting.Json;

public class JsonSyntax
{
	public const string StylePropertyName = "c-pn";
	public const string StyleKeyword = "c-kw";
	public const string StyleString = "c-s";
	public const string StyleEscapedString = "c-es";
	public const string StyleNumber = "c-n";
	public const string StyleComment = "c-c";

	public static CodeBlock ToCodeBlocks(string script)
	{
		var output = new CodeBlockBuilder(0);

		var jsonParser = new JsonParser();
		foreach (var token in jsonParser.Parse(script))
		{
			string tokenContent = script.Substring(token.StartIndex, token.Length);

			switch (token.Type)
			{
				case JsonNodeType.PropertyName:
				{
					WriteWithEscapedHighlighting(output, tokenContent, StylePropertyName, StyleEscapedString);
					break;
				}
				case JsonNodeType.StringLiteral:
				{
					WriteWithEscapedHighlighting(output, tokenContent, StyleString, StyleEscapedString);
					break;
				}
				case JsonNodeType.MultiLineComment:
				case JsonNodeType.SingleLineComment:
				{
					output.Write(tokenContent, StyleComment);
					break;
				}
				case JsonNodeType.NumberLiteral:
				{
					output.Write(tokenContent, StyleNumber);
					break;
				}
				case JsonNodeType.TrueLiteral:
				case JsonNodeType.NullLiteral:
				case JsonNodeType.FalseLiteral:
				{
					output.Write(tokenContent, StyleKeyword);
					break;
				}
				default:
				{
					output.Write(tokenContent);
					break;
				}
			}
		}

		return output.Build();
	}

	private static void WriteWithEscapedHighlighting(CodeBlockBuilder output, ReadOnlySpan<char> content, string style, string escapedString)
	{
		var remaining = content;
		while (true)
		{
			int index = remaining.IndexOf('\\');
			if (index == -1)
			{
				break;
			}
			var upToIndex = remaining[..index];
			output.Write(upToIndex, style);

			int lengthToEscape = 2;
			char nextChar = remaining[index + 1];
			if (nextChar == 'u')
			{
				lengthToEscape = 6;
			}
			output.Write(remaining[new Range(index, index + lengthToEscape)], escapedString);

			remaining = remaining[(index + lengthToEscape)..];
		}
		output.Write(remaining, style);
	}
}
