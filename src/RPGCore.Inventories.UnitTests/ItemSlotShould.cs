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

		[Test]
		public void StorageLimitedStackSize ()
		{
			var storageSlot = new ItemStorageSlot ()
			{
				MaxStackSize = 10
			};

			var itemToAdd = new StackableItem (new ProceduralItemTemplate (), 15);

			Assert.AreEqual (15, itemToAdd.Quantity);

			var result = storageSlot.AddItem (itemToAdd);

			Assert.AreEqual (10, result.Quantity);
			Assert.AreEqual (10, ((StackableItem)result.ItemAdded).Quantity);

			Assert.AreEqual (5, itemToAdd.Quantity);
		}
	}
}
