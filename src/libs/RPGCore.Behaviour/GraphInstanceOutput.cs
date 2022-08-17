namespace RPGCore.Behaviour;

public struct GraphInstanceOutput<TType>
{
	internal Output<TType>.OutputData data;

	public TType Value
	{
		get => data.Value;
		set
		{
			data.Value = value;
			data.HasChanged = true;
		}
	}

	public GraphInstanceOutput(Output<TType>.OutputData data)
	{
		this.data = data;
	}
}
