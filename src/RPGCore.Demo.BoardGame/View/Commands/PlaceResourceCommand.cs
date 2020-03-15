namespace RPGCore.Demo.BoardGame
{
	public class PlaceResourceCommand : GameCommand
	{
		public string ResourceIdentifier { get; set; }
		public Integer2 ResourcePosition { get; set; }
	}
}
