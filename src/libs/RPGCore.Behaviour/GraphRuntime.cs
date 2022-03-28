namespace RPGCore.Behaviour;

public struct GraphRuntime
{
	public GraphEngine GraphEngine { get; }
	public GraphDefinition GraphDefinition { get; }
	public GraphRuntimeData GraphRuntimeData { get; }

	internal GraphRuntime(
		GraphEngine graphEngine,
		GraphDefinition graphDefinition,
		GraphRuntimeData graphRuntimeData)
	{
		GraphEngine = graphEngine;
		GraphDefinition = graphDefinition;
		GraphRuntimeData = graphRuntimeData;
	}

	public GraphRuntimeMutationScope Mutate()
	{
		return new GraphRuntimeMutationScope(this);
	}
}
