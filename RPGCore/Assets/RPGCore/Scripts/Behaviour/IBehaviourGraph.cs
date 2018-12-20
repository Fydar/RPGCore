using System;
using System.Collections.Generic;

namespace RPGCore.Behaviour
{
	public interface IBehaviourGraph
	{
		List<BehaviourNode> AllNodes { get; set; }
	}

	public static class IBehaviourGraphExtensions
	{
		public static T GetNode<T> (this IBehaviourGraph graph)
			where T : BehaviourNode
		{
			return (T)graph.GetNode (typeof (T));
		}

		public static BehaviourNode GetNode (this IBehaviourGraph graph, Type nodeType)
		{
			foreach (BehaviourNode node in graph.AllNodes)
			{
				if (nodeType.IsAssignableFrom (node.GetType ()))
				{
					return node;
				}
			}
			return null;
		}

		public static T[] GetNodes<T> (this IBehaviourGraph graph)
			where T : class
		{
			List<T> foundNodes = new List<T> ();

			foreach (BehaviourNode node in graph.AllNodes)
			{
				if (typeof (T).IsAssignableFrom (node.GetType ()))
				{
					foundNodes.Add ((T)(object)node);
				}
			}
			return foundNodes.ToArray ();
		}

		public static void SetupGraph (this IBehaviourGraph graph, IBehaviourContext context)
		{
			foreach (BehaviourNode node in graph.AllNodes)
			{
				node.SetupContext (context);
			}
		}

		public static void RemoveGraph (this IBehaviourGraph graph, IBehaviourContext context)
		{
			foreach (BehaviourNode node in graph.AllNodes)
			{
				BehaviourNode behaviourNode = node;

				behaviourNode.RemoveContext (context);

				foreach (OutputSocket data in node.Outputs)
				{
					if (data == null)
						continue;

					data.RemoveContext (context);
				}

				foreach (InputSocket data in node.Inputs)
				{
					if (data == null)
						continue;

					data.RemoveContext (context);
				}
			}
		}
	}
}
