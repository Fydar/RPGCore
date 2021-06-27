using RPGCore.Data.Polymorphic.Configuration;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RPGCore.Data.Polymorphic.SystemTextJson.Internal
{
	/// <inheritdoc/>
	internal class PolymorphicConverter : JsonConverter<object>
	{
		private readonly PolymorphicConfiguration configuration;
		private readonly PolymorphicConfigurationBaseType baseTypeConfiguration;

		internal PolymorphicConverter(
			PolymorphicConfiguration configuration,
			Type converterType)
		{
			this.configuration = configuration;
			configuration.TryGetBaseType(converterType, out baseTypeConfiguration);
		}

		/// <inheritdoc/>
		public override object? Read(
			ref Utf8JsonReader reader,
			Type typeToConvert,
			JsonSerializerOptions options)
		{
			if (reader.TokenType == JsonTokenType.Null)
			{
				return null;
			}

			var peek = reader;

			if (peek.TokenType != JsonTokenType.StartObject)
			{
				throw new JsonException("JsonTokenType.StartObject not found.");
			}

			if (!peek.Read()
				|| peek.TokenType != JsonTokenType.PropertyName
				|| peek.GetString() != configuration.DescriminatorName)
			{
				throw new JsonException($"Property \"{configuration.DescriminatorName}\" not found.");
			}

			if (!peek.Read()
				|| peek.TokenType != JsonTokenType.String)
			{
				throw new JsonException($"Value at \"{configuration.DescriminatorName}\" is invalid.");
			}

			string? typeName = peek.GetString();

			if (typeName == null)
			{
				throw CreateInvalidTypeException(typeName);
			}

			var subTypeConfiguration = baseTypeConfiguration.GetSubTypeForDescriminator(typeName);
			if (subTypeConfiguration == null)
			{
				throw CreateInvalidTypeException(typeName);
			}

			return JsonSerializer.Deserialize(ref reader, subTypeConfiguration.Type, options);
		}

		/// <inheritdoc/>
		public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
		{
			writer.WriteStartObject();

			var valueType = value.GetType();

			var subTypeConfiguration = baseTypeConfiguration.GetSubTypeForType(valueType);
			if (subTypeConfiguration == null)
			{
				throw new InvalidOperationException($"Cannot serialize value of type '{valueType.FullName}' as it's not one of the allowed types.");
			}

			writer.WriteString(configuration.DescriminatorName, subTypeConfiguration.Name);

			var buffer = new MemoryStream();
			using (var bufferWriter = new Utf8JsonWriter(buffer, new JsonWriterOptions()
			{
				Encoder = options.Encoder,
				Indented = options.WriteIndented
			}))
			{
				JsonSerializer.Serialize(bufferWriter, value, value.GetType(), options);
			}
			buffer.Seek(0, SeekOrigin.Begin);

			using (var document = JsonDocument.Parse(buffer, new JsonDocumentOptions
			{
				AllowTrailingCommas = options.AllowTrailingCommas,
				MaxDepth = options.MaxDepth,
				CommentHandling = options.ReadCommentHandling
			}))
			{
				foreach (var jsonProperty in document.RootElement.EnumerateObject())
				{
					jsonProperty.WriteTo(writer);
				}
			}

			writer.WriteEndObject();
		}

		private JsonException CreateInvalidTypeException(string? typeName)
		{
			var sb = new StringBuilder();
			sb.Append($"\"{configuration.DescriminatorName}\" value of \"{typeName}\" is invalid.\nValid options for \"{baseTypeConfiguration.BaseType.FullName}\" are:");

			foreach (var validOption in baseTypeConfiguration.SubTypes.Values)
			{
				sb.Append("\n- '");
				sb.Append(validOption.Name);
				sb.Append('\'');

				if (validOption.Aliases != null)
				{
					foreach (string alias in validOption.Aliases)
					{
						sb.Append("\n  - '");
						sb.Append(alias);
						sb.Append('\'');
					}
				}
			}

			return new JsonException(sb.ToString());
		}
	}
}
