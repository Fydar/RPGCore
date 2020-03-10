using Newtonsoft.Json;
using RPGCore.Behaviour;
using RPGCore.Demo.BoardGame.Models;
using RPGCore.Packages;
using RPGCore.Traits;
using System;
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
		public GamePlayer[] Players;

		public IExplorer GameData { get; private set; }

		public void Create(IExplorer gameData)
		{
			GameData = gameData;

			Players = new GamePlayer[]
			{
				new GamePlayer()
				{
					CurrentScore = new StatInstance(),
					SpecialCard = new SpecialCardSlot(),
					Board = new GameBoard(4, 4),
					ResourceHand = new List<string>()
				},
				new GamePlayer()
				{
					CurrentScore = new StatInstance(),
					SpecialCard = new SpecialCardSlot(),
					Board = new GameBoard(4, 4),
					ResourceHand = new List<string>()
				}
			};

			StartGame();
		}

		private void StartGame()
		{
			var packTemplates = LoadAll<BuildingPackTemplate>(GameData.Tags["type-buildingpack"])
				.ToDictionary(template => template.Identifier);

			var resourceTemplates = LoadAll<ResourceTemplate>(GameData.Tags["type-resource"]);

			var buildingTemplates = LoadAll<BuildingTemplate>(GameData.Tags["type-building"])
				.ToDictionary(template => template.Identifier);


			var fullBuildingPacks = packTemplates
				.Select(pack => buildingTemplates
					.Where(building => building.Value.PackIdentifier == pack.Value.Identifier).ToArray()).ToArray();

			var rand = new Random();

			Buildings = fullBuildingPacks.Select(pack =>
			{
				if (pack == null || pack.Length == 0)
					return null;

				return pack[rand.Next(0, pack.Length)].Key;
			}).ToArray();
		}

		static T[] LoadAll<T>(IResourceCollection resources)
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

		public void Apply(GameViewAction action)
		{
			action.Apply(this);
		}

		public GamePlayer GetPlayerForOwner(LocalId owner)
		{
			return Players.Where(player => player.OwnerId == owner).FirstOrDefault();
		}
	}
}
