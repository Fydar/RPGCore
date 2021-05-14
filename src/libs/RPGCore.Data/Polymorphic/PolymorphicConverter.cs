using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RPGCore.Data.Polymorphic
{
	// Can be used on fields to list all types that this field could be.

	// Can be used on types to indiciate that 

	public class PolymorphicConverter : JsonConverter<object>
	{
		public override bool CanConvert(Type typeToConvert)
		{
			object[] attributes = typeToConvert.GetCustomAttributes(typeof(SerializeTypeAttribute), false);

			return attributes.Length != 0;
		}

		public override object? Read(
			ref Utf8JsonReader reader,
			Type typeToConvert,
			JsonSerializerOptions options)
		{
			if (reader.TokenType == JsonTokenType.Null)
			{
				return null;
			}

			if (reader.TokenType != JsonTokenType.StartObject)
			{
				throw new JsonException("JsonTokenType.StartObject not found.");
			}

			if (!reader.Read()
				|| reader.TokenType != JsonTokenType.PropertyName
				|| reader.GetString() != "$type")
			{
				throw new JsonException("Property $type not found.");
			}

			if (!reader.Read()
				|| reader.TokenType != JsonTokenType.String)
			{
				throw new JsonException("Value at $type is invalid.");
			}

			string assemblyQualifiedName = reader.GetString();

			var type = Type.GetType(assemblyQualifiedName);

			using var output = new MemoryStream();
			ReadObject(ref reader, output, options);

			return JsonSerializer.Deserialize(output.ToArray(), type, options);
		}

		private void ReadObject(ref Utf8JsonReader reader, Stream output, JsonSerializerOptions options)
		{
			using var writer = new Utf8JsonWriter(output, new JsonWriterOptions
			{
				Encoder = options.Encoder,
				Indented = false,
				SkipValidation = true
			});
			writer.WriteStartObject();
			int objectIntend = 0;

			while (reader.Read())
			{
				switch (reader.TokenType)
				{
					case JsonTokenType.None:
					case JsonTokenType.Null:
						writer.WriteNullValue();
						break;

					case JsonTokenType.StartObject:
						writer.WriteStartObject();
						objectIntend++;
						break;

					case JsonTokenType.EndObject:
						writer.WriteEndObject();
						if (objectIntend == 0)
						{
							writer.Flush();
							return;
						}
						objectIntend--;
						break;

					case JsonTokenType.StartArray:
						writer.WriteStartArray();
						break;
					case JsonTokenType.EndArray:
						writer.WriteEndArray();
						break;

					case JsonTokenType.PropertyName:
						writer.WritePropertyName(reader.GetString() ?? "");
						break;
					case JsonTokenType.Comment:
						writer.WriteCommentValue(reader.GetComment());
						break;

					case JsonTokenType.String:
						writer.WriteStringValue(reader.GetString());
						break;

					case JsonTokenType.Number:
						writer.WriteStringValue("");
						writer.Flush();

						output.Position -= 2;

						for (int i = 0; i < reader.ValueSpan.Length; i++)
						{
							byte val = reader.ValueSpan[i];
							output.WriteByte(val);
						}
						output.Flush();
						break;

					case JsonTokenType.True:
					case JsonTokenType.False:
						writer.WriteBooleanValue(reader.GetBoolean());
						break;

					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}

		public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
		{
			writer.WriteStartObject();
			var valueType = value.GetType();
			var valueAssemblyName = valueType.Assembly.GetName();
			writer.WriteString("$type", $"{valueType.FullName}, {valueAssemblyName.Name}");

			string? json = JsonSerializer.Serialize(value, value.GetType(), options);
			using (var document = JsonDocument.Parse(json, new JsonDocumentOptions
			{
				AllowTrailingCommas = options.AllowTrailingCommas,
				MaxDepth = options.MaxDepth
			}))
			{
				foreach (var jsonProperty in document.RootElement.EnumerateObject())
				{
					jsonProperty.WriteTo(writer);
				}
			}

			writer.WriteEndObject();
		}
	}
}
