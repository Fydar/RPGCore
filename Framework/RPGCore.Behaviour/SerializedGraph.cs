using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace RPGCore.Behaviour
{
	public struct SerializedGraph
	{
		public string Name;
		public string Description;
		public string Type;

		public Dictionary<string, string> CustomData;
		public Dictionary<LocalId, SerializedNode> Nodes;

		public Graph Unpack ()
		{
			var nodes = new List<Node> (Nodes.Count);

			var outputIds = new List<string> ();
			foreach (var nodeKvp in Nodes)
			{
				var node = nodeKvp.Value;
				var nodeType = System.Type.GetType (node.Type);

				if (nodeType == null)
				{
					throw new InvalidOperationException($"Unable to unpack node \"{nodeKvp.Key}\" of type \"{node.Type}\" on the graph ${Name}. Type could not be resolved.");
				}

				foreach (var field in nodeType.GetFields (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
				{
					if (field.FieldType == typeof (OutputSocket))
					{
						outputIds.Add (nodeKvp.Key + "." + field.Name);
					}
				}
			}
			int outputCounter = -1;
			foreach (var nodeKvp in Nodes)
			{
				nodes.Add(nodeKvp.Value.Unpack (nodeKvp.Key, outputIds, ref outputCounter));
			}

			var graph = new Graph (nodes.ToArray (), outputIds.Count);
			return graph;
		}
	}
}
