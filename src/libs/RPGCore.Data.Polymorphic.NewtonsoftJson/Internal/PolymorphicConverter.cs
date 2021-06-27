using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGCore.Data.Polymorphic.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace RPGCore.Data.Polymorphic.NewtonsoftJson.Internal
{
	/// <summary>
	/// Converts and polymorphic object type to and from JSON.
	/// </summary>
	internal class PolymorphicConverter : JsonConverter
	{
		private readonly PolymorphicConfiguration configuration;

		private readonly ThreadLocal<bool> isInRead = new();
		private readonly ThreadLocal<bool> isInWrite = new();

		/// <inheritdoc/>
		public override bool CanRead
		{
			get
			{
				if (isInRead.Value)
				{
					isInRead.Value = false;
					return false;
				}
				return true;
			}
		}

		/// <inheritdoc/>
		public override bool CanWrite
		{
			get
			{
				if (isInWrite.Value)
				{
					isInWrite.Value = false;
					return false;
				}
				return true;
			}
		}

		internal PolymorphicConverter(PolymorphicConfiguration configuration)
		{
			this.configuration = configuration;
		}

		/// <inheritdoc/>
		public override bool CanConvert(Type objectType)
		{
			return configuration.TryGetSubType(objectType, out _) || configuration.TryGetBaseType(objectType, out _);
		}

		/// <inheritdoc/>
		public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue,
			JsonSerializer serializer)
		{
			while (reader.TokenType == JsonToken.Comment)
			{
				reader.Read();
			}

			if (reader.TokenType == JsonToken.Null)
			{
				return null;
			}

			var baseTypeConfiguration = GetPolymorphicBaseType(objectType);

			var jsonObject = JObject.Load(reader);
			var jsonTypeProperty = jsonObject[configuration.DescriminatorName];

			string? typeName = jsonTypeProperty?.Value<string>();
			if (typeName == null)
			{
				throw CreateInvalidTypeException(baseTypeConfiguration, typeName);
			}

			var subTypeConfiguration = baseTypeConfiguration.GetSubTypeForDescriminator(typeName);
			if (subTypeConfiguration == null)
			{
				throw CreateInvalidTypeException(baseTypeConfiguration, typeName);
			}

			try
			{
				// Prevent recursive serialization when the types match.
				if (objectType == subTypeConfiguration.Type)
				{
					isInRead.Value = true;
				}

				var jsonReader = jsonObject.CreateReader();
				return serializer.Deserialize(jsonReader, subTypeConfiguration.Type);
			}
			finally
			{
				isInRead.Value = false;
			}
		}

		/// <inheritdoc/>
		public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}

			var valueType = value.GetType();

			var baseTypeConfiguration = GetPolymorphicBaseType(valueType);
			if (baseTypeConfiguration == null)
			{
				throw new InvalidOperationException();
			}

			var subTypeConfiguration = baseTypeConfiguration.GetSubTypeForType(valueType);
			if (subTypeConfiguration == null)
			{
				throw new InvalidOperationException();
			}

			var writerProxy = new JsonWriterWithObjectType(configuration.DescriminatorName, subTypeConfiguration.Name, writer);

			try
			{
				isInWrite.Value = true;
				serializer.Serialize(writerProxy, value);
			}
			finally
			{
				isInWrite.Value = false;
			}
		}

		private PolymorphicConfigurationBaseType GetPolymorphicBaseType(Type objectType)
		{
			if (configuration.TryGetBaseType(objectType, out var baseTypeInfo))
			{
				return baseTypeInfo;
			}

			if (!configuration.TryGetSubType(objectType, out var subTypeInfo))
			{
				throw new JsonException($"Cannot determine a base-type for '{objectType.FullName}'.");
			}

			if (subTypeInfo.Count == 1)
			{
				return subTypeInfo[0].BaseType;
			}
			else
			{
				throw CreateAmbigiousBaseTypeException(objectType, subTypeInfo);
			}
		}

		private JsonException CreateInvalidTypeException(PolymorphicConfigurationBaseType baseCache, string? typeName)
		{
			var sb = new StringBuilder();
			sb.Append($"\"{configuration.DescriminatorName}\" value of \"{typeName}\" is invalid.\nValid options for \"{baseCache.BaseType.FullName}\" are:");

			foreach (var validOption in baseCache.SubTypes.Values)
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

		private JsonException CreateAmbigiousBaseTypeException(Type subType, List<PolymorphicConfigurationSubType> subTypeInfos)
		{
			var sb = new StringBuilder();
			sb.Append($"The sub-type '{subType.FullName}' has multiple base-types.\nCannot select a base-type between:");

			foreach (var basetype in subTypeInfos)
			{
				sb.Append("\n- '");
				sb.Append(basetype.BaseType.BaseType.FullName);
				sb.Append('\'');
			}

			return new JsonException(sb.ToString());
		}
	}
}
