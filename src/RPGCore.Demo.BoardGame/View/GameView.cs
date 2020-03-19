using Newtonsoft.Json;
using RPGCore.Behaviour;
using RPGCore.Demo.BoardGame.Models;
using RPGCore.Packages;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RPGCore.Demo.BoardGame
{
	public class GameView
	{
		public int CurrentPlayersTurn;
		public bool DeclaredResource;

		public GlobalCardSlot[] Buildings;
		public List<GamePlayer> Players = new List<GamePlayer>();
		public List<GamePlayerState> PlayerStates = new List<GamePlayerState>();

		public IExplorer GameData { get; private set; }
		public Dictionary<string, BuildingTemplate> BuildingTemplates { get; private set; }

		public void StartGame(IExplorer gameData)
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

		public void Apply(GameViewProcedure action)
		{
			action.Apply(this);
		}

		public GamePlayer GetPlayerForOwner(LocalId owner)
		{
			return Players.Where(player => player.OwnerId == owner).FirstOrDefault();
		}

		public GamePlayerState GetStateForOwner(LocalId owner)
		{
			return PlayerStates.Where(player => player.OwnerId == owner).FirstOrDefault();
		}
	}
}
