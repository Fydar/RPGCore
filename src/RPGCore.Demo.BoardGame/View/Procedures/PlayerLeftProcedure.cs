using RPGCore.Behaviour;

namespace RPGCore.Demo.BoardGame
{
	public class PlayerLeftProcedure : GameViewProcedure
	{
		public LocalId OwnerId { get; set; }

		public override ProcedureResult Apply(GameView view)
		{
			var leaver = view.Players.Find(player => player.OwnerId == OwnerId);

			view.Players.Remove(leaver);

			return ProcedureResult.Success;
		}
	}
}
