namespace RPGCore.Behaviour
{
	public interface IGraphInstance : IBehaviour
	{
		INodeInstance this[LocalId id] { get; }
		
		void Setup (Actor target);

		InputMap Connect<T> (ref InputSocket socket, out IInput<T> connection);
		OutputMap Connect<T> (ref OutputSocket socket, out IOutput<T> connection);
		OutputMap Connect<T> (ref OutputSocket socket, out ILazyOutput<T> connection);
		InputMap Connect<T> (ref InputSocket socket, out T connection)
			where T : INodeInstance;
	}
}
