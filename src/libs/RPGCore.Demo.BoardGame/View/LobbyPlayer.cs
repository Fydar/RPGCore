using Newtonsoft.Json;
using RPGCore.Behaviour;
using System.Diagnostics;

namespace RPGCore.Demo.BoardGame;

public class LobbyPlayer
{
	[JsonIgnore]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public LobbyView Lobby { get; internal set; }

	public LocalId OwnerId { get; set; }
	public string DisplayName { get; set; }
}
