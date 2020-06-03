using RPGCore.Behaviour;

namespace RPGCore.Demo.BoardGame
{
	public class LobbyPlayer
	{
		public LocalId OwnerId { get; set; }
		public string DisplayName { get; set; }
		public LobbyView Lobby { get; internal set; }
	}
}
