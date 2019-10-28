namespace RPGCore.Behaviour
{
	public interface IEventField<T>
	{
		HandlerCollection<T> Handlers { get; set; }
		T Value { get; }
	}
}
