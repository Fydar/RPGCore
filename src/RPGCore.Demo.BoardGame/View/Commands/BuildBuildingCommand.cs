namespace RPGCore.Demo.BoardGame
{
	public class BuildBuildingCommand : GameCommand
	{
		public string BuildingIdentifier { get; set; }
		public Integer2 Offset { get; set; }
		public Integer2 BuildingPosition { get; set; }
		public BuildingOrientation Orientation { get; set; }

		
	}
}
