using NUnit.Framework;
using RPGCore.Inventory.Slots;
using RPGCore.Items;

namespace RPGCore.Inventories.UnitTests
{
	public class StorageSlotMovingShould
	{
		[Test, Parallelizable]
		public void MoveEmptyToEmpty ()
		{
			Assert.Ignore ();

			var fromSlot = new ItemStorageSlot ();
			var toSlot = new ItemStorageSlot ();

			fromSlot.MoveInto (toSlot);

			Assert.AreEqual (null, fromSlot.CurrentItem);
			Assert.AreEqual (null, toSlot.CurrentItem);
		}

		[Test, Parallelizable]
		public void MoveEmptyToStackable ()
		{
			Assert.Ignore ();

			var toItem = new StackableItem (new ProceduralItemTemplate (), 5);

			var fromSlot = new ItemStorageSlot ();
			var toSlot = new ItemStorageSlot ();

			toSlot.AddItem (toItem);

			fromSlot.MoveInto (toSlot);

			Assert.AreEqual (toItem, fromSlot.CurrentItem);
			Assert.AreEqual (null, toSlot.CurrentItem);
		}

		[Test, Parallelizable]
		public void MoveStackableToPartial ()
		{
			Assert.Ignore ();

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

			fromSlot.MoveInto (toSlot);

			Assert.AreEqual (null, fromSlot.CurrentItem);
			Assert.AreEqual (fromItem, toSlot.CurrentItem);
		}

		[Test, Parallelizable]
		public void MoveStackableToStackable ()
		{
			Assert.Ignore ();

			var template = new ProceduralItemTemplate ();

			var fromItem = new StackableItem (template, 5);
			var toItem = new StackableItem (template, 10);

			var fromSlot = new ItemStorageSlot ();
			var toSlot = new ItemStorageSlot ();

			fromSlot.AddItem (fromItem);
			toSlot.AddItem (toItem);

			fromSlot.MoveInto (toSlot);

			Assert.AreEqual (null, fromSlot.CurrentItem);
			Assert.AreEqual (fromItem, toSlot.CurrentItem);
		}

		[Test, Parallelizable]
		public void MoveStackableToEmpty ()
		{
			Assert.Ignore ();

			var fromItem = new StackableItem (new ProceduralItemTemplate (), 5);

			var fromSlot = new ItemStorageSlot ();
			var toSlot = new ItemStorageSlot ();

			fromSlot.AddItem (fromItem);

			fromSlot.MoveInto (toSlot);

			Assert.AreEqual (null, fromSlot.CurrentItem);
			Assert.AreEqual (fromItem, toSlot.CurrentItem);
		}

		[Test, Parallelizable]
		public void MoveStackableToStackableOfDifferentType ()
		{
			Assert.Ignore ();

			var fromItem = new StackableItem (new ProceduralItemTemplate (), 5);
			var toItem = new StackableItem (new ProceduralItemTemplate (), 5);

			var fromSlot = new ItemStorageSlot ();
			var toSlot = new ItemStorageSlot ();

			fromSlot.AddItem (fromItem);
			toSlot.AddItem (toItem);

			fromSlot.MoveInto (toSlot);

			Assert.AreEqual (toItem, fromSlot.CurrentItem);
			Assert.AreEqual (fromItem, toSlot.CurrentItem);
		}
	}
}
