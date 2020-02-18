namespace RPGCore.Demo.BoardGame
{
	public class DeclareResourceAction : GameViewAction
	{
		public string ResourceIdentifier { get; set; }

		public override ActionApplyResult Apply(GameView view)
		{
			view.DeclaredResource = true;

			foreach (var player in view.Players)
			{
				player.ResourceHand.Add(ResourceIdentifier);
			}

			return ActionApplyResult.Success;
		}
	}
}
