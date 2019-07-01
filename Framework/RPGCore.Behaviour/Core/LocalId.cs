using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Globalization;

namespace RPGCore.Behaviour
{
	[TypeConverter (typeof (LocalIdConverter))]
	public struct LocalId
	{
		public static readonly LocalId None = new LocalId (0);

		private readonly ulong Id;

		public LocalId (string id)
		{
			Id = ulong.Parse (id, NumberStyles.HexNumber);
		}

		public LocalId (ulong id)
		{
			Id = id;
		}

		public override string ToString ()
		{
			return Id.ToString ("x8");
		}

		public override int GetHashCode ()
		{
			return Id.GetHashCode ();
		}

		public static bool operator == (LocalId left, LocalId right)
		{
			return left.Id == right.Id;
		}

		public static bool operator != (LocalId left, LocalId right)
		{
			return left.Id != right.Id;
		}
	}

	internal class LocalIdJsonConverter : JsonConverter
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

	public class LocalIdConverter : TypeConverter
	{
		public override bool CanConvertFrom (ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof (string) || base.CanConvertFrom (context, sourceType);
		}

		public override object ConvertFrom (ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string stringValue = value as string;

			if (!string.IsNullOrEmpty (stringValue))
			{
				return new LocalId (stringValue);
			}

			return base.ConvertFrom (context, culture, value);
		}

		public override object ConvertTo (ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			var localId = (LocalId)value;

			if (localId != null && destinationType == typeof (string))
			{
				return localId.ToString ();
			}

			return base.ConvertTo (context, culture, value, destinationType);
		}
	}
}