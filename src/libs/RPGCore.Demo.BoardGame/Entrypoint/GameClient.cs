namespace RPGCore.Demo.BoardGame;

public class GameClient
{
	public LobbyView ClientView;

	public void AcceptInput(LobbyViewProcedure procedure)
	{
		ClientView.Apply(procedure);
	}
}
