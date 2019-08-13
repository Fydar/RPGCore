using UnityEngine;

namespace RPGCore.Inventories
{
	public class ItemSlotDeselector : MonoBehaviour
	{
		private void OnDisable ()
		{
			var managers = GetComponentsInChildren<ItemSlotManager> (true);
			foreach (var manager in managers)
			{
				manager.Unhover ();
			}

			var renderers = GetComponentsInChildren<ItemRenderer> (true);
			foreach (var renderer in renderers)
			{
				renderer.Unhover ();
			}
		}
	}
}

