using System;

namespace RPGCore.Behaviour
{
	public struct InputMap
	{
		public int ConnectionId;
		public Type ConnectionType;

		public InputMap(InputSocket source, Type connectionType)
		{
			ConnectionType = connectionType;
			ConnectionId = source.ConnectionId;
		}

		public override string ToString()
		{
			if (ConnectionId == -1)
			{
				return $"Unmapped Input of type {ConnectionType}";
			}
			else
			{
				return $"Input {ConnectionId} of type {ConnectionType}";
			}
		}
	}
}
