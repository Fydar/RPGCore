using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace RPGCore.Demo.BoardGame
{
	public class GameplayView
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GameplayPlayerCollection players;

		[JsonIgnore]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public LobbyView Lobby { get; internal set; }

		public int CurrentPlayersTurn;
		public bool DeclaredResource;
		public GlobalCardSlot[] Buildings;

		public GameplayPlayerCollection Players
		{
			get
			{
				return players;
			}
			set
			{
				if (value == null)
				{
					players = null;
				}

				if (value.Gameplay == null)
				{
					players = value;
					players.Gameplay = this;
				}
				else
				{
					throw new InvalidOperationException($"Can't add a {nameof(GameplayPlayerCollection)} to this {nameof(GameplayView)} as it belongs to another {nameof(GameplayView)}.");
				}
			}
		}

		public GameplayView()
		{
			Players = new GameplayPlayerCollection();
		}
	}
}
