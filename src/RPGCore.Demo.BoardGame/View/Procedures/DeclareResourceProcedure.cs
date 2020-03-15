using RPGCore.Behaviour;
using System.Collections.Generic;

namespace RPGCore.Demo.BoardGame
{
	public class DeclareResourceProcedure : GameViewProcedure
	{
		public LocalId Player { get; set; }
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
