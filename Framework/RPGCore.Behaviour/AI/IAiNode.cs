﻿namespace RPGCore.Behaviour
{
	public interface IAiNode : INodeInstance
	{
		int LocalWeight { get; }
		IAiNode Source { get; }
	}
}
