using RPGCore.Behaviour;

namespace RPGCore.Demo.BoardGame
{
	public abstract class GameViewAction
	{
		public LocalId Client { get; set; }

		public abstract ActionApplyResult Apply(GameView view);
	}
}
