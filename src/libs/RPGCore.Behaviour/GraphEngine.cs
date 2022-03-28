using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

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
	/// <param name="graphRuntimeData">The <see cref="GraphRuntimeData"/> used to persist state from the <see cref="GraphRuntime"/>.</param>
	/// <returns>A <see cref="GraphRuntime"/> crated from the <see cref="GraphDefinition"/>.</returns>
	public GraphRuntime CreateGraphRuntime(
		GraphDefinition graphDefinition,
		GraphRuntimeData graphRuntimeData)
	{
		var newNodes = new NodeRuntimeData[graphDefinition.NodeDefinitions.Count];

		for (int i = 0; i < graphDefinition.NodeDefinitions.Count; i++)
		{
			var nodeDefinition = graphDefinition.NodeDefinitions[i];

			ref var node = ref newNodes[i];
			if (graphRuntimeData.ContainsNode(nodeDefinition.Node.Id))
			{
				node = graphRuntimeData.GetNode(nodeDefinition.Node.Id);
			}
			else
			{
				node = new NodeRuntimeData
				{
					Id = nodeDefinition.Node.Id,
					Outputs = new Dictionary<string, IOutputData>()
				};
			}

			var newNodeComponentPools = new Array[nodeDefinition.Components.Count];

			for (int j = 0; j < nodeDefinition.Components.Count; j++)
			{
				var componentDefinition = nodeDefinition.Components[j];

				Array? newComponentPool = null;
				foreach (var oldComponentPool in node.componentPools)
				{
					if (oldComponentPool.GetType() == componentDefinition.MakeArrayType())
					{
						newComponentPool = oldComponentPool;
						break;
					}
				}

				if (newComponentPool == null)
				{
					newComponentPool = Array.CreateInstance(componentDefinition, 1);
				}

				newNodeComponentPools[j] = newComponentPool;
			}
			node.componentPools = newNodeComponentPools;
		}

		graphRuntimeData.Nodes = newNodes;

		return new GraphRuntime(this, graphDefinition, graphRuntimeData);
	}
}
