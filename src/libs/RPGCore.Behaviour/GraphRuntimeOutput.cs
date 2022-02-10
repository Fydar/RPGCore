namespace RPGCore.Behaviour;

public struct GraphRuntimeOutput<TType>
{
	internal Output<TType>.OutputData data;

	public TType Value
	{
		get => data.Value;
		set => data.Value = value;
	}

	public GraphRuntimeOutput(Output<TType>.OutputData data)
	{
		this.data = data;
	}
}
