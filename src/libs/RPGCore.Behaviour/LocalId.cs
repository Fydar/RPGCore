using Newtonsoft.Json;
using RPGCore.Data;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

namespace RPGCore.Behaviour;

[EditableType]
[TypeConverter(typeof(LocalIdConverter))]
[DebuggerDisplay("{ToString(),nq}")]
public readonly struct LocalId : IEquatable<LocalId>
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static readonly Random random = new();

	/// <summary>
	/// A identifier representing a missing ID.
	/// </summary>
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static LocalId None { get; } = new(0);

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly ulong id;

	/// <summary>
	/// Initialises a new instance of the <see cref="LocalId"/>.
	/// </summary>
	/// <param name="id">The raw value of the identifier.</param>
	[JsonConstructor]
	public LocalId(ulong id)
	{
		this.id = id;
	}

	/// <summary>
	/// Tries to convert the string represention of the identifier to a <see cref="LocalId"/>. A return value indicates whether the conversion succeeded of failed.
	/// </summary>
	/// <param name="source">A span that represents the identifer to convert.</param>
	/// <param name="value">When this method returns <c>true</c>, contains the <see cref="LocalId"/> that is equivilant to the one specified in <paramref name="source"/>; otherwise contains <see cref="None"/>.</param>
	/// <returns><c>true</c> if <paramref name="source"/> was converted successfully; otherwise, <c>false</c>.</returns>
	public static bool TryParse(ReadOnlySpan<char> source, out LocalId value)
	{
		if (source.IsEmpty || source.IsWhiteSpace())
		{
			value = None;
			return true;
		}

		if (source.StartsWith("0x".AsSpan(), StringComparison.OrdinalIgnoreCase))
		{
			source = source.Slice(2);
		}
		if (ulong.TryParse(source.ToString(), NumberStyles.HexNumber, null, out ulong result))
		{
			value = new LocalId(result);
			return true;
		}
		else
		{
			value = None;
			return false;
		}
	}

	/// <inheritdoc/>
	public override bool Equals(object obj)
	{
		return obj is LocalId id && Equals(id);
	}

	/// <inheritdoc/>
	public bool Equals(LocalId other)
	{
		return id == other.id;
	}

	/// <inheritdoc/>

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
		if (LocalId.TryParse((reader.Value?.ToString() ?? "").AsSpan(), out var result))
		{
			return result;
		}
		else
		{
			return LocalId.None;
		}
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
		string? stringValue = value as string;

		if (LocalId.TryParse(stringValue.AsSpan(), out var result))
		{
			return result;
		}
		else
		{
			return LocalId.None;
		}
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		var localId = (LocalId)value;

		return localId != null && destinationType == typeof(string)
			? localId.ToString()
			: base.ConvertTo(context, culture, value, destinationType);
	}
}
