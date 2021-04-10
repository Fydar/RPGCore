using Newtonsoft.Json;
using RPGCore.Behaviour;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RPGCore.Demo.BoardGame
{
	public class GameplayPlayerCollection : IChildOf<GameplayView>, IEnumerable<GameplayPlayer>
	{
		[JsonProperty("players")]
		private readonly List<GameplayPlayer> internalCollection;

		public int Count => internalCollection.Count;

		[JsonIgnore]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public GameplayView Parent { get; set; }

		public GameplayPlayer this[LocalId ownerId]
		{
			get
			{
				foreach (var player in internalCollection)
				{
					if (player.OwnerId == ownerId)
					{
						return player;
					}
				}
				throw new KeyNotFoundException($"Unable to find a player with ID \"{ownerId}\" in this {nameof(GameplayPlayerCollection)}");
			}
		}

		public GameplayPlayerCollection()
		{
			internalCollection = new List<GameplayPlayer>();
		}

		public GameplayPlayerCollection(List<GameplayPlayer> players)
		{
			internalCollection = players;
		}

		public void AddPlayer(GameplayPlayer gameplayLayer)
		{
			if (gameplayLayer.Gameplay == null)
			{
				gameplayLayer.Gameplay = Parent;
				internalCollection.Add(gameplayLayer);
			}
			else if (gameplayLayer.Gameplay == Parent)
			{
				throw new ArgumentException($"Can't add a {nameof(GameplayPlayer)} to this {nameof(GameplayPlayerCollection)} as it already belongs to this {nameof(LobbyView)}.");
			}
			else
			{
				throw new InvalidOperationException($"Can't add a {nameof(GameplayPlayer)} to this {nameof(GameplayPlayerCollection)} as it belongs to another {nameof(LobbyView)}.");
			}
		}

		public bool ContainsPlayer(GameplayPlayer gameplayLayer)
		{
			return internalCollection.Contains(gameplayLayer);
		}

		public bool ContainsPlayerWithId(LocalId ownerId)
		{
			return internalCollection.Any(player => player.OwnerId == ownerId);
		}

		public bool RemovePlayer(GameplayPlayer gameplayLayer)
		{
			if (gameplayLayer.Gameplay != Parent)
			{
				throw new ArgumentException($"Can't remove a {nameof(GameplayPlayer)} from this {nameof(GameplayPlayerCollection)} as it doesn't recognise this {nameof(LobbyView)} as it's parent.");
			}
			gameplayLayer = null;
			return internalCollection.Remove(gameplayLayer);
		}

		public bool RemovePlayerWithId(LocalId ownerId)
		{
			var player = internalCollection.Find(player => player.OwnerId == ownerId);
			return RemovePlayer(player);
		}

		public bool TryGetPlayer(LocalId ownerId, out GameplayPlayer player)
		{
			foreach (var findPlayer in internalCollection)
			{
				if (findPlayer.OwnerId == ownerId)
				{
					player = findPlayer;
					return true;
				}
			}
			player = null;
			return false;
		}

		public IEnumerator<GameplayPlayer> GetEnumerator()
		{
			return internalCollection.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return internalCollection.GetEnumerator();
		}
	}
}
