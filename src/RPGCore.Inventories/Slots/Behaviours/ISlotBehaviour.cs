using System;

namespace RPGCore.Inventory.Slots
{
	public interface ISlotBehaviour
	{
		void OnItemEnter ();
		void OnItemExit ();
	}
}
