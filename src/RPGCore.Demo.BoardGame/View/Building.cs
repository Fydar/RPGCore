using Newtonsoft.Json;
using RPGCore.Traits;
using System.Diagnostics;

namespace RPGCore.Demo.BoardGame
{
	public class Building
	{
		[JsonIgnore]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public GameTile Tile { get; internal set; }

		public string Identifier { get; }
		public StatInstance LocalReward { get; }

		public Building(string identifier)
		{
			Identifier = identifier;
		}

		public override string ToString()
		{
			return Identifier;
		}
	}
}
