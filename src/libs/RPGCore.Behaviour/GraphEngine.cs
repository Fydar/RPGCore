using System;
using System.Collections.Generic;

namespace RPGCore.Behaviour;

/// <summary>
/// A context used for running a <see cref="Graph"/>.
/// </summary>
public sealed class GraphEngine
{
	private readonly List<GraphEngineModule> modules = new();

	/// <summary>
	/// A collection of all <see cref="GraphEngineModule"/> that this <see cref="GraphEngine"/> utilises.
	/// </summary>
	public IReadOnlyList<GraphEngineModule> LoadedModules => modules;

	/// <summary>
	/// Adds an additional <see cref="GraphEngineModule"/> to this <see cref="GraphEngine"/>.
	/// </summary>
	/// <param name="graphModule">The <see cref="GraphEngineModule"/> to load.</param>
	public void LoadModule(
		GraphEngineModule graphModule)
	{
		modules.Add(graphModule);
	}

	/// <summary>
	/// Removes a <see cref="GraphEngineModule"/> from this <see cref="GraphEngine"/>.
	/// </summary>
	/// <param name="graphModule">The <see cref="GraphEngineModule"/> to unload.</param>
	public void UnloadModule(
		GraphEngineModule graphModule)
	{
		if (!modules.Remove(graphModule))
		{
			throw new InvalidOperationException($"Unable to unload module that is not in use.");
		}
	}

	/// <summary>
	/// Creates a new <see cref="GraphRuntime"/>.
	/// </summary>
	/// <param name="graphDefinition">A <see cref="GraphDefinition"/> which defines the behaviour of the <see cref="GraphRuntime"/>.</param>
	/// <param name="graphInstanceData">The <see cref="GraphRuntimeData"/> used to persist state from the <see cref="GraphRuntime"/>.</param>
	/// <returns>A <see cref="GraphRuntime"/> crated from the <see cref="GraphDefinition"/>.</returns>
	public GraphRuntime CreateGraphRuntime(
		GraphDefinition graphDefinition,
		GraphRuntimeData graphInstanceData)
	{
		return new GraphRuntime(this, graphDefinition, graphInstanceData);
	}
}
