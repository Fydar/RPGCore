namespace RPGCore.Behaviour
{
	public interface IGraphConnections
	{
		InputMap Connect<T> (ref InputSocket socket, ref Input<T> connection);
		OutputMap Connect<T> (ref OutputSocket socket, ref Output<T> connection);
		InputMap Connect<T> (ref InputSocket socket, ref T connection)
			where T : INodeInstance;
	}
}
