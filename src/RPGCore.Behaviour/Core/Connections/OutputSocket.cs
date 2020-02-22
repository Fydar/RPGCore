namespace RPGCore.Behaviour
{
	public readonly struct OutputSocket
	{
		public readonly int Id;

		public OutputSocket(int id)
		{
			Id = id;
		}

		public override string ToString() => $"Output {Id.ToString()}";
	}
}
