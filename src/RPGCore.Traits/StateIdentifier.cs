using System;
using System.Collections.Generic;

namespace RPGCore.Traits
{
	public readonly struct StateIdentifier : IEquatable<StateIdentifier>
	{
		public string Identifer { get; }

		public StateIdentifier(string identifier)
		{
			Identifer = identifier;
		}

		public override string ToString()
		{
			return Identifer;
		}

		public override bool Equals(object obj) => obj is StateIdentifier identifier && Equals(identifier);

		public bool Equals(StateIdentifier other) => Identifer == other.Identifer;

		public override int GetHashCode() => 1924603977 + EqualityComparer<string>.Default.GetHashCode(Identifer);

		public static bool operator ==(StateIdentifier left, StateIdentifier right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(StateIdentifier left, StateIdentifier right)
		{
			return !(left == right);
		}

		public static implicit operator StateIdentifier(string source)
		{
			return new StateIdentifier(source);
		}
	}
}
