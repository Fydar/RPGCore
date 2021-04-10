using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

namespace RPGCore.Behaviour
{
	[TypeConverter(typeof(LocalPropertyIdConverter))]
	[DebuggerDisplay("{ToString()}")]
	public readonly struct LocalPropertyId : IEquatable<LocalPropertyId>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static readonly LocalPropertyId None = new LocalPropertyId(LocalId.None, (string[])null);

		public readonly LocalId TargetIdentifier;
		public readonly string[] PropertyPath;

		public LocalPropertyId(string? expression)
		{
			if (string.IsNullOrEmpty(expression))
			{
				TargetIdentifier = LocalId.None;
				PropertyPath = Array.Empty<string>();
				return;
			}

			string[] elements = expression.Split('.');

			TargetIdentifier = new LocalId(elements[0]);
			PropertyPath = new string[elements.Length - 1];
			for (int i = 0; i < elements.Length - 1; i++)
			{
				PropertyPath[i] = elements[i + 1];
			}
		}

		public LocalPropertyId(LocalId targetIdentifier, string property)
		{
			TargetIdentifier = targetIdentifier;
			PropertyPath = property.Split('.');
		}

		public LocalPropertyId(LocalId targetIdentifier, string[] propertyPath)
		{
			TargetIdentifier = targetIdentifier;
			PropertyPath = propertyPath;
		}

		public override bool Equals(object obj)
		{
			return obj is LocalPropertyId id && Equals(id);
		}

		public bool Equals(LocalPropertyId other)
		{
			return TargetIdentifier == other.TargetIdentifier
				&& ArrayEquals(PropertyPath, other.PropertyPath);
		}

		public override int GetHashCode()
		{
			if (PropertyPath != null)
			{
				unchecked
				{
					int hash = 17;

					foreach (string property in PropertyPath)
					{
						hash = hash * 23 + ((property != null)
							? property.GetHashCode()
							: 0);
					}

					return hash;
				}
			}

			return 0;
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			if (this == None)
			{
				return "0x00";
			}
			return TargetIdentifier.ToString() + "." + string.Join(".", PropertyPath);
		}

		public static bool operator ==(LocalPropertyId left, LocalPropertyId right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(LocalPropertyId left, LocalPropertyId right)
		{
			return !(left == right);
		}

		private bool ArrayEquals(string[] left, string[] right)
		{
			int leftLength = left?.Length ?? 0;

			if (leftLength != (right?.Length ?? 0))
			{
				return false;
			}

			for (int i = 0; i < leftLength; i++)
			{
				string leftElement = left[i];
				string rightElement = right[i];

				if (leftElement != rightElement)
				{
					return false;
				}
			}
			return true;
		}
	}

	public sealed class LocalPropertyIdJsonConverter : JsonConverter
	{
		public override bool CanWrite => true;
		public override bool CanRead => true;

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(LocalPropertyId);
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			writer.WriteValue(value.ToString());
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			return new LocalPropertyId(reader.Value?.ToString());
		}
	}

	internal sealed class LocalPropertyIdConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string? stringValue = value as string;

			return !string.IsNullOrEmpty(stringValue)
				? new LocalPropertyId(stringValue)
				: base.ConvertFrom(context, culture, value);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			var localPropertyId = (LocalPropertyId)value;

			return localPropertyId != null && destinationType == typeof(string)
				? localPropertyId.ToString()
				: base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
