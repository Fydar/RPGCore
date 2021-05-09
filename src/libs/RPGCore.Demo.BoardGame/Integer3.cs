using System;

namespace RPGCore.Demo.BoardGame
{
	[Serializable]
	public readonly struct Integer3 : IEquatable<Integer3>
	{
		public static readonly Integer3 zero = new(0, 0, 0);
		public static readonly Integer3 one = new(1, 1, 1);

		public static readonly Integer3 up = new(0, 1, 0);
		public static readonly Integer3 down = new(0, -1, 0);
		public static readonly Integer3 left = new(-1, 0, 0);
		public static readonly Integer3 right = new(1, 0, 0);
		public static readonly Integer3 forward = new(0, 0, 1);
		public static readonly Integer3 back = new(0, 0, -1);

		public readonly int x;
		public readonly int y;
		public readonly int z;

		public Integer3(int x, int y, int z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		/// <inheritdoc/>
		public override bool Equals(object obj)
		{
			return obj is Integer3 integer3 && Equals(integer3);
		}

		public bool Equals(Integer3 other)
		{
			return x == other.x &&
				   y == other.y &&
				   z == other.z;
		}

		/// <inheritdoc/>
		public override int GetHashCode()
		{
			int hashCode = 373119288;
			hashCode = hashCode * -1521134295 + x.GetHashCode();
			hashCode = hashCode * -1521134295 + y.GetHashCode();
			hashCode = hashCode * -1521134295 + z.GetHashCode();
			return hashCode;
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			return string.Format("{0:0}, {1:0}, {2:0}", x, y, z);
		}

		public static Integer3 operator +(Integer3 left, Integer3 right)
		{
			return new Integer3(left.x + right.x, left.y + right.y, left.z + right.z);
		}

		public static Integer3 operator -(Integer3 left, Integer3 right)
		{
			return new Integer3(left.x - right.x, left.y - right.y, left.z - right.z);
		}

		public static Integer3 operator *(Integer3 left, Integer3 right)
		{
			return new Integer3(left.x * right.x, left.y * right.y, left.z * right.z);
		}

		public static Integer3 operator /(Integer3 left, Integer3 right)
		{
			return new Integer3(left.x / right.x, left.y / right.y, left.z / right.z);
		}

		public static bool operator ==(Integer3 left, Integer3 right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Integer3 left, Integer3 right)
		{
			return !(left == right);
		}
	}
}
