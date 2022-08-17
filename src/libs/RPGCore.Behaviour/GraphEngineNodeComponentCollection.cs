using System.Diagnostics;

namespace RPGCore.Behaviour;

[DebuggerDisplay("Count = {Count,nq}")]
public readonly ref struct GraphEngineNodeComponentCollection
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly GraphEngine graphEngine;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly int engineNodeIndex;

	/// <summary>
	/// The count of elements contained within this collection.
	/// </summary>
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public int Count => graphEngine.nodes[engineNodeIndex].componentsCount;

	public GraphEngineNodeComponent this[int index]
	{
		get
		{
			return new GraphEngineNodeComponent(graphEngine, graphEngine.nodes[engineNodeIndex].componentsStartIndex + index);
		}
	}

	public GraphEngineNodeComponentCollection(
		GraphEngine graphEngine,
		int engineNodeIndex)
	{
		this.graphEngine = graphEngine;
		this.engineNodeIndex = engineNodeIndex;
	}
}
