using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

namespace RPGCore.Behaviour
{
	[EditorType]
	[TypeConverter(typeof(LocalIdConverter))]
	[DebuggerDisplay("{ToString(),nq}")]
	public readonly struct LocalId : IEquatable<LocalId>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static readonly Random random = new Random();

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static readonly LocalId None = new LocalId(0);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly ulong id;

		public LocalId(string id)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				this.id = 0;
				return;
			}

			if (id.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
			{
				id = id.Substring(2);
			}
			this.id = ulong.Parse(id, NumberStyles.HexNumber);
		}

		public LocalId(ulong id)
		{
			this.id = id;
		}

		public override bool Equals(object obj)
		{
			return obj is LocalId id && Equals(id);
		}

		public bool Equals(LocalId other)
		{
			return id == other.id;
		}

		public override int GetHashCode()
		{
			return 2108858624 + id.GetHashCode();
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			return "0x" + id.ToString("x8");
		}

		public static LocalId NewId()
		{
			byte[] buffer = new byte[8];
			random.NextBytes(buffer);
			return new LocalId(BitConverter.ToUInt64(buffer, 0));
		}

		public static LocalId NewShortId()
		{
			byte[] buffer = new byte[4];
			random.NextBytes(buffer);
			return new LocalId(BitConverter.ToUInt32(buffer, 0));
		}

		public static bool operator ==(LocalId left, LocalId right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(LocalId left, LocalId right)
		{
			return !(left == right);
		}

		public static implicit operator LocalId(ulong source)
		{
			return new LocalId(source);
		}
	}

	public sealed class LocalIdJsonConverter : JsonConverter
	{
		public override bool CanWrite => true;
		public override bool CanRead => true;

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(LocalId);
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			writer.WriteValue(value.ToString());
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			return new LocalId(reader.Value.ToString());
		}
	}

	internal sealed class LocalIdConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string stringValue = value as string;

			return !string.IsNullOrEmpty(stringValue)
				? new LocalId(stringValue)
				: base.ConvertFrom(context, culture, value);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			var localId = (LocalId)value;

			return localId != null && destinationType == typeof(string)
				? localId.ToString()
				: base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
