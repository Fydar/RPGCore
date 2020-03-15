using RPGCore.Demo.BoardGame.Models;
using System;
using System.Linq;

namespace RPGCore.Demo.BoardGame
{
	public class StartGameProcedure : GameViewProcedure
	{
		public override ProcedureResult Apply(GameView view)
		{
			var packTemplates = GameView.LoadAll<BuildingPackTemplate>(view.GameData.Tags["type-buildingpack"])
				.ToDictionary(template => template.Identifier);

			var resourceTemplates = GameView.LoadAll<ResourceTemplate>(view.GameData.Tags["type-resource"]);

			var buildingTemplates = GameView.LoadAll<BuildingTemplate>(view.GameData.Tags["type-building"])
				.ToDictionary(template => template.Identifier);


			var fullBuildingPacks = packTemplates
				.Select(pack => buildingTemplates
					.Where(building => building.Value.PackIdentifier == pack.Value.Identifier).ToArray()).ToArray();

			var rand = new Random();

			view.Buildings = fullBuildingPacks.Select(pack =>
			{
				if (pack == null || pack.Length == 0)
				{
					return null;
				}

				return pack[rand.Next(0, pack.Length)].Key;
			}).ToArray();

			return ProcedureResult.Success;
		}
	}
}
