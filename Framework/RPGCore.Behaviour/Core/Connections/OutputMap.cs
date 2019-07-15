using System;

namespace RPGCore.Behaviour
{
	public struct OutputMap
	{
		public int ConnectionId;
		public Type ConnectionType;

		public OutputMap (OutputSocket source, Type connectionType)
		{
			ConnectionType = connectionType;
			ConnectionId = source.Id;
		}

		public override string ToString () => $"Output to Connection {ConnectionId} of type {ConnectionType}";
	}
}
