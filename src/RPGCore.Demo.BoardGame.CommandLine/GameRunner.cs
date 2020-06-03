using RPGCore.Behaviour;
using RPGCore.Packages;
using System;
using System.Collections.Generic;

namespace RPGCore.Demo.BoardGame.CommandLine
{
	public sealed class GameRunner
	{
		public LobbyView GameView { get; }

		public GameRunner()
		{
		}

		public void Start()
		{
			// Import the project
			var importPipeline = new ImportPipeline();
			importPipeline.ImportProcessors.Add(
				new BoardGameResourceImporter());
			var projectExplorer = ProjectExplorer.Load("Content/BoardGame", importPipeline);

			// Build the project to a package.
			var consoleRenderer = new BuildConsoleRenderer();
			var buildPipeline = new BuildPipeline()
			{
				ImportPipeline = importPipeline,
				Exporters = new List<ResourceExporter>()
				{
					new JsonMinimizerResourceExporter()
				},
				BuildActions = new List<IBuildAction>()
				{
					consoleRenderer
				}
			};
			consoleRenderer.DrawProgressBar(32);
			projectExplorer.Export(buildPipeline, "BoardGame/Temp");
			Console.WriteLine("Exported package...");

			Step();

			var gameServer = new GameServer();
			gameServer.StartHosting(projectExplorer);

			var playerA = LocalId.NewShortId();
			var playerB = LocalId.NewShortId();

			gameServer.OnClientConnected(playerA, "Player A");
			gameServer.OnClientConnected(playerB, "Player B");
			DrawGameState(gameServer.Lobby);
			Step();

			gameServer.AcceptInput(playerA, new StartGameCommand() { });
			DrawGameState(gameServer.Lobby);
			Step();

			gameServer.AcceptInput(playerA, new DeclareResourceCommand()
			{
				ResourceIdentifier = "x"
			});
			DrawGameState(gameServer.Lobby);
			Step();

			gameServer.AcceptInput(playerA, new PlaceResourceCommand()
			{
				ResourceIdentifier = "x",
				ResourcePosition = new Integer2(2, 2)
			});
			DrawGameState(gameServer.Lobby);
			Step();

			gameServer.AcceptInput(playerB, new PlaceResourceCommand()
			{
				ResourceIdentifier = "x",
				ResourcePosition = new Integer2(3, 1)
			});
			DrawGameState(gameServer.Lobby);
			Step();

			gameServer.AcceptInput(playerB, new EndTurnCommand());
			DrawGameState(gameServer.Lobby);
		}

		public void Update()
		{
		}

		public void Step()
		{
			Console.ReadLine();
			Console.Clear();
		}

		private void DrawGameState(LobbyView lobby)
		{
			lock (Console.Out)
			{
				int index = 0;
				if (lobby.Players == null
					|| lobby.Players.Count == 0)
				{
					Console.WriteLine($"No players");
				}
				else
				{
					foreach (var lobbyPlayer in lobby.Players)
					{
						if (lobby.Gameplay.Players.TryGetPlayer(lobbyPlayer.OwnerId, out var gameplayPlayer))
						{
							Console.WriteLine($"Player {lobbyPlayer.OwnerId}");
							if (lobby.Gameplay.CurrentPlayersTurn == index)
							{
								Console.WriteLine($"  (Current players turn)");
							}

							if (gameplayPlayer.ResourceHand != null
								&& gameplayPlayer.ResourceHand.Count != 0)
							{
								foreach (string resource in gameplayPlayer.ResourceHand)
								{
									Console.Write('[');
									Console.ForegroundColor = ConsoleColor.Cyan;
									Console.Write(resource);
									Console.ResetColor();
									Console.Write(']');
								}
								Console.WriteLine();
							}

							for (int y = 4 - 1; y >= 0; y--)
							{
								for (int x = 0; x < gameplayPlayer.Board.Width; x++)
								{
									var tile = gameplayPlayer.Board[x, y];

									Console.Write(tile.ToChar());
								}
								Console.WriteLine();
							}
							Console.WriteLine();
							index++;
						}
						else
						{
							Console.WriteLine($"Player {lobbyPlayer.OwnerId}");
							Console.WriteLine($"  (waiting for game to start)");
						}
					}
				}
			}
		}
	}
}
