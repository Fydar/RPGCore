using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

namespace RPGCore.Behaviour
{
	[TypeConverter (typeof (LocalIdConverter))]
	[DebuggerDisplay ("{ToString(),nq}")]
	public struct LocalId : IEquatable<LocalId>
	{
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		public static readonly LocalId None = new LocalId (0);

		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		private readonly ulong Id;

		public LocalId (string id)
		{
			if (string.IsNullOrWhiteSpace (id))
			{
				Id = 0;
				return;
			}

			if (id.StartsWith ("0x", StringComparison.OrdinalIgnoreCase))
			{
				id = id.Substring (2);
			}
			Id = ulong.Parse (id, NumberStyles.HexNumber);
		}

		public LocalId (ulong id)
		{
			Id = id;
		}

		public override bool Equals (object obj)
		{
			return obj is LocalId id && Equals (id);
		}

		public bool Equals (LocalId other)
		{
			return Id == other.Id;
		}

		public override int GetHashCode ()
		{
			return 2108858624 + Id.GetHashCode ();
		}

		public override string ToString ()
		{
			return "0x" + Id.ToString ("x8");
		}

		public static bool operator == (LocalId left, LocalId right)
		{
			return left.Equals (right);
		}

		public static bool operator != (LocalId left, LocalId right)
		{
			return !(left == right);
		}

		public static implicit operator LocalId (ulong source)
		{
			return new LocalId (source);
		}
	}

	public sealed class LocalIdJsonConverter : JsonConverter
	{
		public override bool CanWrite => true;
		public override bool CanRead => true;

		public override bool CanConvert (Type objectType)
		{
			return objectType == typeof (LocalId);
		}

		public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer)
		{
			writer.WriteValue (value.ToString ());
		}

		public override object ReadJson (JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			return new LocalId (reader.Value.ToString ());
		}
	}

	internal sealed class LocalIdConverter : TypeConverter
	{
		public override bool CanConvertFrom (ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof (string) || base.CanConvertFrom (context, sourceType);
		}

		public override object ConvertFrom (ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string stringValue = value as string;

			return !string.IsNullOrEmpty (stringValue)
				? new LocalId (stringValue)
				: base.ConvertFrom (context, culture, value);
		}

		public override object ConvertTo (ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			var localId = (LocalId)value;

			return localId != null && destinationType == typeof (string)
				? localId.ToString ()
				: base.ConvertTo (context, culture, value, destinationType);
		}
	}
}
