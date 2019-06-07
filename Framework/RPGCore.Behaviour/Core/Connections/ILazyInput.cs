namespace RPGCore.Behaviour
{
	public interface ILazyInput<T> : IInput
	{
		T Value { get; }
	}
}
