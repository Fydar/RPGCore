using System;

namespace RPGCore.Behaviour
{
	public struct InputSource
	{
		public Node Node;
		public INodeInstance Instance;
		public OutputMap OutputInformation;

		public InputSource (Node node, INodeInstance instance, OutputMap outputMapping)
		{
			Node = node;
			Instance = instance ?? throw new ArgumentNullException (nameof (instance));
			OutputInformation = outputMapping;
		}

		public override string ToString () => $"Output {OutputInformation.ConnectionId} from node {Instance}";
	}
}
