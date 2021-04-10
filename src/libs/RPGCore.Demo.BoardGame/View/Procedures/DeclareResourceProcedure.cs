using RPGCore.Behaviour;
using System.Collections.Generic;

namespace RPGCore.Demo.BoardGame
{
	public class DeclareResourceProcedure : LobbyViewProcedure
	{
		public LocalId Player { get; set; }
		public string ResourceIdentifier { get; set; }

		public override ProcedureResult Apply(LobbyView view)
		{
			view.Gameplay.DeclaredResource = true;

			foreach (var gameplayPlayer in view.Gameplay.Players)
			{
				if (gameplayPlayer.ResourceHand == null)
				{
					gameplayPlayer.ResourceHand = new List<string>();
				}
				gameplayPlayer.ResourceHand.Add(ResourceIdentifier);
			}

			return ProcedureResult.Success;
		}
	}
}
