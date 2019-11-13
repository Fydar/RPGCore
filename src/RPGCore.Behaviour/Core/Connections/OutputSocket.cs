namespace RPGCore.Behaviour
{
	public struct OutputSocket
	{
		public int Id;

		public OutputSocket(int id)
		{
			Id = id;
		}

		public override string ToString() => $"Output {Id.ToString ()}";
	}
}
