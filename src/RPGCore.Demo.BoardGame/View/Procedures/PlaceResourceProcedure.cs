using RPGCore.Behaviour;
using System.Linq;

namespace RPGCore.Demo.BoardGame
{
	public class PlaceResourceProcedure : LobbyViewProcedure
	{
		public LocalId Player { get; set; }
		public string ResourceIdentifier { get; set; }
		public Integer2 ResourcePosition { get; set; }

		public override ProcedureResult Apply(LobbyView view)
		{
			var gameplayPlayer = view.Gameplay.Players[Player];

			bool owned = gameplayPlayer.ResourceHand.Remove(ResourceIdentifier);
			var placeTile = gameplayPlayer.Board[ResourcePosition];

			if (placeTile.Resource != null
				|| placeTile.Building != null
				|| !owned)
			{
				return ProcedureResult.NotModified;
			}

			placeTile.Resource = ResourceIdentifier;

			return ProcedureResult.Success;
		}
	}
}
