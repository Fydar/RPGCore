using System;
using System.Collections.Generic;

namespace RPGCore.Traits;

public readonly struct StatIdentifier : IEquatable<StatIdentifier>
{
	public string Identifer { get; }

	public StatIdentifier(string identifier)
	{
		Identifer = identifier;
	}

	/// <inheritdoc/>
	public override string ToString()
	{
		return Identifer;
	}

	/// <inheritdoc/>
	public override bool Equals(object obj)
	{
		return obj is StatIdentifier identifier && Equals(identifier);
	}

	public bool Equals(StatIdentifier other)
	{
		return Identifer == other.Identifer;
	}

	/// <inheritdoc/>
	public override int GetHashCode()
	{
		return 1924603977 + EqualityComparer<string>.Default.GetHashCode(Identifer);
	}

	public static bool operator ==(StatIdentifier left, StatIdentifier right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(StatIdentifier left, StatIdentifier right)
	{
		return !(left == right);
	}

	public static implicit operator StatIdentifier(string source)
	{
		return new StatIdentifier(source);
	}
}
