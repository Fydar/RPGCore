using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGCore.Data.Polymorphic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace JsonKnownTypes.Polymorphic.Internal
{
	public class PolymorphicConverter : JsonConverter
	{
		private readonly PolymorphicOptions options;
		private readonly Dictionary<Type, PolymorphicBaseTypeInformation> polymorphicBaseCache;

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

		internal PolymorphicConverter(PolymorphicOptions options)
		{
			this.options = options;
			polymorphicBaseCache = new Dictionary<Type, PolymorphicBaseTypeInformation>();
		}

		/// <inheritdoc/>
		public override bool CanConvert(Type objectType)
		{
			var typeInfo = GetPolymorphicBaseType(objectType);

			return typeInfo != null;
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

			var polymorphicBaseType = GetPolymorphicBaseType(objectType);
			if (polymorphicBaseType == null)
			{
				throw new InvalidOperationException();
			}
			var baseTypeInfo = GetPolymorphicBaseTypeInfoForType(polymorphicBaseType);

			var jsonObject = JObject.Load(reader);
			var jsonTypeProperty = jsonObject[options.DescriminatorName];

			string? typeName = jsonTypeProperty?.Value<string>();
			if (typeName == null)
			{
				throw CreateInvalidTypeException(baseTypeInfo, typeName);
			}

			var type = baseTypeInfo.GetTypeForDescriminatorValue(typeName);
			if (type == null)
			{
				throw CreateInvalidTypeException(baseTypeInfo, typeName);
			}

			try
			{
				// Prevent recursive serialization when the types match.
				if (objectType == type)
				{
					isInRead.Value = true;
				}

				var jsonReader = jsonObject.CreateReader();
				return serializer.Deserialize(jsonReader, type);
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

			var polymorphicBaseType = GetPolymorphicBaseType(value.GetType());
			if (polymorphicBaseType == null)
			{
				throw new InvalidOperationException();
			}

			var polymorphicBaseTypeInfo = GetPolymorphicBaseTypeInfoForType(polymorphicBaseType);
			var typeInfo = polymorphicBaseTypeInfo.GetSubTypeInformation(value.GetType());

			var writerProxy = new JsonWriterWithObjectType(options.DescriminatorName, typeInfo?.Name, writer);

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

		private PolymorphicBaseTypeInformation GetPolymorphicBaseTypeInfoForType(Type type)
		{
			if (!polymorphicBaseCache.TryGetValue(type, out var cached))
			{
				cached = new PolymorphicBaseTypeInformation(options, type);
			}
			return cached;
		}

		/// <remarks>
		/// Since Netwonsoft.Json provides us with the actual type (and not the base type) we need to extrapolate a bit.
		/// </remarks>
		private Type? GetPolymorphicBaseType(Type type)
		{
			foreach (var typeInterface in type.GetInterfaces())
			{
				object[] interfaceAttributes = typeInterface.GetCustomAttributes(typeof(SerializeTypeAttribute), true);
				if (interfaceAttributes.Length != 0)
				{
					return typeInterface;
				}
			}

			object[] attributes = type.GetCustomAttributes(typeof(SerializeTypeAttribute), true);
			return attributes.Length == 0 ? null : type;
		}

		private JsonException CreateInvalidTypeException(PolymorphicBaseTypeInformation baseCache, string? typeName)
		{
			var sb = new StringBuilder();
			sb.Append($"\"{options.DescriminatorName}\" value of \"{typeName}\" is invalid.\nValid options for \"{baseCache.BaseType.FullName}\" are:");

			foreach (var validOption in baseCache.SubTypes)
			{
				sb.Append("\n- '");
				sb.Append(validOption.Name);
				sb.Append("'");

				if (validOption.Aliases != null)
				{
					sb.Append(", also known as '");
					sb.Append(string.Join("', '", validOption.Aliases));
				}
				sb.Append("'");
			}

			return new JsonException(sb.ToString());
		}
	}
}
