namespace RPGCore.Behaviour
{
	public sealed class StaticValue<T>
	{
		public T Value { get; }

		public StaticValue(T value)
		{
			Value = value;
		}
	}
}
