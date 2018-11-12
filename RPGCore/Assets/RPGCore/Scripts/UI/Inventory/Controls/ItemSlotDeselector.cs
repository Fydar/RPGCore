using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using RPGCore;
using RPGCore.Tooltips;

namespace RPGCore.Inventories
{
	public class ItemSlotDeselector : MonoBehaviour
	{
		private void OnDisable ()
		{
			ItemSlotManager[] managers = GetComponentsInChildren<ItemSlotManager> (true);
			foreach (ItemSlotManager manager in managers)
			{
				manager.Unhover ();
			}

			ItemRenderer[] renderers = GetComponentsInChildren<ItemRenderer> (true);
			foreach (ItemRenderer renderer in renderers)
			{
				renderer.Unhover ();
			}
		}
	}
}