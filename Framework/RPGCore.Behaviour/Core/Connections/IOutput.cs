namespace RPGCore.Behaviour
{
	public interface IOutput
	{
	}

	public interface IOutput<T> : IOutput
	{
		T Value { get; set; }
	}
}
