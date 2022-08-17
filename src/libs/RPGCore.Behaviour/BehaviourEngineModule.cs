using System.Collections.Generic;

namespace RPGCore.Behaviour;

/// <summary>
/// A module that can be loaded into a <see cref="BehaviourEngine"/> to extend functionality.
/// </summary>
public sealed class BehaviourEngineModule
{
	private readonly Dictionary<string, Graph> graphs;

	/// <summary>
	/// A collection of all <see cref="Graph"/> provided by this <see cref="BehaviourEngineModule"/>.
	/// </summary>
	public IReadOnlyDictionary<string, Graph> Graphs => graphs;

	internal BehaviourEngineModule(
		Dictionary<string, Graph> graphs)
	{
		this.graphs = graphs;
	}

	/// <summary>
	/// Begins the construction of a <see cref="BehaviourEngineModule"/> via a <see cref="BehaviourEngineModuleBuilder"/>.
	/// </summary>
	/// <returns>A builder that can be used to extend the <see cref="BehaviourEngineModule"/>.</returns>
	public static BehaviourEngineModuleBuilder Create()
	{
		return new BehaviourEngineModuleBuilder();
	}
}
