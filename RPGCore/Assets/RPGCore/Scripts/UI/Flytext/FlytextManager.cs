using UnityEngine;

namespace RPGCore.UI
{
	public class FlytextManager : MonoBehaviour
	{
		public static FlytextManager instance;

		[SerializeField]
		private FloatingText popupText;
		[SerializeField]
		private GameObject canvas;

		public FlytextManager Instance
		{
			get
			{
				return instance;
			}
		}

		private void Awake ()
		{
			instance = this;
		}

		public static void CreateDamagePopup (int healthChange, Transform target)
		{
			FloatingText clone = Instantiate (instance.popupText, instance.canvas.transform);

			clone.SetText (target, healthChange);
		}

		public static void CreateBuffPopup (string buffText, Transform target)
		{
			FloatingText clone = Instantiate (instance.popupText, instance.canvas.transform);

			clone.SetText (target, buffText);
		}
	}
}

