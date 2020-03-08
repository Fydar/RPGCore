using RPGCore.Behaviour.Manifest;
using System.Diagnostics;

namespace RPGCore.Behaviour
{
	[EditorType]
	public readonly struct InputSocket
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly int internalConnectionId;

		public int ConnectionId
		{
			get
			{
				return internalConnectionId - 1;
			}
		}

		public InputSocket(int connectionId)
		{
			internalConnectionId = connectionId + 1;
		}

		public override string ToString() => $"Input {ConnectionId.ToString()}";
	}
}
