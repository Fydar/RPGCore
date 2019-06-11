using System;

namespace RPGCore.Behaviour
{
	public struct OutputMap
	{
		public OutputSocket Source;
		public Type ConnectionType;
		public Connection Connection;

		public OutputMap (OutputSocket source, Type connectionType, Connection connection)
		{
			Source = source;
			Connection = connection;
			ConnectionType = connectionType;
		}
	}
}
