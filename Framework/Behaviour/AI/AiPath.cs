namespace Behaviour
{
	public struct AiPath
	{
		public readonly int Weight;
		public readonly IAiNode Source;

		public AiPath(int weight, IAiNode source)
		{
			Weight = weight;
			Source = source;
		}

		public override string ToString()
		{
			if (Source == null)
				return "Terminating " + Weight;

			return Weight + " on " + Source + " (" + Source.GetHashCode() + ")";
		}
	}
}
