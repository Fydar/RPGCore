namespace Behaviour
{
	public struct InputSocket : ISocket
	{
		public int TargetId = -1;

		public InputSocket(int targetId)
		{
			TargetId = targetId;
		}
	}
}
