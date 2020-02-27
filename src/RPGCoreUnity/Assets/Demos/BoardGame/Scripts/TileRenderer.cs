using RPGCore.Demo.BoardGame;
using UnityEngine;

namespace RPGCoreUnity.Demo.BoardGame
{
	public class TileRenderer : MonoBehaviour
	{
		public GameObject Resource;
		public GameObject Building;

		private GameViewRenderer GameViewRenderer;
		private GameTile Tile;

		public Integer2 Position { get; private set; }

		public void Render(GameViewRenderer gameViewRenderer, GameTile tile, Integer2 position)
		{
			GameViewRenderer = gameViewRenderer;
			Tile = tile;
			Position = position;
		}

		public void UpdateRendering()
		{
			Resource.SetActive(Tile.Resource != null);
			Building.SetActive(Tile.Building != null);
		}

		private void OnMouseEnter()
		{
			GameViewRenderer.OnTileHover(this);
		}

		private void OnMouseExit()
		{
			GameViewRenderer.OnTileUnhover(this);
		}
	}
}
