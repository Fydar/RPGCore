using System.Collections.Generic;

namespace RPGCore.Behaviour;

public sealed class GraphRuntimeData
{
	public Dictionary<string, INodeData> Nodes { get; set; } = new();

	public Dictionary<string, IOutputData> Outputs { get; set; } = new();
}
