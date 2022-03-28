namespace RPGCore.Behaviour.Fluent;

public static class Input
{
	public static DefaultInput<TType> Default<TType>()
	{
		return new DefaultInput<TType>();
	}

	public static DefaultInput<TType> Default<TType>(TType defaultValue)
	{
		return new DefaultInput<TType>(defaultValue);
	}

	public static ConnectedInput<TType> Connected<TType>(Node node, string property)
	{
		return new ConnectedInput<TType>($"{node.Id}.{property}");
	}
}
