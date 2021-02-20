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

		/// <inheritdoc/>
		public override string ToString()
		{
			return $"Input {ConnectionId.ToString()}";
		}
	}
}
