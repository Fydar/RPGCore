using RPGCore.Demo.BoardGame;
using UnityEngine;

namespace RPGCoreUnity.Demo.BoardGame
{
	public class GameBoardRenderer : MonoBehaviour
	{
		public TileRenderer[,] TileRenderers;

		private GameBoard Board;

		public void Render(GameBoard board)
		{
			Board = board;
		}

		public void UpdateRendering()
		{
			foreach (var tile in TileRenderers)
			{
				tile.UpdateRendering();
			}
		}
	}
}
