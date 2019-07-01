namespace RPGCore.Behaviour
{
	public struct AiPath
	{
		public readonly int Weight;
		public readonly IAiNode Source;

		public AiPath (int weight, IAiNode source)
		{
			Weight = weight;
			Source = source;
		}

		public override string ToString ()
		{
			return Source != null
				? Weight + " on " + Source + " (" + Source.GetHashCode () + ")"
				: "Terminating " + Weight;
		}
	}
}
