namespace RPGCore.Behaviour
{
	public sealed class ManifestCaptureGraphInstance : IGraphInstance
	{
		private Node node;
		private INodeInstance instance;

		public INodeInstance this[LocalId id] => null;

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

		public InputMap Connect<T> (ref InputSocket socket, ref IInput<T> connection)
		{
			connection = null;
			return new InputMap (socket, typeof (T), connection);
		}

		public OutputMap Connect<T> (ref OutputSocket socket, ref IOutput<T> connection)
		{
			connection = null;
			return new OutputMap (socket, typeof (T), null);
		}

		public OutputMap Connect<T> (ref OutputSocket socket, ref ILazyOutput<T> connection)
		{
			connection = null;
			return new OutputMap (socket, typeof (T), null);
		}

		public InputMap Connect<T> (ref InputSocket socket, ref T connection)
			where T : INodeInstance
		{
			connection = default (T);
			return new InputMap (socket, typeof (T), connection);
		}
	}
}
