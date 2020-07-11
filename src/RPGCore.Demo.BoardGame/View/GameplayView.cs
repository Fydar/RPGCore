using Newtonsoft.Json;
using System.Diagnostics;

namespace RPGCore.Demo.BoardGame
{
	public class GameplayView : IChildOf<LobbyView>
	{
		[JsonIgnore]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GameplayPlayerCollection players;

		public int CurrentPlayersTurn;
		public bool DeclaredResource;
		public GlobalCardSlot[] Buildings;

		[JsonIgnore]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public LobbyView Parent { get; set; }

		public GameplayPlayerCollection Players
		{
			get => HierachyHelper.Get(this, ref players);
			set => HierachyHelper.Set(this, ref players, value);
		}

		public GameplayView()
		{
			Players = new GameplayPlayerCollection();
		}
	}
}
