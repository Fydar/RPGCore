using Newtonsoft.Json;
using RPGCore.Demo.BoardGame.Models;
using RPGCore.Packages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace RPGCore.Demo.BoardGame
{
	public class LobbyView
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GameplayView gameplay;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private LobbyPlayerCollection players;

		public LobbyPlayerCollection Players
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
					return;
				}

				if (value.Lobby == null)
				{
					players = value;
					players.Lobby = this;
				}
				else
				{
					throw new InvalidOperationException($"Can't add a {nameof(LobbyPlayerCollection)} to this {nameof(LobbyView)} as it belongs to another {nameof(LobbyView)}.");
				}
			}
		}

		public GameplayView Gameplay
		{
			get
			{
				return gameplay;
			}
			set
			{
				if (value == null)
				{
					gameplay = null;
					return;
				}

				if (value.Lobby == null)
				{
					gameplay = value;
					gameplay.Lobby = this;
				}
				else
				{
					throw new InvalidOperationException($"Can't add a {nameof(GameplayView)} to this {nameof(LobbyView)} as it belongs to another {nameof(LobbyView)}.");
				}
			}
		}

		public IExplorer GameData { get; private set; }
		public Dictionary<string, BuildingTemplate> BuildingTemplates { get; private set; }

		public LobbyView()
		{
			Players = new LobbyPlayerCollection();
			Gameplay = new GameplayView();
		}

		public void SetupDependancies(IExplorer gameData)
		{
			GameData = gameData;
			BuildingTemplates = LoadAll<BuildingTemplate>(GameData.Tags["type-building"])
				.ToDictionary(template => template.Identifier);
		}

		public static T[] LoadAll<T>(IResourceCollection resources)
		{
			var deserializedResources = new T[resources.Count];

			int index = 0;
			foreach (var resource in resources)
			{
				deserializedResources[index] = Load<T>(resource);
				index++;
			}
			return deserializedResources;
		}

		public static T Load<T>(IResource resource)
		{
			var serializer = new JsonSerializer();

			using var data = resource.LoadStream();
			using var sr = new StreamReader(data);
			using var reader = new JsonTextReader(sr);

			return serializer.Deserialize<T>(reader);
		}

		public void Apply(LobbyViewProcedure action)
		{
			action.Apply(this);
		}
	}
}
