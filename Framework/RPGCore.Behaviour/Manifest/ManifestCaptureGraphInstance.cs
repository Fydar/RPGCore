namespace RPGCore.Behaviour
{
	public sealed class ManifestCaptureGraphInstance : IGraphInstance
	{
		private Node node;
		private INodeInstance instance;

		public ManifestCaptureGraphInstance (Node singleNodeGraph)
		{
			node = singleNodeGraph;
			instance = node.Create ();

			node.Inputs (this, instance);
			node.Outputs (this, instance);
		}

		public void Setup (Actor target)
		{
			node.Setup (this, instance, target);
		}

		public void Remove ()
		{
			instance.Remove ();
		}

		public INodeInstance GetNode<T> ()
		{
			return instance;
		}

		public InputMap Connect<T> (ref InputSocket socket, out IInput<T> connection)
		{
			connection = null;
			return new InputMap (socket, typeof (T), connection);
		}

		public OutputMap Connect<T> (ref OutputSocket socket, out IOutput<T> connection)
		{
			connection = null;
			return new OutputMap (socket, typeof (T), connection);
		}

		public OutputMap Connect<T> (ref OutputSocket socket, out ILazyOutput<T> connection)
		{
			connection = null;
			return new OutputMap (socket, typeof (T), connection);
		}

		public InputMap Connect<T> (ref InputSocket socket, out T connection)
			where T : INodeInstance
		{
			connection = default (T);
			return new InputMap (socket, typeof (T), connection);
		}

		public void Connect (ref InputSocket input, out INodeInstance connection)
		{
			connection = null;
		}
	}
}
