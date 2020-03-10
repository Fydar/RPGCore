using RPGCore.Demo.BoardGame.Models;
using RPGCore.Traits;

namespace RPGCore.Demo.BoardGame
{
	public class Building
	{
		public BuildingTemplate Template { get; }
		public StatInstance LocalReward { get; }
		public GameTile Tile { get; }

		public Building(BuildingTemplate template, GameTile tile)
		{
			Template = template;
			Tile = tile;
		}

		public override string ToString()
		{
			return Template?.DisplayName ?? "Unknown";
		}
	}
}
