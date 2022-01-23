using System;

namespace RPGCore.Demo.BoardGame;

public class HierachyHelper
{
	public static TChild Get<TParent, TChild>(TParent parent, ref TChild source)
		where TChild : class, IChildOf<TParent>
	{
		return source;
	}

	public static void Set<TParent, TChild>(TParent parent, ref TChild source, TChild value)
		where TChild : class, IChildOf<TParent>
	{
		if (value == null)
		{
			source = null;
			return;
		}

		if (value.Parent != null)
		{
			throw new InvalidOperationException($"Can't add a {nameof(TChild)} to this {nameof(TParent)} as it belongs to another {nameof(TParent)}.");
		}

		source = value;
		value.Parent = parent;
	}
}
