namespace RPGCore.Demo.BoardGame
{
	public class GameClient
	{
		public GameView ClientView;

		public void AcceptInput(GameViewProcedure procedure)
		{
			ClientView.Apply(procedure);
		}
	}
}
