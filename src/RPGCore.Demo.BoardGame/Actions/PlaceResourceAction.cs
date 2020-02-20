using RPGCore.Demo.BoardGame.Models;
using System.Linq;

namespace RPGCore.Demo.BoardGame
{
	public class PlaceResourceAction : GameViewAction
	{
		public string ResourceIdentifier { get; set; }
		public Integer2 ResourcePosition { get; set; }

		public override ActionApplyResult Apply(GameView view)
		{
			var ownerPlayer = view.Players.Where(player => player.OwnerId == Client).FirstOrDefault();

			bool owned = ownerPlayer.ResourceHand.Remove(ResourceIdentifier);
			var placeTile = ownerPlayer.Board[ResourcePosition];

			if (placeTile.Resource != null
				|| placeTile.Building != null
				|| !owned)
			{
				return ActionApplyResult.Unauthorized;
			}

			placeTile.Resource = ResourceIdentifier;

			return ActionApplyResult.Success;
		}
	}
}
