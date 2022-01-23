using System;
using System.Collections.Generic;

namespace RPGCore.Behaviour;

public readonly struct ConnectionSubscription
{
	public readonly INodeInstance Node;
	public readonly List<Action> Callbacks;

	public ConnectionSubscription(INodeInstance node)
	{
		Node = node;
		Callbacks = new List<Action>();
	}

	/// <inheritdoc/>
	public override string ToString()
	{
		return $"{Node?.Template?.Id}: {Node?.Template.GetType().Name}";
	}
}
