using Newtonsoft.Json;
using RPGCore.Behaviour;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RPGCore.Demo.BoardGame;

public class LobbyPlayerCollection : IChildOf<LobbyView>, IEnumerable<LobbyPlayer>
{
	[JsonProperty("players")]
	private readonly List<LobbyPlayer> internalCollection;

	public int Count => internalCollection.Count;

	[JsonIgnore]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public LobbyView Parent { get; set; }

	public LobbyPlayer this[LocalId ownerId]
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
			throw new KeyNotFoundException($"Unable to find a player with ID \"{ownerId}\" in this {nameof(LobbyPlayerCollection)}");
		}
	}

	public LobbyPlayerCollection()
	{
		internalCollection = new List<LobbyPlayer>();
	}

	public LobbyPlayerCollection(List<LobbyPlayer> players)
	{
		internalCollection = players;
	}

	public void AddPlayer(LobbyPlayer lobbyPlayer)
	{
		if (lobbyPlayer.Lobby == null)
		{
			lobbyPlayer.Lobby = Parent;
			internalCollection.Add(lobbyPlayer);
		}
		else if (lobbyPlayer.Lobby == Parent)
		{
			throw new ArgumentException($"Can't add a {nameof(LobbyPlayer)} to this {nameof(LobbyPlayerCollection)} as it already belongs to this {nameof(LobbyView)}.");
		}
		else
		{
			throw new InvalidOperationException($"Can't add a {nameof(LobbyPlayer)} to this {nameof(LobbyPlayerCollection)} as it belongs to another {nameof(LobbyView)}.");
		}
	}

	public bool ContainsPlayer(LobbyPlayer lobbyPlayer)
	{
		return internalCollection.Contains(lobbyPlayer);
	}

	public bool ContainsPlayerWithId(LocalId ownerId)
	{
		return internalCollection.Any(player => player.OwnerId == ownerId);
	}

	public bool RemovePlayer(LobbyPlayer lobbyPlayer)
	{
		if (lobbyPlayer.Lobby != Parent)
		{
			throw new ArgumentException($"Can't remove a {nameof(LobbyPlayer)} from this {nameof(LobbyPlayerCollection)} as it doesn't recognise this {nameof(LobbyView)} as it's parent.");
		}
		lobbyPlayer = null;
		return internalCollection.Remove(lobbyPlayer);
	}

	public bool RemovePlayerWithId(LocalId ownerId)
	{
		var player = internalCollection.Find(player => player.OwnerId == ownerId);
		return RemovePlayer(player);
	}

	public IEnumerator<LobbyPlayer> GetEnumerator()
	{
		return internalCollection.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return internalCollection.GetEnumerator();
	}
}
