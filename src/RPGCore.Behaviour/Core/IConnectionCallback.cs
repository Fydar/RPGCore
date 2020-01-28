namespace RPGCore.Behaviour
{
	public interface IConnectionCallback
	{
		InputMap Connect<T>(INodeInstance parent, ref InputSocket socket, ref Input<T> connection);
		OutputMap Connect<T>(INodeInstance parent, ref OutputSocket socket, ref Output<T> connection);
		InputMap Connect<T>(INodeInstance parent, ref InputSocket socket, ref T connection)
			where T : INodeInstance;
	}
}
