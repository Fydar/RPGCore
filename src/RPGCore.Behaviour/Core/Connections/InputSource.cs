using System;

namespace RPGCore.Behaviour
{
	public readonly struct InputSource
	{
		public readonly Node Node;
		public readonly INodeInstance Instance;
		public readonly OutputMap OutputInformation;

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
