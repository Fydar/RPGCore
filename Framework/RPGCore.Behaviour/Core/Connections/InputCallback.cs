using System;

namespace RPGCore.Behaviour
{
	public struct InputCallback
	{
		public INodeInstance Node;
		public Action Callback;

		public InputCallback (INodeInstance node, Action callback)
		{
			Node = node;
			Callback = callback;
		}
	}
}
