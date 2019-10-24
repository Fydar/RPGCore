using NUnit.Framework;
using RPGCore.Inventory.Slots;
using RPGCore.Items;

namespace RPGCore.Inventories.UnitTests
{
	public class StorageSlotDraggingShould
	{
		[Test, Parallelizable]
		public void DragEmptyToEmpty ()
		{
			var fromSlot = new ItemStorageSlot ();
			var toSlot = new ItemStorageSlot ();

			fromSlot.DragInto (toSlot);

			Assert.AreEqual (null, fromSlot.CurrentItem);
			Assert.AreEqual (null, toSlot.CurrentItem);
		}

		[Test, Parallelizable]
		public void DragEmptyToStackable ()
		{
			var toItem = new StackableItem (new ProceduralItemTemplate (), 5);

			var fromSlot = new ItemStorageSlot ();
			var toSlot = new ItemStorageSlot ();

			toSlot.AddItem (toItem);

			fromSlot.DragInto (toSlot);

			Assert.AreEqual (null, fromSlot.CurrentItem);
			Assert.AreEqual (toItem, toSlot.CurrentItem);
		}

		[Test, Parallelizable]
		public void DragStackableToPartial ()
		{
			var template = new ProceduralItemTemplate ();

			var fromItem = new StackableItem (template, 15);
			var toItem = new StackableItem (template, 10);

			var fromSlot = new ItemStorageSlot ();
			var toSlot = new ItemStorageSlot ()
			{
				MaxStackSize = 15
			};

			fromSlot.AddItem (fromItem);
			toSlot.AddItem (toItem);

			fromSlot.DragInto (toSlot);

			Assert.AreEqual (10, fromItem.Quantity);
			Assert.AreEqual (15, toItem.Quantity);
		}

		[Test, Parallelizable]
		public void DragStackableToStackable ()
		{
			var template = new ProceduralItemTemplate ();

			var fromItem = new StackableItem (template, 5);
			var toItem = new StackableItem (template, 10);

			var fromSlot = new ItemStorageSlot ();
			var toSlot = new ItemStorageSlot ();

			fromSlot.AddItem (fromItem);
			toSlot.AddItem (toItem);

			fromSlot.DragInto (toSlot);

			Assert.AreEqual (0, fromItem.Quantity);
			Assert.AreEqual (15, toItem.Quantity);
		}

		[Test, Parallelizable]
		public void DragStackableToEmpty ()
		{
			var fromItem = new StackableItem (new ProceduralItemTemplate (), 5);

			var fromSlot = new ItemStorageSlot ();
			var toSlot = new ItemStorageSlot ();

			fromSlot.AddItem (fromItem);

			fromSlot.DragInto (toSlot);

			Assert.AreEqual (null, fromSlot.CurrentItem);
			Assert.AreEqual (fromItem, toSlot.CurrentItem);
		}

		[Test, Parallelizable]
		public void DragStackableToStackableOfDifferentType ()
		{
			var fromItem = new StackableItem (new ProceduralItemTemplate (), 5);
			var toItem = new StackableItem (new ProceduralItemTemplate (), 5);

			var fromSlot = new ItemStorageSlot ();
			var toSlot = new ItemStorageSlot ();

			fromSlot.AddItem (fromItem);
			toSlot.AddItem (toItem);

			fromSlot.DragInto (toSlot);

			Assert.AreEqual (toItem, fromSlot.CurrentItem);
			Assert.AreEqual (fromItem, toSlot.CurrentItem);
		}
	}
}
