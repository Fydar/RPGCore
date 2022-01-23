using Newtonsoft.Json;
using RPGCore.Traits;
using System.Diagnostics;

namespace RPGCore.Demo.BoardGame;

public class Building : IChildOf<GameTile>
{
	[JsonIgnore]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public GameTile Parent { get; set; }

	public string Identifier { get; }
	public StatInstance LocalReward { get; }

	public Building(string identifier)
	{
		Identifier = identifier;
	}

	/// <inheritdoc/>
	public override string ToString()
	{
		return Identifier;
	}
}
