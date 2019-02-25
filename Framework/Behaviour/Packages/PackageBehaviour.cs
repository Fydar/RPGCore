using System.Collections.Generic;
using System.Reflection;

namespace RPGCore.Behaviour.Packages
{
	public struct PackageBehaviour
	{
		public string Name;
		public string Description;
		public string Type;
		public Dictionary<string, string> CustomData;
		public Dictionary<string, PackageNode> Nodes;

		public Graph Unpack ()
		{
			var nodes = new Dictionary<string, Node> (Nodes.Count);
			int index = 0;

			var outputIds = new List<string> ();
			foreach (var nodeKvp in Nodes)
			{
				var nodeType = System.Type.GetType (nodeKvp.Value.Type);

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
				nodes[nodeKvp.Key] = nodeKvp.Value.Unpack (outputIds, ref outputCounter);
				index++;
			}

			var graph = new Graph (nodes, outputIds.Count);
			return graph;
		}
	}
}
