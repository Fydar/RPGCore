using System.Collections.Generic;

namespace RPGCore.Behaviour;

/// <summary>
/// A builder used to construct instances of <see cref="BehaviourEngineModule"/>.
/// </summary>
public sealed class BehaviourEngineModuleBuilder
{
	private readonly Dictionary<string, Graph> graphs = new();

	/// <summary>
	/// Adds an additional
	/// </summary>
	/// <param name="identifier">A <see cref="string"/> identifier used to locate the <see cref="Graph"/> in the <see cref="BehaviourEngineModule"/>.</param>
	/// <param name="graph">The <see cref="Graph"/> to add to the constructed <see cref="BehaviourEngineModule"/>.</param>
	/// <returns>The current instance of this builder.</returns>
	public BehaviourEngineModuleBuilder UseGraph(
		string identifier,
		Graph graph)
	{
		graphs.Add(identifier, graph);
		return this;
	}

	/// <summary>
	/// Creates a new instance of the <see cref="BehaviourEngineModule"/> class created from the current state of this <see cref="BehaviourEngineModuleBuilder"/>.
	/// </summary>
	/// <returns>A new instance of the <see cref="BehaviourEngineModule"/> class created from the current state of this <see cref="BehaviourEngineModuleBuilder"/>.</returns>
	public BehaviourEngineModule Build()
	{
		return new BehaviourEngineModule(graphs);
	}
}
