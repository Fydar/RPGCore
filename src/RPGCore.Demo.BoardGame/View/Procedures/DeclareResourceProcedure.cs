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

			foreach (var playerState in view.PlayerStates)
			{
				if (playerState.ResourceHand == null)
				{
					playerState.ResourceHand = new List<string>();
				}
				playerState.ResourceHand.Add(ResourceIdentifier);
			}

			return ProcedureResult.Success;
		}
	}
}
