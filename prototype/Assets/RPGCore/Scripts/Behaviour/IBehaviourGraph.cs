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
		public static T GetNode<T>(this IBehaviourGraph graph)
			where T : BehaviourNode
		{
			return (T)graph.GetNode(typeof(T));
		}

		public static BehaviourNode GetNode(this IBehaviourGraph graph, Type nodeType)
		{
			foreach (var node in graph.AllNodes)
			{
				if (nodeType.IsAssignableFrom(node.GetType()))
				{
					return node;
				}
			}
			return null;
		}

		public static T[] GetNodes<T>(this IBehaviourGraph graph)
			where T : class
		{
			var foundNodes = new List<T>();

			foreach (var node in graph.AllNodes)
			{
				if (typeof(T).IsAssignableFrom(node.GetType()))
				{
					foundNodes.Add((T)(object)node);
				}
			}
			return foundNodes.ToArray();
		}

		public static void SetupGraph(this IBehaviourGraph graph, IBehaviourContext context)
		{
			foreach (var node in graph.AllNodes)
			{
				node.SetupContext(context);
			}
		}

		public static void RemoveGraph(this IBehaviourGraph graph, IBehaviourContext context)
		{
			foreach (var node in graph.AllNodes)
			{
				var behaviourNode = node;

				behaviourNode.RemoveContext(context);

				foreach (var data in node.OutputSockets)
				{
					if (data == null)
					{
						continue;
					}

					data.RemoveContext(context);
				}

				foreach (var data in node.InputSockets)
				{
					if (data == null)
					{
						continue;
					}

					data.RemoveContext(context);
				}
			}
		}
	}
}
