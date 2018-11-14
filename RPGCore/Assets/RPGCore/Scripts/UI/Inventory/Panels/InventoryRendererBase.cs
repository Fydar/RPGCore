using System.Collections.Generic;
using UnityEngine;

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