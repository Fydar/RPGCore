namespace RPGCore.Behaviour;

public struct NodeDefinitionInput
{
	public IInput Input { get; }

	internal NodeDefinitionInput(
		IInput input)
	{
		Input = input;
	}
}
