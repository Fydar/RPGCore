namespace RPGCore.Behaviour
{
	public readonly struct OutputSocket
	{
		public readonly int Id;

		public OutputSocket(int id)
		{
			Id = id;
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			return $"Output {Id}";
		}
	}
}
