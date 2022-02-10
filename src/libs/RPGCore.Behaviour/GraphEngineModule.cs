using System.Collections.Generic;

namespace RPGCore.Behaviour;

/// <summary>
/// A module that can be loaded into a <see cref="GraphEngine"/> to extend functionality.
/// </summary>
public sealed class GraphEngineModule
{
	private readonly Dictionary<string, Graph> graphs;

	/// <summary>
	/// A collection of all <see cref="Graph"/> provided by this <see cref="GraphEngineModule"/>.
	/// </summary>
	public IReadOnlyDictionary<string, Graph> Graphs => graphs;

	internal GraphEngineModule(
		Dictionary<string, Graph> graphs)
	{
		this.graphs = graphs;
	}

	/// <summary>
	/// Begins the construction of a <see cref="GraphEngineModule"/> via a <see cref="GraphEngineModuleBuilder"/>.
	/// </summary>
	/// <returns>A builder that can be used to extend the <see cref="GraphEngineModule"/>.</returns>
	public static GraphEngineModuleBuilder Create()
	{
		return new GraphEngineModuleBuilder();
	}
}
