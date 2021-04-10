using NUnit.Framework;
using RPGCore.Inventory.Slots;
using RPGCore.Items;

namespace RPGCore.Inventories.UnitTests
{
	[TestFixture(TestOf = typeof(ItemStorageSlot))]
	public class ItemStorageSlotMovingShould
	{
		[Test, Parallelizable]
		public void MoveEmptyToEmptySlot()
		{
			var fromSlot = new ItemStorageSlot();
			var toSlot = new ItemStorageSlot();

			fromSlot.MoveInto(toSlot);

			Assert.AreEqual(null, fromSlot.CurrentItem);
			Assert.AreEqual(null, toSlot.CurrentItem);
		}

		[Test, Parallelizable]
		public void MoveEmptyToStackableSlot()
		{
			var toItem = new StackableItem(new ProceduralItemTemplate(), 5);

			var fromSlot = new ItemStorageSlot();
			var toSlot = new ItemStorageSlot();

			toSlot.AddItem(toItem);

			fromSlot.MoveInto(toSlot);

			Assert.AreEqual(null, fromSlot.CurrentItem);
			Assert.AreEqual(toItem, toSlot.CurrentItem);
		}

		[Test, Parallelizable]
		public void MoveEmptyToUniqueSlot()
		{
			var toItem = new UniqueItem(new ProceduralItemTemplate());

			var fromSlot = new ItemStorageSlot();
			var toSlot = new ItemStorageSlot();

			toSlot.AddItem(toItem);

			fromSlot.MoveInto(toSlot);

			Assert.AreEqual(null, fromSlot.CurrentItem);
			Assert.AreEqual(toItem, toSlot.CurrentItem);
		}

		[Test, Parallelizable]
		public void MoveStackableToStackableSlot()
		{
			var template = new ProceduralItemTemplate();

			var fromItem = new StackableItem(template, 5);
			var toItem = new StackableItem(template, 10);

			var fromSlot = new ItemStorageSlot();
			var toSlot = new ItemStorageSlot();

			fromSlot.AddItem(fromItem);
			toSlot.AddItem(toItem);

			fromSlot.MoveInto(toSlot);

			Assert.AreEqual(null, fromSlot.CurrentItem);
			Assert.AreEqual(toItem, toSlot.CurrentItem);
		}

		[Test, Parallelizable]
		public void MoveStackableToStackableSlotOverflow()
		{
			var template = new ProceduralItemTemplate();

			var fromItem = new StackableItem(template, 10);
			var toItem = new StackableItem(template, 5);

			var fromSlot = new ItemStorageSlot();
			var toSlot = new ItemStorageSlot()
			{
				MaxStackSize = 10
			};

			fromSlot.AddItem(fromItem);
			toSlot.AddItem(toItem);

			var result = fromSlot.MoveInto(toSlot);

			Assert.AreEqual(TransactionStatus.Partial, result.Status);
			Assert.AreEqual(fromItem, fromSlot.CurrentItem);
			Assert.AreEqual(toItem, toSlot.CurrentItem);
		}

		[Test, Parallelizable]
		public void MoveStackableToEmptySlot()
		{
			var fromItem = new StackableItem(new ProceduralItemTemplate(), 5);

			var fromSlot = new ItemStorageSlot();
			var toSlot = new ItemStorageSlot();

			fromSlot.AddItem(fromItem);

			fromSlot.MoveInto(toSlot);

			Assert.AreEqual(null, fromSlot.CurrentItem);
			Assert.AreEqual(fromItem, toSlot.CurrentItem);
		}

		[Test, Parallelizable]
		public void MoveStackableToStackableSlotOfDifferentType()
		{
			var fromItem = new StackableItem(new ProceduralItemTemplate(), 5);
			var toItem = new StackableItem(new ProceduralItemTemplate(), 5);

			var fromSlot = new ItemStorageSlot();
			var toSlot = new ItemStorageSlot();

			fromSlot.AddItem(fromItem);
			toSlot.AddItem(toItem);

			fromSlot.MoveInto(toSlot);

			Assert.AreEqual(fromItem, fromSlot.CurrentItem);
			Assert.AreEqual(toItem, toSlot.CurrentItem);
		}
	}
}
