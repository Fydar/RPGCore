using RPGCore.Behaviour;
using RPGCore.Packages;
using System;
using System.Collections.Generic;

namespace RPGCore.Demo.BoardGame.CommandLine
{
	public sealed class GameRunner
	{
		public GameView GameView { get; }

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

			var gameServer = new GameServer();
			gameServer.StartHosting(projectExplorer);

			var playerA = LocalId.NewShortId();
			var playerB = LocalId.NewShortId();

			gameServer.OnClientConnected(playerA, "Player A");
			gameServer.OnClientConnected(playerB, "Player B");

			gameServer.AcceptInput(playerA, new StartGameCommand() { });
		}

		public void Update()
		{
		}
	}
}
