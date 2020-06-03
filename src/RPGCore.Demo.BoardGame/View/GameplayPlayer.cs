using Newtonsoft.Json;
using RPGCore.Behaviour;
using RPGCore.Traits;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.Demo.BoardGame
{
	public class GameplayPlayer
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GameBoard board;

		public LocalId OwnerId { get; set; }
		public StatInstance CurrentScore { get; set; }
		public List<BoardCardSlot> Buildings { get; set; }
		public List<SpecialCardSlot> SpecialCards { get; set; }
		public List<string> ResourceHand { get; set; }

		[JsonIgnore]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public GameplayView Gameplay { get; internal set; }

		public GameBoard Board
		{
			get
			{
				return board;
			}
			set
			{
				if (value == null)
				{
					board = null;
				}

				if (value.Owner == null)
				{
					board = value;
					board.Owner = this;
				}
				else
				{
					throw new InvalidOperationException($"Can't add a {nameof(GameBoard)} to this {nameof(LobbyPlayer)} as it belongs to another {nameof(GameBoard)}.");
				}
			}
		}
	}
}
