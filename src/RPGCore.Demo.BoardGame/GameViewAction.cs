using RPGCore.Behaviour;

namespace RPGCore.Demo.BoardGame
{
	public abstract class GameViewProcedure 
	{
		public LocalId Client { get; set; }

		public abstract ProcedureResult Apply(GameView view);
	}
}
