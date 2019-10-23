using NUnit.Framework;
using RPGCore.Inventory.Slots;
using RPGCore.Items;

namespace RPGCore.Inventories.UnitTests
{
	public class ItemSlotSwappingShould
	{
		[Test, Parallelizable]
		public void EmptyStorageToFullStorage ()
		{
			var toItem = new StackableItem (new ProceduralItemTemplate (), 5);

			var fromSlot = new ItemStorageSlot ();
			var toSlot = new ItemStorageSlot ();

			toSlot.AddItem (toItem);

			fromSlot.SwapInto (toSlot);

			Assert.AreEqual (toItem, fromSlot.CurrentItem);
			Assert.AreEqual (null, toSlot.CurrentItem);
		}

		[Test, Parallelizable]
		public void FullStorageToEmptyStorage ()
		{
			var fromItem = new StackableItem (new ProceduralItemTemplate (), 5);

			var fromSlot = new ItemStorageSlot ();
			var toSlot = new ItemStorageSlot ();

			fromSlot.AddItem (fromItem);

			fromSlot.SwapInto (toSlot);

			Assert.AreEqual (null, fromSlot.CurrentItem);
			Assert.AreEqual (fromItem, toSlot.CurrentItem);
		}

		[Test, Parallelizable]
		public void FullStorageToFullStorage ()
		{
			var fromItem = new StackableItem (new ProceduralItemTemplate (), 5);
			var toItem = new StackableItem (new ProceduralItemTemplate (), 5);

			var fromSlot = new ItemStorageSlot ();
			var toSlot = new ItemStorageSlot ();

			fromSlot.AddItem (fromItem);
			toSlot.AddItem (toItem);

			fromSlot.SwapInto (toSlot);

			Assert.AreEqual (toItem, fromSlot.CurrentItem);
			Assert.AreEqual (fromItem, toSlot.CurrentItem);
		}

		[Test, Parallelizable]
		public void EmptyStorageToEmptyStorage ()
		{
			var fromSlot = new ItemStorageSlot ();
			var toSlot = new ItemStorageSlot ();

			fromSlot.SwapInto (toSlot);

			Assert.AreEqual (null, fromSlot.CurrentItem);
			Assert.AreEqual (null, toSlot.CurrentItem);
		}

		/*[Test, Parallelizable]
		public void EmptySelectToEmptyStorage ()
		{
			var fromSlot = new ItemSelectSlot ();
			var toSlot = new ItemStorageSlot ();

			fromSlot.SwapInto (toSlot);

			Assert.AreEqual (null, fromSlot.CurrentItem);
			Assert.AreEqual (null, toSlot.CurrentItem);
		}

		[Test, Parallelizable]
		public void EmptySelectToFullSelect ()
		{
			var toItem = new StackableItem (new ProceduralItemTemplate (), 5);

			var fromSlot = new ItemSelectSlot ();
			var toSlot = new ItemSelectSlot ();

			toSlot.AddItem (toItem);

			fromSlot.SwapInto (toSlot);

			Assert.AreEqual (toItem, fromSlot.CurrentItem);
			Assert.AreEqual (null, toSlot.CurrentItem);
		}

		[Test, Parallelizable]
		public void EmptySelectToFullStorage ()
		{
			var toItem = new StackableItem (new ProceduralItemTemplate (), 5);

			var fromSlot = new ItemSelectSlot ();
			var toSlot = new ItemStorageSlot ();

			toSlot.AddItem (toItem);

			fromSlot.SwapInto (toSlot);

			Assert.AreEqual (toItem, fromSlot.CurrentItem);
			Assert.AreEqual (toItem, toSlot.CurrentItem);
		}

		[Test, Parallelizable]
		public void EmptySelectToEmptySelect ()
		{
			var fromSlot = new ItemSelectSlot ();
			var toSlot = new ItemSelectSlot ();

			fromSlot.SwapInto (toSlot);

			Assert.AreEqual (null, fromSlot.CurrentItem);
			Assert.AreEqual (null, toSlot.CurrentItem);
		}

		[Test, Parallelizable]
		public void FullSelectToFullSelect ()
		{
			var fromItem = new StackableItem (new ProceduralItemTemplate (), 5);
			var toItem = new StackableItem (new ProceduralItemTemplate (), 5);

			var fromSlot = new ItemSelectSlot ();
			var toSlot = new ItemSelectSlot ();

			fromSlot.AddItem (fromItem);
			toSlot.AddItem (toItem);

			fromSlot.SwapInto (toSlot);

			Assert.AreEqual (fromItem, toSlot.CurrentItem);
			Assert.AreEqual (toItem, fromSlot.CurrentItem);
		}

		[Test, Parallelizable]
		public void FullSelectToEmptySelect ()
		{
			var fromItem = new StackableItem (new ProceduralItemTemplate (), 5);

			var fromSlot = new ItemSelectSlot ();
			var toSlot = new ItemSelectSlot ();

			fromSlot.AddItem (fromItem);

			fromSlot.SwapInto (toSlot);

			Assert.AreEqual (null, fromSlot.CurrentItem);
			Assert.AreEqual (fromItem, toSlot.CurrentItem);
		}

		[Test, Parallelizable]
		public void FullStorageToEmptySelect ()
		{
			var fromItem = new StackableItem (new ProceduralItemTemplate (), 5);

			var fromSlot = new ItemStorageSlot ();
			var toSlot = new ItemSelectSlot ();

			fromSlot.AddItem (fromItem);

			fromSlot.SwapInto (toSlot);

			Assert.AreEqual (fromItem, fromSlot.CurrentItem);
			Assert.AreEqual (fromItem, toSlot.CurrentItem);
		}

		[Test, Parallelizable]
		public void FullStorageToFullSelect ()
		{
			var fromItem = new StackableItem (new ProceduralItemTemplate (), 5);
			var toItem = new StackableItem (new ProceduralItemTemplate (), 5);

			var fromSlot = new ItemStorageSlot ();
			var toSlot = new ItemSelectSlot ();

			fromSlot.AddItem (fromItem);
			toSlot.AddItem (toItem);

			fromSlot.SwapInto (toSlot);

			Assert.AreEqual (fromItem, fromSlot.CurrentItem);
			Assert.AreEqual (fromItem, toSlot.CurrentItem);
		} */
	}
}
