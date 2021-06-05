using System;
using System.IO;
using System.Text;
using System.Text.Json;

namespace RPGCore.Documentation.SyntaxHighlighting.Json
{
	public class JsonSyntax
	{
		public const string StylePropertyName = "c-pn";
		public const string StyleKeyword = "c-kw";
		public const string StyleString = "c-s";
		public const string StyleNumber = "c-n";

		public static CodeBlock ToCodeBlocks(string script)
		{
			var output = new CodeBlockBuilder(0);

			byte[]? data = Encoding.UTF8.GetBytes(script);

			// Format the json
			var parsed = JsonDocument.Parse(data);
			using (var ms = new MemoryStream())
			{
				using (var jw = new Utf8JsonWriter(ms, new JsonWriterOptions()
				{
					Indented = true
				}))
				{
					parsed.WriteTo(jw);
				}
				data = ms.ToArray();
			}

			var reader = new Utf8JsonReader(data);

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
