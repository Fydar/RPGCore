using System;

namespace RPGCore.Behaviour
{
	public readonly ref struct ConnectionMapper
	{
		internal readonly INodeInstance NodeInstance;
		internal readonly IConnectionCallback Callback;

		public ConnectionMapper(INodeInstance nodeInstance, IConnectionCallback callback)
		{
			NodeInstance = nodeInstance ?? throw new ArgumentNullException(nameof(nodeInstance));
			Callback = callback ?? throw new ArgumentNullException(nameof(callback));
		}


		public InputMap Connect<T>(ref InputSocket socket, ref Input<T> connection)
		{
			return Callback.Connect(NodeInstance, ref socket, ref connection);
		}

		public OutputMap Connect<T>(ref OutputSocket socket, ref Output<T> connection)
		{
			return Callback.Connect(NodeInstance, ref socket, ref connection);
		}

		public InputMap Connect<T>(ref InputSocket socket, ref T connection)
			where T : INodeInstance
		{
			return Callback.Connect(NodeInstance, ref socket, ref connection);
		}
	}
}
