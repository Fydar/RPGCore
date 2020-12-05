using RPGCore.Behaviour;
using RPGCore.FileTree.FileSystem;
using RPGCore.Packages;
using RPGCore.Projects;
using RPGCore.Projects.Extensions.MetaFiles;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RPGCore.Demo.BoardGame.CommandLine
{
	public sealed class GameRunner
	{
		public LobbyView GameView { get; }

		public GameRunner()
		{
		}

		public async Task StartAsync()
		{
			// Import the project
			var importPipeline = ImportPipeline.Create()
				.UseImporter(new BoardGameResourceImporter())
				.UseJsonMetaFiles(options =>
				{
					options.IsMetaFilesOptional = true;
				})
				.Build();

			var projectExplorer = ProjectExplorer.Load("Content/BoardGame", importPipeline);

			// Build the project to a package.
			var consoleRenderer = new BuildConsoleRenderer();

			var buildPipeline = new BuildPipeline();
			buildPipeline.BuildActions.Add(consoleRenderer);

			consoleRenderer.DrawProgressBar(32);

			projectExplorer.ExportZippedToDirectory(buildPipeline, "BoardGame/Temp");
			var packageExplorer = PackageExplorer.LoadFromFileAsync("BoardGame/Temp/BoardGame.bpkg").Result;

			var dest = new FileSystemArchive(new DirectoryInfo("BoardGame/Temp"));
			await packageExplorer.Source.CopyIntoAsync(dest.RootDirectory, "Fast");

			var cottage = packageExplorer.Resources["buildings/cottage.json"];

			foreach (var dep in cottage.Dependencies)
			{
				Console.WriteLine($"{dep.Key}: {dep.Resource?.Name}");
			}

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



			gameServer.AcceptInput(playerA, new BuildBuildingCommand()
			{
				BuildingIdentifier = "cottage",
				BuildingPosition = new Integer2(1, 1),
				Offset = new Integer2(1, 1),
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
								Console.ForegroundColor = ConsoleColor.DarkGray;
								Console.WriteLine($"(Current players turn)");
							}
							else
							{
								Console.WriteLine();
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
							else
							{
								Console.WriteLine();
							}

							Console.ForegroundColor = ConsoleColor.DarkGray;
							for (int x = 0; x < (gameplayPlayer.Board.Width * 2) + 4; x++)
							{
								Console.Write('\u2591');
							}
							Console.WriteLine();

							for (int y = 4 - 1; y >= 0; y--)
							{
								Console.Write('\u2591');
								Console.Write('\u2591');
								Console.ResetColor();

								for (int x = 0; x < gameplayPlayer.Board.Width; x++)
								{
									var tile = gameplayPlayer.Board[x, y];

									if (tile.IsEmpty)
									{
										Console.Write("  ");
									}
									else if (tile.Building != null)
									{
										Console.ForegroundColor = ConsoleColor.DarkGray;
										Console.Write('\u2588');
										Console.Write('\u2588');
										Console.ResetColor();
									}
									else
									{
										Console.Write(tile.ToChar());
										Console.Write(tile.ToChar());
									}
								}

								Console.ForegroundColor = ConsoleColor.DarkGray;
								Console.Write('\u2591');
								Console.Write('\u2591');
								Console.WriteLine();
							}

							Console.ForegroundColor = ConsoleColor.DarkGray;
							for (int x = 0; x < (gameplayPlayer.Board.Width * 2) + 4; x++)
							{
								Console.Write('\u2591');
							}
							Console.ResetColor();

							Console.WriteLine();
							index++;
						}
						else
						{
							Console.WriteLine($"Player {lobbyPlayer.OwnerId}");
							Console.WriteLine($"  (waiting for game to start)");
						}
						Console.WriteLine();
					}
				}
			}
		}
	}
}
