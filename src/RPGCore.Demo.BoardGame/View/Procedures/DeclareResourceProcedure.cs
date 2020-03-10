using System.Collections.Generic;

namespace RPGCore.Demo.BoardGame
{
	public class DeclareResourceProcedure : GameViewProcedure 
	{
		public string ResourceIdentifier { get; set; }

		public override ProcedureResult Apply(GameView view)
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

			return ProcedureResult.Success;
		}
	}
}
