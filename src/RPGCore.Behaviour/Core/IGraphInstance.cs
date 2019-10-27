﻿using System.Collections.Generic;

namespace RPGCore.Behaviour
{
	public interface IGraphInstance
	{
		INodeInstance this[LocalId id] { get; }

		INodeInstance GetNode<T> ();
		void Setup ();
		void Remove ();
		T GetNodeInstance<T> () where T : INodeInstance;
		IEnumerable<T> GetNodeInstances<T> () where T : INodeInstance;
		InputSource GetSource<T> (Input<T> input);
		IEnumerable<OutputSource> GetSource<T> (Output<T> output);
		void SetInput<T> (T input);
	}
}
