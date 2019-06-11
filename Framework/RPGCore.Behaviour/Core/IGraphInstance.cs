namespace RPGCore.Behaviour
{
	public interface IGraphInstance : IBehaviour
	{
		INodeInstance this[LocalId id] { get; }
		
		void Setup (Actor target);

		InputMap Connect<T> (ref InputSocket socket, ref IInput<T> connection);
		OutputMap Connect<T> (ref OutputSocket socket, ref IOutput<T> connection);
		OutputMap Connect<T> (ref OutputSocket socket, ref ILazyOutput<T> connection);
		InputMap Connect<T> (ref InputSocket socket, ref T connection)
			where T : INodeInstance;
	}
}
