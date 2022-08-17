using System.Diagnostics;

namespace RPGCore.Behaviour;

[DebuggerDisplay("Count = {Count,nq}")]
public readonly ref struct GraphEngineNodeCollection
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly GraphEngine graphEngine;

	/// <summary>
	/// The count of elements contained within this collection.
	/// </summary>
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public int Count => graphEngine.nodes.Length;

	public GraphEngineNode this[int index] => new(graphEngine, index);

	public GraphEngineNodeCollection(
		GraphEngine graphEngine)
	{
		this.graphEngine = graphEngine;
	}
}
