using RPGCore.DataEditor.CSharp;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace RPGCore.Behaviour
{
	[EditorType]
	public class SerializedGraph
	{
		public Dictionary<LocalId, SerializedNode> Nodes;
		public Dictionary<string, SerializedGraph> SubGraphs;

		public Graph Unpack()
		{
			if (Nodes == null || Nodes.Count == 0)
			{
				return null;
			}
			var nodes = new List<NodeTemplate>(Nodes.Count);

			// Find all valid outputs that inputs can map too.
			var allValidOutputs = new HashSet<LocalPropertyId>();
			foreach (var nodeKvp in Nodes)
			{
				var nodeType = GetType(nodeKvp.Value.Type);

				foreach (var field in nodeType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
				{
					if (field.FieldType == typeof(OutputSocket))
					{
						allValidOutputs.Add(new LocalPropertyId(nodeKvp.Key, field.Name));
					}
				}
			}

			// Deserialize nodes and unpack their inputs.
			var connectionIds = new List<LocalPropertyId>();
			foreach (var nodeKvp in Nodes)
			{
				var nodeType = GetType(nodeKvp.Value.Type);

				nodes.Add(nodeKvp.Value.UnpackNodeAndInputs(nodeType, nodeKvp.Key, allValidOutputs, connectionIds));
			}

			// Tell all the outputs that are being used that they are connected.
			foreach (var node in nodes)
			{
				SerializedNode.UnpackOutputs(connectionIds, node);
			}

			Dictionary<string, Graph> templateSubgraphs = null;
			if (SubGraphs != null && SubGraphs.Count > 0)
			{
				templateSubgraphs = new Dictionary<string, Graph>();
				foreach (var subgraph in SubGraphs)
				{
					templateSubgraphs.Add(subgraph.Key, subgraph.Value.Unpack());
				}
			}

			var graph = new Graph(nodes.ToArray(), connectionIds.Count, templateSubgraphs);
			return graph;
		}

		private static Type GetType(string name)
		{
			Type nodeType = null;
			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				nodeType = assembly.GetType(name);

				if (nodeType != null)
				{
					break;
				}
			}
			if (nodeType == null)
			{
				throw new InvalidOperationException($"Cannot find type \"{name}\".");
			}
			return nodeType;
		}
	}
}
