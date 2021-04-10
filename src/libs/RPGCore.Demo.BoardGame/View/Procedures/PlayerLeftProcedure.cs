using RPGCore.Behaviour;

namespace RPGCore.Demo.BoardGame
{
	public class PlayerLeftProcedure : LobbyViewProcedure
	{
		public LocalId OwnerId { get; set; }

		public override ProcedureResult Apply(LobbyView view)
		{
			view.Players.RemovePlayerWithId(OwnerId);

			return ProcedureResult.Success;
		}
	}
}
