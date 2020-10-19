using RPGCore.Behaviour;
using RPGCore.Packages;
using RPGCore.Packages.Archives;
using RPGCore.Packages.Extensions.MetaFiles;
using RPGCore.Packages.Pipeline;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace RPGCore.Demo.BoardGame.CommandLine
{
	public sealed class GameRunner
	{
		public LobbyView GameView { get; }

		public GameRunner()
		{
		}

		public static void CompressImageSave(Bitmap bitmap, FileInfo destination, int quality)
		{
			static ImageCodecInfo GetEncoder(ImageFormat format)
			{
				var codecs = ImageCodecInfo.GetImageDecoders();
				foreach (var codec in codecs)
				{
					if (codec.FormatID == format.Guid)
					{
						return codec;
					}
				}
				return null;
			}
			ImageCodecInfo encoder;
			switch (destination.Extension)
			{
				case ".png":
					encoder = GetEncoder(ImageFormat.Png);
					break;

				case ".jpeg":
				case ".jpg":
					encoder = GetEncoder(ImageFormat.Jpeg);
					break;

				case ".bmp":
					encoder = GetEncoder(ImageFormat.Bmp);
					break;

				default:
					throw new InvalidOperationException("Cannot compress a file of type " + destination.Extension);
			}

			var parameters = new EncoderParameters(1);
			parameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

			bitmap.Save(destination.FullName, encoder, parameters);
		}

		public static Bitmap Resize(Bitmap bitmap, int width, int height)
		{
			static Size CalculateResizedDimensions(Image image, int desiredWidth, int desiredHeight)
			{
				double widthScale = (double)desiredWidth / image.Width;
				double heightScale = (double)desiredHeight / image.Height;

				double scale = widthScale < heightScale ? widthScale : heightScale;

				return new Size
				{
					Width = (int)(scale * image.Width),
					Height = (int)(scale * image.Height)
				};
			}

			var newSize = CalculateResizedDimensions(bitmap, width, height);

			var resizedImage = new Bitmap(newSize.Width, newSize.Height, PixelFormat.Format24bppRgb);
			resizedImage.SetResolution(72, 72);

			using var graphics = Graphics.FromImage(resizedImage);

			// set parameters to create a high-quality thumbnail
			graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			graphics.SmoothingMode = SmoothingMode.AntiAlias;
			graphics.CompositingQuality = CompositingQuality.HighQuality;
			graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

			using var attribute = new ImageAttributes();
			attribute.SetWrapMode(WrapMode.TileFlipXY);

			graphics.DrawImage(bitmap,
				new Rectangle(new Point(0, 0), newSize),
				0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel, attribute);
			return resizedImage;
		}


		public void Start()
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
			buildPipeline.Exporters.Add(new BhvrExporter());
			buildPipeline.Exporters.Add(new JsonMinimizerResourceExporter());
			buildPipeline.BuildActions.Add(consoleRenderer);

			consoleRenderer.DrawProgressBar(32);

			var png = projectExplorer.Resources["pngdemo.png"];

			string tmp = Path.Combine(
				Path.GetTempPath(),
				"ImgEdit",
				Guid.NewGuid().ToString() + png.Extension
			);
			Directory.CreateDirectory(new FileInfo(tmp).Directory.FullName);

			using (var stream = png.Content.ArchiveEntry.OpenRead())
			{
				var bitmap = new Bitmap(stream);

				// bitmap = Resize(bitmap, 200, 367);

				byte[] itemData;
				var newItem = (PropertyItem)FormatterServices.GetUninitializedObject(typeof(PropertyItem));

				itemData = Encoding.ASCII.GetBytes("TMP Image Title");
				itemData[^1] = 0; // Strings must be null terminated or they will run together
				newItem.Id = 0x0320; // Title ID
				newItem.Type = 2; // ASCII
				newItem.Len = itemData.Length; // Number of items in the byte array
				newItem.Value = itemData; // The byte array
				bitmap.SetPropertyItem(newItem);

				itemData = Encoding.ASCII.GetBytes("TMP Model");
				itemData[^1] = 0;
				newItem.Id = 0x0110;
				newItem.Type = 2;
				newItem.Len = itemData.Length;
				newItem.Value = itemData;
				bitmap.SetPropertyItem(newItem);

				itemData = Encoding.ASCII.GetBytes("TMP DESCRIPTION :D");
				itemData[^1] = 0;
				newItem.Id = 0x010E;
				newItem.Type = 2;
				newItem.Len = itemData.Length;
				newItem.Value = itemData;
				bitmap.SetPropertyItem(newItem);

				itemData = Encoding.ASCII.GetBytes("HAY I'M AN ARTISTS");
				itemData[^1] = 0;
				newItem.Id = 0x013B;
				newItem.Type = 2;
				newItem.Len = itemData.Length;
				newItem.Value = itemData;
				bitmap.SetPropertyItem(newItem);

				itemData = Encoding.ASCII.GetBytes("LMAO COPYRIGHTED");
				itemData[^1] = 0;
				newItem.Id = 0x8298;
				newItem.Type = 2;
				newItem.Len = itemData.Length;
				newItem.Value = itemData;
				bitmap.SetPropertyItem(newItem);

				CompressImageSave(bitmap, new FileInfo(tmp), 0);
			}

			File.Copy(tmp, png.Content.ArchiveEntry.FullName, true);
			File.Delete(tmp);


			projectExplorer.ExportZippedToDirectory(buildPipeline, "BoardGame/Temp");
			var packageExplorer = PackageExplorer.LoadFromFileAsync("BoardGame/Temp/BoardGame.bpkg").Result;

			var dest = new FileSystemArchive(new DirectoryInfo("BoardGame/Temp"));
			packageExplorer.Source.CopyInto(dest.RootDirectory, "Fast").Wait();

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
