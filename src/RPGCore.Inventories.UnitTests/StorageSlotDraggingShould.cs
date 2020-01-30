using NUnit.Framework;
using RPGCore.Inventory.Slots;
using RPGCore.Items;

namespace RPGCore.Inventories.UnitTests
{
	public class StorageSlotDraggingShould
	{
		[Test, Parallelizable]
		public void DragEmptyToEmpty()
		{
			var fromSlot = new ItemStorageSlot();
			var toSlot = new ItemStorageSlot();

			var result = fromSlot.DragInto(toSlot);

			Assert.That(result, Is.EqualTo(InventoryTransaction.None));
			Assert.That(fromSlot.CurrentItem, Is.Null);
			Assert.That(toSlot.CurrentItem, Is.Null);
		}

		[Test, Parallelizable]
		public void DragEmptyToStackable()
		{
			var toItem = new StackableItem(new ProceduralItemTemplate(), 5);

			var fromSlot = new ItemStorageSlot();
			var toSlot = new ItemStorageSlot();

			toSlot.AddItem(toItem);

			var result = fromSlot.DragInto(toSlot);

			Assert.That(result, Is.EqualTo(InventoryTransaction.None));
			Assert.That(fromSlot.CurrentItem, Is.Null);
			Assert.That(toSlot.CurrentItem, Is.EqualTo(toItem));
		}

		[Test, Parallelizable]
		public void DragEmptyToUnique()
		{
			var toItem = new UniqueItem(new ProceduralItemTemplate());

			var fromSlot = new ItemStorageSlot();
			var toSlot = new ItemStorageSlot();

			toSlot.AddItem(toItem);

			var result = fromSlot.DragInto(toSlot);

			Assert.That(result, Is.EqualTo(InventoryTransaction.None));
			Assert.That(fromSlot.CurrentItem, Is.Null);
			Assert.That(toSlot.CurrentItem, Is.EqualTo(toItem));
		}

		[Test, Parallelizable]
		public void DragStackableToEmpty()
		{
			var fromItem = new StackableItem(new ProceduralItemTemplate(), 5);

			var fromSlot = new ItemStorageSlot();
			var toSlot = new ItemStorageSlot();

			fromSlot.AddItem(fromItem);

			var result = fromSlot.DragInto(toSlot);

			var expected = new InventoryTransactionBuilder()
			{
				new ItemTransaction()
				{
					FromInventory = null,
					ToInventory = toSlot,
					Item = fromItem,
					Quantity = 5
				}
			}.Build(TransactionStatus.Complete);

			Assert.That(result, Is.EqualTo(expected));
			Assert.That(fromSlot.CurrentItem, Is.Null);
			Assert.That(toSlot.CurrentItem, Is.EqualTo(fromItem));
		}

		[Test, Parallelizable]
		public void DragStackableToStackable()
		{
			var template = new ProceduralItemTemplate();

			var fromItem = new StackableItem(template, 5);
			var toItem = new StackableItem(template, 10);

			var fromSlot = new ItemStorageSlot();
			var toSlot = new ItemStorageSlot();

			fromSlot.AddItem(fromItem);
			toSlot.AddItem(toItem);

			var result = fromSlot.DragInto(toSlot);

			var expected = new InventoryTransactionBuilder()
			{
				new ItemTransaction()
				{
					FromInventory = fromSlot,
					ToInventory = toSlot,
					Item = fromItem,
					Quantity = 15
				}
			}.Build(TransactionStatus.Complete);

			// Assert.That (result, Is.EqualTo (expected));
			Assert.That(fromItem.Quantity, Is.EqualTo(0));
			Assert.That(toItem.Quantity, Is.EqualTo(15));
		}

		[Test, Parallelizable]
		public void DragStackableToStackableOfDifferentType()
		{
			var fromItem = new StackableItem(new ProceduralItemTemplate(), 5);
			var toItem = new StackableItem(new ProceduralItemTemplate(), 5);

			var fromSlot = new ItemStorageSlot();
			var toSlot = new ItemStorageSlot();

			fromSlot.AddItem(fromItem);
			toSlot.AddItem(toItem);

			fromSlot.DragInto(toSlot);

			Assert.AreEqual(toItem, fromSlot.CurrentItem);
			Assert.AreEqual(fromItem, toSlot.CurrentItem);
		}

		[Test, Parallelizable]
		public void DragStackableToStackableOverflow()
		{
			var template = new ProceduralItemTemplate();

			var fromItem = new StackableItem(template, 15);
			var toItem = new StackableItem(template, 10);

			var fromSlot = new ItemStorageSlot();
			var toSlot = new ItemStorageSlot()
			{
				MaxStackSize = 15
			};

			fromSlot.AddItem(fromItem);
			toSlot.AddItem(toItem);

			fromSlot.DragInto(toSlot);

			Assert.AreEqual(10, fromItem.Quantity);
			Assert.AreEqual(15, toItem.Quantity);
		}

		[Test, Parallelizable]
		public void DragUniqueToEmpty()
		{
			var fromItem = new UniqueItem(new ProceduralItemTemplate());

			var fromSlot = new ItemStorageSlot();
			var toSlot = new ItemStorageSlot();

			fromSlot.AddItem(fromItem);

			fromSlot.DragInto(toSlot);

			Assert.AreEqual(null, fromSlot.CurrentItem);
			Assert.AreEqual(fromItem, toSlot.CurrentItem);
		}

		[Test, Parallelizable]
		public void DragUniqueToStackable()
		{
			var fromItem = new UniqueItem(new ProceduralItemTemplate());
			var toItem = new StackableItem(new ProceduralItemTemplate(), 5);

			var fromSlot = new ItemStorageSlot();
			var toSlot = new ItemStorageSlot();

			fromSlot.AddItem(fromItem);
			toSlot.AddItem(toItem);

			fromSlot.DragInto(toSlot);

			Assert.AreEqual(toItem, fromSlot.CurrentItem);
			Assert.AreEqual(fromItem, toSlot.CurrentItem);
		}

		[Test, Parallelizable]
		public void DragUniqueToUnique()
		{
			var fromItem = new UniqueItem(new ProceduralItemTemplate());
			var toItem = new UniqueItem(new ProceduralItemTemplate());

			var fromSlot = new ItemStorageSlot();
			var toSlot = new ItemStorageSlot();

			fromSlot.AddItem(fromItem);
			toSlot.AddItem(toItem);

			fromSlot.DragInto(toSlot);

			Assert.AreEqual(toItem, fromSlot.CurrentItem);
			Assert.AreEqual(fromItem, toSlot.CurrentItem);
		}

		[Test, Parallelizable]
		public void DragUniqueToUniqueOfDifferentType()
		{
			var fromItem = new UniqueItem(new ProceduralItemTemplate());
			var toItem = new UniqueItem(new ProceduralItemTemplate());

			var fromSlot = new ItemStorageSlot();
			var toSlot = new ItemStorageSlot();

			fromSlot.AddItem(fromItem);
			toSlot.AddItem(toItem);

			fromSlot.DragInto(toSlot);

			Assert.AreEqual(toItem, fromSlot.CurrentItem);
			Assert.AreEqual(fromItem, toSlot.CurrentItem);
		}
	}
}
