namespace RPGCore.Demo.BoardGame
{
	public class BuildBuildingAction : GameViewAction
	{
		public string BuildingIdentifier { get; set; }
		public Integer2 Offset { get; set; }
		public BuildingOrientation Orientation { get; set; }

		public override ActionApplyResult Apply(GameView view)
		{


			return ActionApplyResult.Success;
		}
	}
}
