using System.Collections.Generic;

namespace RPGCore.Behaviour
{
	public class SerializedGraph
	{
		public string Name;
		public string Description;
		public string Type;

		public Dictionary<string, string> CustomData;
		public Dictionary<LocalId, SerializedNode> Nodes;

		public Graph Unpack ()
		{
			var nodes = new List<Node> (Nodes.Count);

			var connectionIds = new List<LocalPropertyId> ();
			int outputCounter = -1;
			foreach (var nodeKvp in Nodes)
			{
				nodes.Add (nodeKvp.Value.UnpackInputs (nodeKvp.Key, connectionIds, ref outputCounter));
			}

			foreach (var node in nodes)
			{
				SerializedNode.UnpackOutputs (connectionIds, node);
			}

			var graph = new Graph (nodes.ToArray (), connectionIds.Count);
			return graph;
		}
	}
}
