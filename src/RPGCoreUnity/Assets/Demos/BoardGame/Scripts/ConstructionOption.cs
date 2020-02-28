using RPGCore.Demo.BoardGame;
using RPGCore.Demo.BoardGame.Models;
using UnityEngine;
using UnityEngine.UI;

namespace RPGCoreUnity.Demo.BoardGame
{
	public class ConstructionOption : MonoBehaviour
	{
		[SerializeField] private Text buildingName;

		public void Render(BuildingTemplate buildingTemplate, OffsetAndRotation offsetAndRotation)
		{
			if (buildingName != null)
			{
				buildingName.text = buildingTemplate.DisplayName;
			}
		}
	}
}
