using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RPGCore.Tooltips;

namespace RPGCore.Inventories
{
	public abstract class InventoryRendererBase : MonoBehaviour
	{
		[Header ("Setup")]
		protected Inventory current;
		protected List<ItemSlotManager> managers;

		public abstract void Setup (Inventory inventory);
		protected virtual void OnSlotAdd (ItemSlotManager slot) { }
		protected virtual void OnSlotRemove (ItemSlotManager slot) { }
	}
}