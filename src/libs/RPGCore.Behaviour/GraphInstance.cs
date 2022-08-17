namespace RPGCore.Behaviour;

public struct GraphInstance
{
	public BehaviourEngine BehaviourEngine { get; }
	public GraphEngine GraphEngine { get; }
	public GraphInstanceData GraphRuntimeData { get; }

	internal GraphInstance(
		BehaviourEngine behaviourEngine,
		GraphEngine graphEngine,
		GraphInstanceData graphRuntimeData)
	{
		BehaviourEngine = behaviourEngine;
		GraphEngine = graphEngine;
		GraphRuntimeData = graphRuntimeData;
	}

	public GraphInstanceMutationScope Mutate()
	{
		return new GraphInstanceMutationScope(this);
	}
}
