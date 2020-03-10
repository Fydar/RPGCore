using System.Linq;

namespace RPGCore.Demo.BoardGame
{
	public class PlaceResourceProcedure : GameViewProcedure
	{
		public string ResourceIdentifier { get; set; }
		public Integer2 ResourcePosition { get; set; }

		public override ProcedureResult Apply(GameView view)
		{
			var ownerPlayer = view.Players.Where(player => player.OwnerId == Client).FirstOrDefault();

			bool owned = ownerPlayer.ResourceHand.Remove(ResourceIdentifier);
			var placeTile = ownerPlayer.Board[ResourcePosition];

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
