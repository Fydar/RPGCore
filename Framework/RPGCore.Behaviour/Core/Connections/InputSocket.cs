namespace RPGCore.Behaviour
{
	public struct InputSocket : ISocket
	{
		private int internalTargetId;

		public int TargetId
		{
			get
			{
				return internalTargetId - 1;
			}
			set
			{
				internalTargetId = value + 1;
			}
		}

		public InputSocket (int targetId)
		{
			internalTargetId = targetId + 1;
		}
	}
}
