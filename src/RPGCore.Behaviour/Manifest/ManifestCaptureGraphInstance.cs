namespace RPGCore.Behaviour
{
	public sealed class ManifestCaptureGraphInstance : IConnectionCallback
	{
		private readonly NodeTemplate node;
		private readonly INodeInstance instance;

		public ManifestCaptureGraphInstance(NodeTemplate singleNodeGraph)
		{
			node = singleNodeGraph;
			instance = node.CreateInstance();

			var connectionMapper = new ConnectionMapper(instance, this);

			node.Inputs(connectionMapper, instance);
			node.Outputs(connectionMapper, instance);
		}

		public InputMap Connect<T>(INodeInstance parent, ref InputSocket socket, ref Input<T> connection)
		{
			connection = new Input<T>();
			return new InputMap(socket, typeof(T));
		}

		public OutputMap Connect<T>(INodeInstance parent, ref OutputSocket socket, ref Output<T> connection)
		{
			connection = new Output<T>(null);
			return new OutputMap(socket, typeof(T));
		}

		public InputMap Connect<T>(INodeInstance parent, ref InputSocket socket, ref T connection)
			where T : INodeInstance
		{
			connection = default;
			return new InputMap(socket, typeof(T));
		}
	}
}
