namespace RPGCore.Demo.BoardGame
{
	public struct OffsetAndRotation
	{
		public Integer2 Offset { get; }
		public BuildingOrientation Orientation { get; }

		public OffsetAndRotation(Integer2 offset, BuildingOrientation orientation)
		{
			Offset = offset;
			Orientation = orientation;
		}
	}
}
