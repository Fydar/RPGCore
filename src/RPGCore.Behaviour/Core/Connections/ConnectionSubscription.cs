using System;
using System.Collections.Generic;

namespace RPGCore.Behaviour
{
	public struct ConnectionSubscription
	{
		public INodeInstance Node;
		public List<Action> Callbacks;

		public ConnectionSubscription (INodeInstance node)
		{
			Node = node;
			Callbacks = new List<Action> ();
		}
	}
}
