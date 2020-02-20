using System;

namespace RPGCore.Demo.BoardGame
{
	[Serializable]
	public readonly struct Integer2 : IEquatable<Integer2>
	{
		public static readonly Integer2 zero = new Integer2(0, 0);
		public static readonly Integer2 one = new Integer2(1, 1);

		public static readonly Integer2 up = new Integer2(0, 1);
		public static readonly Integer2 down = new Integer2(0, -1);
		public static readonly Integer2 left = new Integer2(-1, 0);
		public static readonly Integer2 right = new Integer2(1, 0);

		public readonly int x;
		public readonly int y;

		public Integer2(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public override bool Equals(object obj)
		{
			return obj is Integer2 integer && Equals(integer);
		}

		public bool Equals(Integer2 other)
		{
			return x == other.x &&
				   y == other.y;
		}

		public override int GetHashCode()
		{
			int hashCode = 1502939027;
			hashCode = hashCode * -1521134295 + x.GetHashCode();
			hashCode = hashCode * -1521134295 + y.GetHashCode();
			return hashCode;
		}

		public override string ToString()
		{
			return string.Format("{0:0}, {1:0}", x, y);
		}

		public static Integer2 operator +(Integer2 left, Integer2 right)
		{
			return new Integer2(left.x + right.x, left.y + right.y);
		}

		public static Integer2 operator -(Integer2 left, Integer2 right)
		{
			return new Integer2(left.x - right.x, left.y - right.y);
		}

		public static Integer2 operator *(Integer2 left, Integer2 right)
		{
			return new Integer2(left.x * right.x, left.y * right.y);
		}

		public static Integer2 operator /(Integer2 left, Integer2 right)
		{
			return new Integer2(left.x / right.x, left.y / right.y);
		}

		public static bool operator ==(Integer2 left, Integer2 right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Integer2 left, Integer2 right)
		{
			return !(left == right);
		}
	}
}
