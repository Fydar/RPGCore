using UnityEngine;
using System.Collections;

namespace RPGCore.Inventories
{
	public class CanvasMouse : MonoBehaviour
	{
		private Canvas canvas;

		private void Awake ()
		{
			canvas = GetComponent<Canvas> ();
		}

		public Vector3 ScreenToCanvas (Vector3 screen)
		{
			if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
			{
				return screen;
			}

			Vector2 pos;
			RectTransformUtility.ScreenPointToLocalPointInRectangle (transform as RectTransform,
				Input.mousePosition, Camera.main, out pos);

			return transform.TransformPoint (pos);
		}
	}
}