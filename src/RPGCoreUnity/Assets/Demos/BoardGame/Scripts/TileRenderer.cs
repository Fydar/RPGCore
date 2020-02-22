using RPGCore.Demo.BoardGame;
using UnityEngine;

namespace RPGCoreUnity.Demo.BoardGame
{
	public class TileRenderer : MonoBehaviour
	{
		public GameObject Resource;
		public GameObject Building;

		private GameTile Tile;

		public void Render(GameTile tile)
		{
			Tile = tile;
		}

		public void UpdateRendering()
		{
			Resource.SetActive(Tile.Resource != null);
			Building.SetActive(Tile.Building != null);
		}
	}
}
