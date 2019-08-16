using NUnit.Framework;
using RPGCore.Inventory.Slots;
using RPGCore.Items;

namespace RPGCore.Inventories.UnitTests
{
	public class ItemSlotShould
	{
		[Test]
		public void StorageStoresItem ()
		{
			var storageSlot = new ItemStorageSlot ();

			var itemToAdd = new StackableItem (new ProceduralItemTemplate (), 10);

			storageSlot.AddItem (itemToAdd);

			Assert.AreEqual (itemToAdd, storageSlot.CurrentItem);
		}
	}
}
