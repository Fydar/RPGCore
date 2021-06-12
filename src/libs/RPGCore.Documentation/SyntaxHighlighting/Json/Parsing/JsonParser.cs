using RPGCore.Documentation.SyntaxHighlighting.Internal;
using System.Collections.Generic;

namespace RPGCore.Documentation.SyntaxHighlighting.Json.Parsing
{
	public class JsonParser
	{
		private static readonly JsonLexerLanguage jsonLanguage = new();

		public IEnumerable<JsonNode> Parse(string source)
		{
			var headLocation = new Stack<JsonNodeLocation>();
			headLocation.Push(new JsonNodeLocation(JsonNodeLocation.LocationType.Value));

			var lexer = new Lexer(jsonLanguage);
			foreach (var token in lexer.Tokenize(source))
			{
				var tokenType = (JsonLexerTokenType)token.Classifier;

				switch (tokenType)
				{
					case JsonLexerTokenType.String:
					{
						var peek = headLocation.Peek();
						if (peek.Type == JsonNodeLocation.LocationType.Object)
						{
							yield return new JsonNode(JsonNodeType.PropertyName, token.StartIndex, token.Length);
						}
						else
						{
							yield return new JsonNode(JsonNodeType.StringLiteral, token.StartIndex, token.Length);
						}
						break;
					}
					case JsonLexerTokenType.Numeric:
					{
						yield return new JsonNode(JsonNodeType.NumberLiteral, token.StartIndex, token.Length);
						break;
					}
					case JsonLexerTokenType.Whitespace:
					{
						yield return new JsonNode(JsonNodeType.Whitespace, token.StartIndex, token.Length);
						break;
					}
					case JsonLexerTokenType.Newline:
					{
						yield return new JsonNode(JsonNodeType.Newline, token.StartIndex, token.Length);
						break;
					}
					case JsonLexerTokenType.MultiLineComment:
					{
						yield return new JsonNode(JsonNodeType.MultiLineComment, token.StartIndex, token.Length);
						break;
					}
					case JsonLexerTokenType.LineComment:
					{
						yield return new JsonNode(JsonNodeType.SingleLineComment, token.StartIndex, token.Length);
						break;
					}
					case JsonLexerTokenType.OpenObject:
					{
						headLocation.Push(new JsonNodeLocation(JsonNodeLocation.LocationType.Object));
						yield return new JsonNode(JsonNodeType.StartObject, token.StartIndex, token.Length);
						break;
					}
					case JsonLexerTokenType.CloseObject:
					{
						var peek = headLocation.Peek();
						if (peek.Type == JsonNodeLocation.LocationType.Value)
						{
							headLocation.Pop();
						}

						headLocation.Pop();
						yield return new JsonNode(JsonNodeType.EndObject, token.StartIndex, token.Length);
						break;
					}
					case JsonLexerTokenType.OpenArray:
					{
						headLocation.Push(new JsonNodeLocation(JsonNodeLocation.LocationType.Array));
						yield return new JsonNode(JsonNodeType.StartArray, token.StartIndex, token.Length);
						break;
					}
					case JsonLexerTokenType.CloseArray:
					{
						var peek = headLocation.Peek();
						if (peek.Type == JsonNodeLocation.LocationType.Value)
						{
							headLocation.Pop();
						}

						headLocation.Pop();
						yield return new JsonNode(JsonNodeType.EndArray, token.StartIndex, token.Length);
						break;
					}
					case JsonLexerTokenType.Comma:
					{
						var peek = headLocation.Peek();
						if (peek.Type == JsonNodeLocation.LocationType.Value)
						{
							headLocation.Pop();
						}

						yield return new JsonNode(JsonNodeType.ValueDeliminator, token.StartIndex, token.Length);
						break;
					}
					case JsonLexerTokenType.Colon:
					{
						headLocation.Push(new JsonNodeLocation(JsonNodeLocation.LocationType.Value));
						yield return new JsonNode(JsonNodeType.PropertyValueDeliminator, token.StartIndex, token.Length);
						break;
					}
					case JsonLexerTokenType.Null:
					{
						yield return new JsonNode(JsonNodeType.NullLiteral, token.StartIndex, token.Length);
						break;
					}
					case JsonLexerTokenType.True:
					{
						yield return new JsonNode(JsonNodeType.TrueLiteral, token.StartIndex, token.Length);
						break;
					}
					case JsonLexerTokenType.False:
					{
						yield return new JsonNode(JsonNodeType.FalseLiteral, token.StartIndex, token.Length);
						break;
					}
				}
			}
		}
	}
}
