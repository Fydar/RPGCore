using System.Collections.Generic;

namespace RPGCore.Demo.BoardGame
{
	public class DeclareResourceAction : GameViewAction
	{
		public string ResourceIdentifier { get; set; }

		public override ActionApplyResult Apply(GameView view)
		{
			view.DeclaredResource = true;

			foreach (var player in view.Players)
			{
				if (player.ResourceHand == null)
				{
					player.ResourceHand = new List<string>();
				}
				player.ResourceHand.Add(ResourceIdentifier);
			}

			return ActionApplyResult.Success;
		}
	}
}
