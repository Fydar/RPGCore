using System;

namespace RPGCore.Behaviour;

public readonly ref struct ConnectionMapper
{
	internal readonly INodeInstance nodeInstance;
	internal readonly IConnectionCallback callback;

	public ConnectionMapper(INodeInstance nodeInstance, IConnectionCallback callback)
	{
		this.nodeInstance = nodeInstance ?? throw new ArgumentNullException(nameof(nodeInstance));
		this.callback = callback ?? throw new ArgumentNullException(nameof(callback));
	}

	public InputMap Connect<T>(ref InputSocket socket, ref Input<T> connection)
	{
		return callback.Connect(nodeInstance, ref socket, ref connection);
	}

	public OutputMap Connect<T>(ref OutputSocket socket, ref Output<T> connection)
	{
		return callback.Connect(nodeInstance, ref socket, ref connection);
	}

	public InputMap Connect<T>(ref InputSocket socket, ref T connection)
		where T : INodeInstance
	{
		return callback.Connect(nodeInstance, ref socket, ref connection);
	}
}
