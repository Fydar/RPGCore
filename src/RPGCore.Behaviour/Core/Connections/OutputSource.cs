using System;

namespace RPGCore.Behaviour
{
	public struct OutputSource
	{
		public INodeInstance Instance;
		public InputMap InputInformation;
		public Node Node => Instance.Node;

		public OutputSource (INodeInstance instance, InputMap outputMapping)
		{
			Instance = instance ?? throw new ArgumentNullException (nameof (instance));
			InputInformation = outputMapping;
		}

		public override string ToString ()
		{
			if (Instance == null)
			{
				return $"No Input";
			}
			else
			{
				return $"Input {InputInformation.ConnectionId} from node {Instance}";
			}
		}
	}
}
