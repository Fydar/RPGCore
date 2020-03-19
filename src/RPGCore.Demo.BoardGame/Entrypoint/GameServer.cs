using RPGCore.Behaviour;
using RPGCore.Demo.BoardGame.Models;
using RPGCore.Packages;
using RPGCore.Traits;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RPGCore.Demo.BoardGame
{
	public class GameServer
	{
		public GameView ServerView;

		public event Action<GameViewProcedure> OnRemoteCall;

		public GameServer()
		{

		}

		public void StartHosting(IExplorer explorer)
		{
			ServerView = new GameView();

			ServerView.StartGame(explorer);
		}

		public void OnClientConnected(LocalId localId, string displayName)
		{
			var playerJoinedProcedure = new PlayerJoinedProcedure()
			{
				DisplayName = displayName,
				OwnerId = localId
			};

			RemoteCall(playerJoinedProcedure);
		}

		public void OnClientDisconnected(LocalId localId)
		{
			var playerJoinedProcedure = new PlayerLeftProcedure()
			{
				OwnerId = localId
			};

			RemoteCall(playerJoinedProcedure);
		}

		public void AcceptInput(LocalId localId, GameCommand command)
		{
			if (command is StartGameCommand startGameCommand)
			{
				var gameRules = GameView.Load<GameRulesTemplate>(ServerView.GameData.Resources["gamerules/default-rules.json"]);

				var packTemplates = GameView.LoadAll<BuildingPackTemplate>(ServerView.GameData.Tags["type-buildingpack"])
					.ToDictionary(template => template.Identifier);

				var resourceTemplates = GameView.LoadAll<ResourceTemplate>(ServerView.GameData.Tags["type-resource"]);

				var rand = new Random();

				var sharedBuildings = gameRules.SharedCards
					.Select(card => packTemplates[card])
					.Select(pack => ServerView.BuildingTemplates.Where(building => building.Value.PackIdentifier == pack.Identifier).ToArray())
					.ToArray();

				var playerBuildings = gameRules.PlayerCards
					.Select(card => packTemplates[card])
					.Select(pack => ServerView.BuildingTemplates.Where(building => building.Value.PackIdentifier == pack.Identifier).ToArray())
					.ToArray();

				string[] buildings = sharedBuildings.Select(pack =>
				{
					if (pack == null || pack.Length == 0)
					{
						return null;
					}

					return pack[rand.Next(0, pack.Length)].Key;
				}).ToArray();

				foreach (var player in ServerView.PlayerStates)
				{
					var thisPlayerBuildings = playerBuildings
						.Select(pack =>
						{
							if (pack == null || pack.Length == 0)
							{
								return null;
							}

							return pack[rand.Next(0, pack.Length)].Key;
						})
						.Select(cardId => new SpecialCardSlot() { BuildingIdentifier = cardId })
						.ToList();

					player.Board = new GameBoard();
					player.CurrentScore = new StatInstance();
					player.ResourceHand = new List<string>();
					player.SpecialCards = thisPlayerBuildings;
				}

				var procedure = new StartGameProcedure()
				{
					Buildings = buildings,
					PlayerStates = ServerView.PlayerStates
				};

				RemoteCall(procedure);
			}
			else if (command is DeclareResourceCommand declareResourceCommand)
			{
				var procedure = new DeclareResourceProcedure()
				{
					Player = localId,
					ResourceIdentifier = declareResourceCommand.ResourceIdentifier
				};

				RemoteCall(procedure);
			}
			else if (command is PlaceResourceCommand placeResourceCommand)
			{
				var procedure = new PlaceResourceProcedure()
				{
					Player = localId,
					ResourceIdentifier = placeResourceCommand.ResourceIdentifier,
					ResourcePosition = placeResourceCommand.ResourcePosition
				};

				RemoteCall(procedure);
			}
			else if (command is BuildBuildingCommand buildBuildingCommand)
			{
				var procedure = new BuildBuildingProcedure()
				{
					Player = localId,
					BuildingIdentifier = buildBuildingCommand.BuildingIdentifier,
					BuildingPosition = buildBuildingCommand.BuildingPosition,
					Offset = buildBuildingCommand.Offset,
					Orientation = buildBuildingCommand.Orientation
				};

				RemoteCall(procedure);
			}
		}

		private void RemoteCall(GameViewProcedure procedure)
		{
			ServerView.Apply(procedure);

			OnRemoteCall?.Invoke(procedure);
		}
	}
}
