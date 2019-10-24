using NUnit.Framework;
using RPGCore.Inventory.Slots;
using RPGCore.Items;

namespace RPGCore.Inventories.UnitTests
{
	public class StorageSlotSwapShould
	{
		[Test, Parallelizable]
		public void SwapEmptyToEmpty ()
		{
			var fromSlot = new ItemStorageSlot ();
			var toSlot = new ItemStorageSlot ();

			fromSlot.Swap (toSlot);

			Assert.AreEqual (null, fromSlot.CurrentItem);
			Assert.AreEqual (null, toSlot.CurrentItem);
		}

		[Test, Parallelizable]
		public void SwapEmptyToStackable ()
		{
			var toItem = new StackableItem (new ProceduralItemTemplate (), 5);

			var fromSlot = new ItemStorageSlot ();
			var toSlot = new ItemStorageSlot ();

			toSlot.AddItem (toItem);

			fromSlot.Swap (toSlot);

			Assert.AreEqual (toItem, fromSlot.CurrentItem);
			Assert.AreEqual (null, toSlot.CurrentItem);
		}

		[Test, Parallelizable]
		public void SwapStackableToPartial ()
		{
			var template = new ProceduralItemTemplate ();

			var fromItem = new StackableItem (template, 5);
			var toItem = new StackableItem (template, 10);

			var fromSlot = new ItemStorageSlot ();
			var toSlot = new ItemStorageSlot ()
			{
				MaxStackSize = 15
			};

			fromSlot.AddItem (fromItem);
			toSlot.AddItem (toItem);

			fromSlot.Swap (toSlot);

			Assert.AreEqual (toItem, fromSlot.CurrentItem);
			Assert.AreEqual (fromItem, toSlot.CurrentItem);
		}

		[Test, Parallelizable]
		public void SwapStackableToStackable ()
		{
			var template = new ProceduralItemTemplate ();

			var fromItem = new StackableItem (template, 5);
			var toItem = new StackableItem (template, 10);

			var fromSlot = new ItemStorageSlot ();
			var toSlot = new ItemStorageSlot ();

			fromSlot.AddItem (fromItem);
			toSlot.AddItem (toItem);

			fromSlot.Swap (toSlot);

			Assert.AreEqual (toItem, fromSlot.CurrentItem);
			Assert.AreEqual (fromItem, toSlot.CurrentItem);
		}

		[Test, Parallelizable]
		public void SwapStackableToEmpty ()
		{
			var fromItem = new StackableItem (new ProceduralItemTemplate (), 5);

			var fromSlot = new ItemStorageSlot ();
			var toSlot = new ItemStorageSlot ();

			fromSlot.AddItem (fromItem);

			fromSlot.Swap (toSlot);

			Assert.AreEqual (null, fromSlot.CurrentItem);
			Assert.AreEqual (fromItem, toSlot.CurrentItem);
		}

		[Test, Parallelizable]
		public void SwapStackableToStackableOfDifferentType ()
		{
			var fromItem = new StackableItem (new ProceduralItemTemplate (), 5);
			var toItem = new StackableItem (new ProceduralItemTemplate (), 5);

			var fromSlot = new ItemStorageSlot ();
			var toSlot = new ItemStorageSlot ();

			fromSlot.AddItem (fromItem);
			toSlot.AddItem (toItem);

			fromSlot.Swap (toSlot);

			Assert.AreEqual (toItem, fromSlot.CurrentItem);
			Assert.AreEqual (fromItem, toSlot.CurrentItem);
		}
	}
}
