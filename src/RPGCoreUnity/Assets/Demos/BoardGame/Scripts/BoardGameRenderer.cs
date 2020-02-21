using RPGCore.Demo.BoardGame;
using UnityEngine;

namespace RPGCoreUnity.Demo.BoardGame
{
	public class BoardGameRenderer : MonoBehaviour
	{
		private GameView Game;

		private void Awake()
		{
			Game = new GameView()
			{
				Players = new GamePlayer[]
				{
					new GamePlayer()
					{
						Board = new GameBoard(4, 4)
					}
				}
			};
		}

		private void Update()
		{

		}
	}
}
