﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RPGCore.DataEditor.Manifest;

/// <summary>
/// A qualified type name, including template type identifiers.
/// </summary>
public sealed class TypeName : IEquatable<TypeName?>
{
	/// <summary>
	/// Represents an unknown type.
	/// </summary>
	/// <remarks>
	/// Unknown types are used for:
	/// <list type="bullet">
	/// <item>Allowing types to be determined from serialized data.</item>
	/// </list>
	/// </remarks>
	public static TypeName Unknown = new("[Unknown]");

	/// <summary>
	/// Creates a new <see cref="TypeName"/> that represents an array.
	/// </summary>
	/// <param name="elementType">The type of all elements in the array.</param>
	/// <returns>A <see cref="TypeName"/> representing an array.</returns>
	public static TypeName ForArray(TypeName elementType)
	{
		return new TypeName("[Array]", new TypeName[] { elementType });
	}

	/// <summary>
	/// Creates a new <see cref="TypeName"/> that represents a dictionary.
	/// </summary>
	/// <param name="keyType">The type of the key in the dictionary.</param>
	/// <param name="valueType">The value of the values in the dictionary.</param>
	/// <returns>A <see cref="TypeName"/> representing a dictionary.</returns>
	public static TypeName ForDictionary(TypeName keyType, TypeName valueType)
	{
		return new TypeName("[Dictionary]", new TypeName[] { keyType, valueType });
	}

	/// <summary>
	/// A string identifier that is used to identifiy the type template.
	/// </summary>
	public string Identifier { get; set; } = string.Empty;

	/// <summary>
	/// Templated types that make up this type.
	/// </summary>
	public TypeName[] TemplateTypes { get; set; } = Array.Empty<TypeName>();

	/// <summary>
	/// Determines whether the type is nullable.
	/// </summary>
	[JsonIgnore]
	public bool IsNullable { get; set; }

	/// <summary>
	/// Determines whether this type represents a dictionary.
	/// </summary>
	[JsonIgnore]
	public bool IsDictionary => Identifier == "[Dictionary]";

	/// <summary>
	/// Determines whether this type represents a dictionary.
	/// </summary>
	[JsonIgnore]
	public bool IsArray => Identifier == "[Array]";

	/// <summary>
	/// Determines whether this type is unknown.
	/// </summary>
	[JsonIgnore]
	public bool IsUnknown => this == Unknown;

	/// <summary>
	/// Initialises a new instance of <see cref="TypeName"/>
	/// </summary>
	public TypeName()
	{
		Identifier = "[Unknown]";
	}

	/// <summary>
	/// Initialises a new instance of <see cref="TypeName"/> from a <see cref="string"/>.
	/// </summary>
	/// <param name="identifier">An identifier for the type.</param>
	public TypeName(string identifier)
	{
		Identifier = identifier;
	}

	/// <summary>
	/// Initialises a new instance of <see cref="TypeName"/>.
	/// </summary>
	/// <param name="identifier">An identifier for the type.</param>
	/// <param name="templateTypes">Parameters that qualify the templated type.</param>
	public TypeName(string identifier, TypeName[] templateTypes)
	{
		Identifier = identifier;
		TemplateTypes = templateTypes;
	}

	/// <inheritdoc/>
	public override string ToString()
	{
		if (TemplateTypes.Length == 0)
		{
			return IsNullable ? $"{Identifier}?" : Identifier;
		}
		return $"{Identifier}<{string.Join(", ", (object[])TemplateTypes)}>{(IsNullable ? "?" : "")}";
	}

	/// <summary>
	/// Reads a <see cref="string"/> to a <see cref="TypeName"/>.
	/// </summary>
	/// <param name="type">A <see cref="string"/> representing the <see cref="TypeName"/> to be read.</param>
	/// <returns></returns>
	public static TypeName FromString(ReadOnlySpan<char> type)
	{
		static void AppendTemplateType(ReadOnlySpan<char> value, List<TypeName> templateTypes, ref int lastDividerIndex, int i)
		{
			var templateTypeString = value.Slice(lastDividerIndex + 1, i - lastDividerIndex - 1);
			bool isNullable = false;
			if (templateTypeString[templateTypeString.Length - 1] == '?')
			{
				isNullable = true;
				templateTypeString = templateTypeString.Slice(0, templateTypeString.Length - 1);
			}
			var template = FromString(templateTypeString.Trim());
			template.IsNullable = isNullable;
			templateTypes.Add(template);

			lastDividerIndex = i;
		}

		int index = type.IndexOf('<');

		if (index == -1)
		{
			return new TypeName(type.ToString());
		}

		var templateTypes = new List<TypeName>();

		int lastDividerIndex = index;
		int depth = 0;
		for (int i = index + 1; i < type.Length - 1; i++)
		{
			char c = type[i];

			if (c == ',')
			{
				if (depth == 0)
				{
					AppendTemplateType(type, templateTypes, ref lastDividerIndex, i);
				}
			}
			else if (c == '<')
			{
				depth++;
			}
			else if (c == '>')
			{
				depth--;
				if (depth == 0)
				{
					AppendTemplateType(type, templateTypes, ref lastDividerIndex, i);
				}
			}
		}
		AppendTemplateType(type, templateTypes, ref lastDividerIndex, type.Length - 1);

		var identifier = type.Slice(0, index);
		return new TypeName(identifier.ToString(), templateTypes.ToArray());
	}

	/// <inheritdoc/>
	public bool Equals(TypeName? other)
	{
		return other != null &&
			   ToString() == other.ToString();
	}

	/// <inheritdoc/>
	public override bool Equals(object? obj)
	{
		return Equals(obj as TypeName);
	}

	/// <inheritdoc/>
	public override int GetHashCode()
	{
		return ToString().GetHashCode();
	}

	/// <summary>
	/// Determines whether an instance of an object is equal to another object of the same type.
	/// </summary>
	/// <param name="left">An object to compare.</param>
	/// <param name="right">An object to compare <paramref name="left"/> with.</param>
	/// <returns><c>true</c> if <paramref name="left"/> is equal to <paramref name="right"/>; otherwise <c>false</c>.</returns>
	public static bool operator ==(TypeName? left, TypeName? right)
	{
		return string.Equals(left?.ToString() ?? "", right?.ToString() ?? "");
	}

	/// <summary>
	/// Determines whether an instance of an object is <b>not</b> equal to another object of the same type.
	/// </summary>
	/// <param name="left">An object to compare.</param>
	/// <param name="right">An object to compare <paramref name="left"/> with.</param>
	/// <returns><c>true</c> if <paramref name="left"/> is <b>not</b> equal to <paramref name="right"/>; otherwise <c>false</c>.</returns>
	public static bool operator !=(TypeName? left, TypeName? right)
	{
		return !(left == right);
	}
}
