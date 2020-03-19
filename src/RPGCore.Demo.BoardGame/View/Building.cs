using RPGCore.Traits;

namespace RPGCore.Demo.BoardGame
{
	public class Building
	{
		public string Identifier { get; }
		public StatInstance LocalReward { get; }

		public GameTile Tile { get; internal set; }

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
