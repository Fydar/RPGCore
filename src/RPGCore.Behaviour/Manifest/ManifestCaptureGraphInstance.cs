namespace RPGCore.Behaviour
{
	public sealed class ManifestCaptureGraphInstance : IGraphConnections
	{
		private readonly Node node;
		private readonly INodeInstance instance;

		public ManifestCaptureGraphInstance (Node singleNodeGraph)
		{
			node = singleNodeGraph;
			instance = node.CreateInstance ();

			node.Inputs (this, instance);
			node.Outputs (this, instance);
		}

		public InputMap Connect<T> (ref InputSocket socket, ref Input<T> connection)
		{
			connection = new Input<T> ();
			return new InputMap (socket, typeof (T));
		}

		public OutputMap Connect<T> (ref OutputSocket socket, ref Output<T> connection)
		{
			connection = new Output<T> (null);
			return new OutputMap (socket, typeof (T));
		}

		public InputMap Connect<T> (ref InputSocket socket, ref T connection)
			where T : INodeInstance
		{
			connection = default (T);
			return new InputMap (socket, typeof (T));
		}
	}
}
