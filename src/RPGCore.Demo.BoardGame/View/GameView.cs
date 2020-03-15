using Newtonsoft.Json;
using RPGCore.Behaviour;
using RPGCore.Packages;
using RPGCore.Traits;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RPGCore.Demo.BoardGame
{
	public class GameView
	{
		public int CurrentPlayersTurn;
		public bool DeclaredResource;

		public string[] Buildings;
		public List<GamePlayer> Players;

		public IExplorer GameData { get; private set; }

		public void Create(IExplorer gameData)
		{
			GameData = gameData;

			Players = new List<GamePlayer>();
		}

		public static T[] LoadAll<T>(IResourceCollection resources)
		{
			var deserializedResources = new T[resources.Count];

			var serializer = new JsonSerializer();
			int index = 0;
			foreach (var buildingPackResource in resources)
			{
				using (var data = buildingPackResource.LoadStream())
				using (var sr = new StreamReader(data))
				using (var reader = new JsonTextReader(sr))
				{
					deserializedResources[index] = serializer.Deserialize<T>(reader);
				}
				index++;
			}
			return deserializedResources;
		}

		public void Apply(GameViewProcedure action)
		{
			action.Apply(this);
		}

		public GamePlayer GetPlayerForOwner(LocalId owner)
		{
			return Players.Where(player => player.OwnerId == owner).FirstOrDefault();
		}
	}
}
