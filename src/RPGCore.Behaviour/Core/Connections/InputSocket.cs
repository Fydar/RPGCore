using System.Diagnostics;

namespace RPGCore.Behaviour
{
	public struct InputSocket : ISocket
	{
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		private int internalConnectionId;

		public int ConnectionId
		{
			get
			{
				return internalConnectionId - 1;
			}
			set
			{
				internalConnectionId = value + 1;
			}
		}

		public InputSocket (int connectionId)
		{
			internalConnectionId = connectionId + 1;
		}

		public override string ToString () => $"Input {ConnectionId.ToString ()}";
	}
}
