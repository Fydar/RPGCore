using System;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace RPGCore.Documentation.SyntaxHighlighting.Json
{
	public class JsonSyntax
	{
		public const string StylePropertyName = "c-pn";
		public const string StyleKeyword = "c-kw";
		public const string StyleString = "c-s";
		public const string StyleNumber = "c-n";
		public const string StyleComment = "c-c";

		public static CodeBlock ToCodeBlocks(string script)
		{
			var output = new CodeBlockBuilder(0);

			byte[]? data = Encoding.UTF8.GetBytes(script);

			// Format the json
			using var formatted = new MemoryStream();
			var formatReader = new Utf8JsonReader(data, new JsonReaderOptions()
			{
				CommentHandling = JsonCommentHandling.Allow
			});
			using (var formatWriter = new Utf8JsonWriter(formatted, new JsonWriterOptions()
			{
				Indented = true,
				Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
			}))
			{
				while (formatReader.Read())
				{
					switch (formatReader.TokenType)
					{
						case JsonTokenType.None:
						case JsonTokenType.Null:
							formatWriter.WriteNullValue();
							break;
						case JsonTokenType.StartObject:
							formatWriter.WriteStartObject();
							break;
						case JsonTokenType.EndObject:
							formatWriter.WriteEndObject();
							break;
						case JsonTokenType.StartArray:
							formatWriter.WriteStartArray();
							break;
						case JsonTokenType.EndArray:
							formatWriter.WriteEndArray();
							break;
						case JsonTokenType.PropertyName:
							formatWriter.WritePropertyName(formatReader.GetString() ?? "");
							break;
						case JsonTokenType.Comment:
							formatWriter.WriteCommentValue(formatReader.GetComment());
							break;
						case JsonTokenType.String:
							formatWriter.WriteStringValue(formatReader.GetString());
							break;
						case JsonTokenType.Number:
							formatWriter.WriteStringValue("");
							formatWriter.Flush();

							formatted.Position -= 2;

							for (int i = 0; i < formatReader.ValueSpan.Length; i++)
							{
								byte val = formatReader.ValueSpan[i];
								formatted.WriteByte(val);
							}
							formatted.Flush();
							break;

						case JsonTokenType.True:
						case JsonTokenType.False:
							formatWriter.WriteBooleanValue(formatReader.GetBoolean());
							break;
					}
				}
			}

			data = formatted.ToArray();
			var reader = new Utf8JsonReader(data, new JsonReaderOptions()
			{
				CommentHandling = JsonCommentHandling.Allow
			});

			int lastEndIndex = 0;
			while (reader.Read())
			{
				switch (reader.TokenType)
				{
					case JsonTokenType.PropertyName:
					{
						InsertUnstyledText(output, data, reader.TokenStartIndex, lastEndIndex);

						output.Write("\"" + Encoding.UTF8.GetString(reader.ValueSpan) + "\"", StylePropertyName);
						lastEndIndex = (int)reader.TokenStartIndex + reader.ValueSpan.Length + 2;
						break;
					}
					case JsonTokenType.Null:
					case JsonTokenType.True:
					case JsonTokenType.False:
					{
						InsertUnstyledText(output, data, reader.TokenStartIndex, lastEndIndex);

						output.Write(Encoding.UTF8.GetString(reader.ValueSpan), StyleKeyword);
						lastEndIndex = (int)reader.TokenStartIndex + reader.ValueSpan.Length;
						break;
					}
					case JsonTokenType.String:
					{
						InsertUnstyledText(output, data, reader.TokenStartIndex, lastEndIndex);

						output.Write("\"" + Encoding.UTF8.GetString(reader.ValueSpan) + "\"", StyleString);
						lastEndIndex = (int)reader.TokenStartIndex + reader.ValueSpan.Length + 2;
						break;
					}
					case JsonTokenType.Number:
					{
						InsertUnstyledText(output, data, reader.TokenStartIndex, lastEndIndex);

						output.Write(Encoding.UTF8.GetString(reader.ValueSpan), StyleNumber);
						lastEndIndex = (int)reader.TokenStartIndex + reader.ValueSpan.Length;
						break;
					}
					case JsonTokenType.Comment:
					{
						InsertUnstyledText(output, data, reader.TokenStartIndex, lastEndIndex);

						output.Write("/*" + Encoding.UTF8.GetString(reader.ValueSpan) + "*/", StyleComment);
						lastEndIndex = (int)reader.TokenStartIndex + reader.ValueSpan.Length + 4;
						break;
					}
				}
			}
			InsertUnstyledText(output, data, data.Length, lastEndIndex);

			return output.Build();
		}

		private static void InsertUnstyledText(CodeBlockBuilder builder, byte[] data, long currentIndex, int lastEndIndex)
		{
			if (lastEndIndex != currentIndex)
			{
				var missingText = data.AsSpan(lastEndIndex, (int)(currentIndex - lastEndIndex));
				builder.Write(Encoding.UTF8.GetString(missingText));
			}
		}
	}
}
