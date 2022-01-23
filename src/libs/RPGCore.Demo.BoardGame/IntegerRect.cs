using System;

namespace RPGCore.Demo.BoardGame;

[Serializable]
public readonly struct IntegerRect : IEquatable<IntegerRect>
{
	public readonly int x;
	public readonly int y;
	public readonly int width;
	public readonly int height;

	public int xMax => x + width;

	public int yMax => y + height;

	public IntegerRect(int x, int y, int width, int height)
	{
		this.x = x;
		this.y = y;
		this.width = width;
		this.height = height;
	}

	/// <inheritdoc/>
	public override string ToString()
	{
		return string.Format("x: {0:0}, y: {1:0}, width: {1:0}, height: {1:0}", x, y, width, height);
	}

	public override bool Equals(object obj)
	{
		return obj is IntegerRect rect && Equals(rect);
	}

	public bool Equals(IntegerRect other)
	{
		return x == other.x &&
			   y == other.y &&
			   width == other.width &&
			   height == other.height;
	}

	public override int GetHashCode()
	{
		int hashCode = -1222528132;
		hashCode = hashCode * -1521134295 + x.GetHashCode();
		hashCode = hashCode * -1521134295 + y.GetHashCode();
		hashCode = hashCode * -1521134295 + width.GetHashCode();
		hashCode = hashCode * -1521134295 + height.GetHashCode();
		return hashCode;
	}

	public static bool operator ==(IntegerRect left, IntegerRect right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(IntegerRect left, IntegerRect right)
	{
		return !(left == right);
	}
}
