using RPGCore.Demo.BoardGame.Models;

namespace RPGCore.Demo.BoardGame
{
	public class BuildBuildingProcedure : GameViewProcedure
	{
		public string BuildingIdentifier { get; set; }
		public Integer2 Offset { get; set; }
		public Integer2 BuildingPosition { get; set; }
		public BuildingOrientation Orientation { get; set; }

		public override ProcedureResult Apply(GameView view)
		{
			var buildingTemplate = new BuildingTemplate()
			{
				Recipe = new string[,]
				{
					{ "x", "x", "x" },
					{ "x", null, null },
				}
			};

			var rotatedBuilding = new RotatedBuilding(buildingTemplate, Orientation);

			var ownerPlayer = view.GetPlayerForOwner(Client);

			for (int x = 0; x < rotatedBuilding.Width; x++)
			{
				for (int y = 0; y < rotatedBuilding.Height; y++)
				{
					var position = Offset + new Integer2(x, y);

					string recipeTile = rotatedBuilding[x, y];
					var tile = ownerPlayer.Board[position];

					if (recipeTile != null)
					{
						tile.Resource = "x";
					}
				}
			}

			var placeTile = ownerPlayer.Board[BuildingPosition];
			placeTile.Building = new Building(buildingTemplate, placeTile);

			return ProcedureResult.Success;
		}
	}
}
