using System.Collections.Generic;

namespace RPGCore.Behaviour;

/// <summary>
/// A builder used to construct instances of <see cref="GraphEngineModule"/>.
/// </summary>
public sealed class GraphEngineModuleBuilder
{
	private readonly Dictionary<string, Graph> graphs = new();

	/// <summary>
	/// Adds an additional
	/// </summary>
	/// <param name="identifier">A <see cref="string"/> identifier used to locate the <see cref="Graph"/> in the <see cref="GraphEngineModule"/>.</param>
	/// <param name="graph">The <see cref="Graph"/> to add to the constructed <see cref="GraphEngineModule"/>.</param>
	/// <returns>The current instance of this builder.</returns>
	public GraphEngineModuleBuilder UseGraph(
		string identifier,
		Graph graph)
	{
		graphs.Add(identifier, graph);
		return this;
	}

	/// <summary>
	/// Creates a new instance of the <see cref="GraphEngineModule"/> class created from the current state of this <see cref="GraphEngineModuleBuilder"/>.
	/// </summary>
	/// <returns>A new instance of the <see cref="GraphEngineModule"/> class created from the current state of this <see cref="GraphEngineModuleBuilder"/>.</returns>
	public GraphEngineModule Build()
	{
		return new GraphEngineModule(graphs);
	}
}
