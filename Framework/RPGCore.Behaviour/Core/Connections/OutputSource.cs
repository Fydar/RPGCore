using System;

namespace RPGCore.Behaviour
{
	public struct OutputSource
	{
		public Node Node;
		public INodeInstance Instance;
		public InputMap InputInformation;

		public OutputSource (Node node, INodeInstance instance, InputMap outputMapping)
		{
			Node = node;
			Instance = instance ?? throw new ArgumentNullException (nameof (instance));
			InputInformation = outputMapping;
		}

		public override string ToString () => $"Output {InputInformation.ConnectionId} from node {Instance}";
	}
}
