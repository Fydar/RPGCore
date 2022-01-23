using RPGCore.Behaviour;
using RPGCore.Demo.BoardGame.Models;
using RPGCore.Packages;
using RPGCore.Traits;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RPGCore.Demo.BoardGame;

public class GameServer
{
	public LobbyView Lobby;

	public event Action<LobbyViewProcedure> OnRemoteCall;

	public GameServer()
	{

	}

	public void StartHosting(IExplorer explorer)
	{
		Lobby = new LobbyView();

		Lobby.SetupDependancies(explorer);
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
			var gameRules = LobbyView.Load<GameRulesTemplate>(Lobby.GameData.Resources["gamerules/default-rules.json"]);

			var packTemplates = LobbyView.LoadAll<BuildingPackTemplate>(Lobby.GameData.Tags["type-buildingpack"])
				.ToDictionary(template => template.Identifier);

			var resourceTemplates = LobbyView.LoadAll<ResourceTemplate>(Lobby.GameData.Tags["type-resource"]);

			var rand = new Random();

			var sharedBuildings = gameRules.SharedCards
				.Select(card => packTemplates[card])
				.Select(pack => Lobby.BuildingTemplates.Where(building => building.Value.PackIdentifier == pack.Identifier).ToArray())
				.ToArray();

			var playerBuildings = gameRules.PlayerCards
				.Select(card => packTemplates[card])
				.Select(pack => Lobby.BuildingTemplates.Where(building => building.Value.PackIdentifier == pack.Identifier).ToArray())
				.ToArray();

			var globalCardSlots = sharedBuildings.Select(pack =>
			{
				if (pack == null || pack.Length == 0)
				{
					return null;
				}

				return new GlobalCardSlot()
				{
					BuildingIdentifier = pack[rand.Next(0, pack.Length)].Key
				};
			}).ToArray();

			var gameplayPlayers = new List<GameplayPlayer>(Lobby.Players.Count);
			foreach (var lobbyPlayer in Lobby.Players)
			{
				var gameplayPlayer = new GameplayPlayer
				{
					OwnerId = lobbyPlayer.OwnerId,
					CurrentScore = new StatInstance(),
					ResourceHand = new List<string>(),
				};

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

				gameplayPlayer.SpecialCards = thisPlayerBuildings;
				gameplayPlayer.Board = new GameBoard();

				gameplayPlayer.Buildings = globalCardSlots.Select(building =>
				{
					var buildingTemplate = Lobby.BuildingTemplates[building.BuildingIdentifier];

					return new BoardCardSlot()
					{
						BuildingIdentifier = building.BuildingIdentifier,
						BoardEffect = buildingTemplate.BoardEffectGraph?.Unpack()?.Create()
					};
				}).ToList();

				gameplayPlayers.Add(gameplayPlayer);
			}

			var procedure = new StartGameProcedure()
			{
				Gameplay = new GameplayView()
				{
					Players = new GameplayPlayerCollection(gameplayPlayers),
					Buildings = globalCardSlots,
					CurrentPlayersTurn = 0,
					DeclaredResource = false,
				}
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
		else if (command is EndTurnCommand endTurnCommand)
		{
			var procedure = new EndTurnProcedure()
			{
				Player = localId
			};

			RemoteCall(procedure);
		}
	}

	private void RemoteCall(LobbyViewProcedure procedure)
	{
		Lobby.Apply(procedure);

		OnRemoteCall?.Invoke(procedure);
	}
}
