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
}
