using UnityEngine;

namespace RPGCoreUnity.Demo.BoardGame
{
	public class SelectionRenderer : MonoBehaviour
	{
		[Header("Height Scaling")]
		[SerializeField] private float maxScale = 1.0f;
		[SerializeField] private float heightScaleDamped = 0.3f;

		[Header("Positioning")]
		[SerializeField] private float positionDamped = 0.3f;

		private PlayerSelection playerSelection;
		private Rect currentRect;
		private float currentHeight;

		public void Render(PlayerSelection playerSelection)
		{
			this.playerSelection = playerSelection;
		}

		private void Update()
		{
			if (playerSelection == null)
			{
				return;
			}

			var targetRect = playerSelection.AsRect;
			float targetHeight = playerSelection.isSelected
				? maxScale
				: 0.0f;

			if (playerSelection.isSelected)
			{
				currentRect = RectLerp(currentRect, targetRect, Time.deltaTime * positionDamped);
			}
			else
			{
				currentRect = targetRect;
			}

			currentHeight = Mathf.Lerp(currentHeight, targetHeight, Time.deltaTime * heightScaleDamped);

			transform.position = new Vector3(currentRect.center.x, 0.0f, currentRect.center.y);
			transform.localScale = new Vector3(currentRect.width, currentHeight, currentRect.height);
		}

		private static Rect RectLerp(Rect from, Rect to, float time)
		{
			return new Rect()
			{
				x = Mathf.Lerp(from.x, to.x, time),
				y = Mathf.Lerp(from.y, to.y, time),
				width = Mathf.Lerp(from.width, to.width, time),
				height = Mathf.Lerp(from.height, to.height, time),
			};
		}
	}
}
