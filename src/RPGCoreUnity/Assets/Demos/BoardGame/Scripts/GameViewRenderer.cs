using RPGCore.Behaviour;
using RPGCore.Demo.BoardGame;
using System.Collections.Generic;
using UnityEngine;

namespace RPGCoreUnity.Demo.BoardGame
{
	public class GameViewRenderer : MonoBehaviour
	{
		[Header("Grid")]
		public GameObject TilePrefab;

		private GameBoardRenderer[] Boards;

		private GameView Game;
		private LocalId Player1;
		private LocalId Player2;

		private void Awake()
		{
			Player1 = LocalId.NewShortId();
			Player2 = LocalId.NewShortId();

			Game = new GameView()
			{
				CurrentPlayersTurn = 0,
				DeclaredResource = false,
				Players = new GamePlayer[]
				{
					new GamePlayer()
					{
						OwnerId = Player1,
						Board = new GameBoard(4, 4)
					},
					new GamePlayer()
					{
						OwnerId = Player2,
						Board = new GameBoard(4, 4)
					}
				}
			};
		}

		private void Start()
		{
			Boards = new GameBoardRenderer[Game.Players.Length];

			for (int i = 0; i < Game.Players.Length; i++)
			{
				var player = Game.Players[i];
				var playerHolder = new GameObject(player.OwnerId.ToString());
				playerHolder.transform.position = new Vector3(i * 5.0f, 0.0f, 0.0f);

				var boardRenderer = playerHolder.AddComponent<GameBoardRenderer>();
				boardRenderer.TileRenderers = new TileRenderer[player.Board.Width, player.Board.Height];

				Boards[i] = boardRenderer;

				for (int x = 0; x < player.Board.Width; x++)
				{
					for (int y = 0; y < player.Board.Height; y++)
					{
						var tileClone = Instantiate(TilePrefab,
							Vector3.zero,
							Quaternion.LookRotation(Vector3.down),
							playerHolder.transform);

						tileClone.transform.localPosition = new Vector3(x + 0.5f, 0.0f, y + 0.5f);
						tileClone.name = $"{x},{y}";

						var tileRenderer = tileClone.GetComponent<TileRenderer>();
						boardRenderer.TileRenderers[x, y] = tileRenderer;

						var tile = player.Board[x, y];
						tileRenderer.Render(tile);
					}
				}
			}

			StartCoroutine(Run());
		}

		private void UpdateRendering()
		{
			foreach (var board in Boards)
			{
				board.UpdateRendering();
			}
		}

		private IEnumerator<YieldInstruction> Run()
		{
			var actions = new GameViewAction[]
			{
				new DeclareResourceAction()
				{
					Client = Player1,
					ResourceIdentifier = "1"
				},
				new PlaceResourceAction()
				{
					Client = Player1,
					ResourceIdentifier = "1",
					ResourcePosition = new Integer2(1, 1)
				},
				new PlaceResourceAction()
				{
					Client = Player2,
					ResourceIdentifier = "1",
					ResourcePosition = new Integer2(3, 3)
				},
				new EndTurnAction()
				{
					Client = Player1
				},

				new DeclareResourceAction()
				{
					Client = Player2,
					ResourceIdentifier = "2"
				},
				new PlaceResourceAction()
				{
					Client = Player1,
					ResourceIdentifier = "2",
					ResourcePosition = new Integer2(0, 1)
				},
				new PlaceResourceAction()
				{
					Client = Player2,
					ResourceIdentifier = "2",
					ResourcePosition = new Integer2(2, 3)
				},
				new EndTurnAction()
				{
					Client = Player2
				},

				new DeclareResourceAction()
				{
					Client = Player1,
					ResourceIdentifier = "3"
				},
				new PlaceResourceAction()
				{
					Client = Player1,
					ResourceIdentifier = "3",
					ResourcePosition = new Integer2(1, 2)
				},
				new PlaceResourceAction()
				{
					Client = Player2,
					ResourceIdentifier = "3",
					ResourcePosition = new Integer2(2, 2)
				},
				new BuildBuildingAction()
				{
					Client = Player1,
					BuildingIdentifier = "building",
					BuildingPosition = new Integer2(3, 3),
					Offset = new Integer2(2, 1),
					Orientation = BuildingOrientation.MirrorXandY
				},
				new EndTurnAction()
				{
					Client = Player1
				}
			};

			UpdateRendering();

			foreach (var action in actions)
			{
				while (!Input.GetMouseButtonDown(0))
				{
					yield return null;
				}

				Debug.Log($"Running action {action.GetType()}");

				Game.Apply(action);

				UpdateRendering();
				yield return null;
			}
		}
	}
}
