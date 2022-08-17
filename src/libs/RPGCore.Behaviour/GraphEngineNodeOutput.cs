using System.Diagnostics;

namespace RPGCore.Behaviour;

public readonly ref struct GraphEngineNodeOutput
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly GraphEngine graphEngine;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly int engineNodeOutputIndex;

	/// <summary>
	/// The name of this output.
	/// </summary>
	public string Name => graphEngine.outputs[engineNodeOutputIndex].name;

	/// <summary>
	/// A collection of all inputs that source their values from this output.
	/// </summary>
	public GraphEngineNodeOutputConnectedInputsCollection ConnectedInputs => new(graphEngine, engineNodeOutputIndex);

	public GraphEngineNodeOutput(
		GraphEngine graphEngine,
		int engineNodeOutputIndex)
	{
		this.graphEngine = graphEngine;
		this.engineNodeOutputIndex = engineNodeOutputIndex;
	}
}
