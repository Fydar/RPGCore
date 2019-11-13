using System;

namespace RPGCore.Behaviour
{
	public struct InputSource
	{
		public Node Node;
		public INodeInstance Instance;
		public OutputMap OutputInformation;

		public InputSource(Node node, INodeInstance instance, OutputMap outputMapping)
		{
			Node = node;
			Instance = instance ?? throw new ArgumentNullException (nameof (instance));
			OutputInformation = outputMapping;
		}

		public override string ToString()
		{
			if (Instance == null)
			{
				return $"No Output";
			}
			else
			{
				return $"Output {OutputInformation.ConnectionId} from node {Instance}";
			}
		}
	}
}
