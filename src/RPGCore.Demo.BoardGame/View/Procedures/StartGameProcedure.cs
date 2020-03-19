using System.Collections.Generic;
using System.Linq;

namespace RPGCore.Demo.BoardGame
{
	public class StartGameProcedure : GameViewProcedure
	{
		public string[] Buildings { get; set; }
		public List<GamePlayerState> PlayerStates { get; set; }

		public override ProcedureResult Apply(GameView view)
		{
			view.Buildings = Buildings.Select(identifier =>
			{
				var buildingTemplate = view.BuildingTemplates[identifier];

				return new GlobalCardSlot()
				{
					BuildingIdentifier = identifier,
					GlobalEffect = buildingTemplate.GlobalEffectGraph?.Unpack()?.Create()
				};
			}).ToArray();

			view.PlayerStates = PlayerStates;

			foreach (var player in view.PlayerStates)
			{
				player.Buildings = Buildings.Select(identifier =>
				{
					var buildingTemplate = view.BuildingTemplates[identifier];

					return new BoardCardSlot()
					{
						BuildingIdentifier = identifier,
						BoardEffect = buildingTemplate.BoardEffectGraph?.Unpack()?.Create()
					};
				}).ToList();
			}

			return ProcedureResult.Success;
		}
	}
}
