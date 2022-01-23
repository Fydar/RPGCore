using RPGCore.Behaviour;

namespace RPGCore.Demo.BoardGame;

public class PlayerJoinedProcedure : LobbyViewProcedure
{
	public string DisplayName { get; set; }
	public LocalId OwnerId { get; set; }

	public override ProcedureResult Apply(LobbyView view)
	{
		view.Players.AddPlayer(new LobbyPlayer()
		{
			OwnerId = OwnerId,
			DisplayName = DisplayName
		});

		return ProcedureResult.Success;
	}
}
