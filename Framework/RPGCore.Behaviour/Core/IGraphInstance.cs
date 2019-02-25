namespace RPGCore.Behaviour
{
	public interface IGraphInstance : IBehaviour
	{
		void Setup (Actor target);

		InputMap Connect<T> (ref InputSocket socket, out IInput<T> connection);
		OutputMap Connect<T> (ref OutputSocket socket, out IOutput<T> connection);
		OutputMap Connect<T> (ref OutputSocket socket, out ILazyOutput<T> connection);
		InputMap Connect<T> (ref InputSocket socket, out T connection)
			where T : INodeInstance;
		void Connect (ref InputSocket input, out INodeInstance connection);
	}
}
