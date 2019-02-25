namespace RPGCore.Behaviour
{
	public struct InputSocket : ISocket
	{
		public int TargetId;

		public InputSocket (int targetId)
		{
			TargetId = targetId;
		}
	}
}
