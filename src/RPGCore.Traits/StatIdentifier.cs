using System;
using System.Collections.Generic;

namespace RPGCore.Traits
{
	public struct StatIdentifier : IEquatable<StatIdentifier>
	{
		public string Identifer { get; }

		public StatIdentifier(string identifier)
		{
			Identifer = identifier;
		}

		public override string ToString()
		{
			return Identifer;
		}

		public override bool Equals(object obj) => obj is StatIdentifier identifier && Equals (identifier);
		public bool Equals(StatIdentifier other) => Identifer == other.Identifer;
		public override int GetHashCode() => 1924603977 + EqualityComparer<string>.Default.GetHashCode (Identifer);

		public static bool operator ==(StatIdentifier left, StatIdentifier right)
		{
			return left.Equals (right);
		}

		public static bool operator !=(StatIdentifier left, StatIdentifier right)
		{
			return !(left == right);
		}

		public static implicit operator StatIdentifier(string source)
		{
			return new StatIdentifier (source);
		}
	}
}
