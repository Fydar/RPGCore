using System;
using System.Collections.Generic;

namespace RPGCore.Behaviour;

/// <summary>
/// A context used for running a <see cref="Graph"/>.
/// </summary>
public sealed class BehaviourEngine
{
	private readonly List<BehaviourEngineModule> modules = new();

	/// <summary>
	/// A collection of all <see cref="BehaviourEngineModule"/> that this <see cref="BehaviourEngine"/> utilises.
	/// </summary>
	public IReadOnlyList<BehaviourEngineModule> LoadedModules => modules;

	/// <summary>
	/// Adds an additional <see cref="BehaviourEngineModule"/> to this <see cref="BehaviourEngine"/>.
	/// </summary>
	/// <param name="behaviourEngineModule">The <see cref="BehaviourEngineModule"/> to load.</param>
	public void LoadModule( 
		BehaviourEngineModule behaviourEngineModule)
	{
		modules.Add(behaviourEngineModule);
	}

	/// <summary>
	/// Removes a <see cref="BehaviourEngineModule"/> from this <see cref="BehaviourEngine"/>.
	/// </summary>
	/// <param name="behaviourEngineModule">The <see cref="BehaviourEngineModule"/> to unload.</param>
	public void UnloadModule(
		BehaviourEngineModule behaviourEngineModule)
	{
		if (!modules.Remove(behaviourEngineModule))
		{
			throw new InvalidOperationException($"Unable to unload module that is not in use.");
		}
	}

	/// <summary>
	/// Creates a new <see cref="GraphInstance"/>.
	/// </summary>
	/// <param name="graphEngine">A <see cref="GraphDefinition"/> which defines the behaviour of the <see cref="GraphInstance"/>.</param>
	/// <param name="graphRuntimeData">The <see cref="GraphInstanceData"/> used to persist state from the <see cref="GraphInstance"/>.</param>
	/// <returns>A <see cref="GraphInstance"/> crated from the <see cref="GraphDefinition"/>.</returns>
	public GraphInstance CreateGraphRuntime(
		GraphEngine graphEngine,
		GraphInstanceData graphRuntimeData)
	{
		var newNodes = new GraphInstanceDataNode[graphEngine.Nodes.Count];

		for (int i = 0; i < graphEngine.Nodes.Count; i++)
		{
			var graphEngineNode = graphEngine.Nodes[i];

			ref var node = ref newNodes[i];
			if (graphRuntimeData.ContainsNode(graphEngineNode.Node.Id))
			{
				node = graphRuntimeData.GetNodeData(graphEngineNode.Node.Id);
			}
			else
			{
				node = new GraphInstanceDataNode
				{
					Id = graphEngineNode.Node.Id,
					Outputs = new Dictionary<string, IOutputData>()
				};
			}

			var newNodeComponentPools = new Array[graphEngineNode.Components.Count];

			for (int j = 0; j < graphEngineNode.Components.Count; j++)
			{
				var graphEngineNodeComponent = graphEngineNode.Components[j];

				Array? newComponentPool = null;
				foreach (var oldComponentPool in node.componentPools)
				{
					if (oldComponentPool.GetType() == graphEngineNodeComponent.ComponentType.MakeArrayType())
					{
						newComponentPool = oldComponentPool;
						break;
					}
				}

				if (newComponentPool == null)
				{
					newComponentPool = Array.CreateInstance(graphEngineNodeComponent.ComponentType, 1);
				}

				newNodeComponentPools[j] = newComponentPool;
			}
			node.componentPools = newNodeComponentPools;
		}

		graphRuntimeData.Nodes = newNodes;

		return new GraphInstance(this, graphEngine, graphRuntimeData);
	}
}
