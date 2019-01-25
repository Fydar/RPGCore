using System;

namespace Behaviour
{
	public struct SocketMap
	{
		public ISocket Source;
		public object Link;

		public SocketMap(ISocket source, object link)
		{
			Source = source;
			Link = link;
		}
	}

	public struct InputMap
	{
		public InputSocket Source;
		public Type ConnectionType;
		public object Link;

		public InputMap(InputSocket source, Type connectionType, object link)
		{
			Source = source;
			Link = link;
			ConnectionType = connectionType;
		}
	}

	public struct OutputMap
	{
		public OutputSocket Source;
		public Type ConnectionType;
		public object Link;

		public OutputMap (OutputSocket source, Type connectionType, object link)
		{
			Source = source;
			Link = link;
			ConnectionType = connectionType;
		}
	}
}
