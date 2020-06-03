using RPGCore.Demo.BoardGame.Models;
using System;
using UnityEngine;

namespace RPGCoreUnity.Demo.BoardGame
{
	[Serializable] public class ConstructionOptionPool : UIPool<ConstructionOption> { }

	public class ConstructionDialogue : MonoBehaviour
	{
		[SerializeField] private GameViewRenderer gameViewRenderer;

		[Space]
		[SerializeField] private RectTransform holder;
		[SerializeField] private ConstructionOptionPool optionsPool;

		private void Update()
		{
			optionsPool.Flush();

			var board = gameViewRenderer.Lobby.Gameplay.Players[0].Board;

			var building = new BuildingTemplate()
			{
				DisplayName = "Name",
				Recipe = new string[,]
				{
					{ "x", "x" }
				}
			};

			foreach (var option in board.AllBuildableLocations(building, gameViewRenderer.ThisPlayerSelection.AsIntegerRect))
			{
				var optionRenderer = optionsPool.Grab(holder);

				optionRenderer.Render(building, option);
			}
		}
	}
}
