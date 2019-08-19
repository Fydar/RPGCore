using NUnit.Framework;
using RPGCore.Inventory.Slots;
using RPGCore.Items;

namespace RPGCore.Inventories.UnitTests
{
	public class ItemStorageSlotAddingShould
	{
		[Test, Parallelizable]
		public void CompleteOnFilledFromEmpty ()
		{
			var storageSlot = new ItemStorageSlot ()
			{
				MaxStackSize = 10
			};

			var template = new ProceduralItemTemplate ();

			var itemToAddA = new StackableItem (template, 10);

			var result = storageSlot.AddItem (itemToAddA);

			Assert.AreEqual (10, result.Quantity);
			Assert.AreEqual (InventoryResult.OperationStatus.Complete, result.Status);
			Assert.AreEqual (itemToAddA, result.ItemAdded);
		}

		[Test, Parallelizable]
		public void CompleteOnFilledFromPartial ()
		{
			var storageSlot = new ItemStorageSlot ()
			{
				MaxStackSize = 10
			};

			var template = new ProceduralItemTemplate ();

			var itemToAddA = new StackableItem (template, 5);
			var itemToAddB = new StackableItem (template, 5);

			storageSlot.AddItem (itemToAddA);
			var result = storageSlot.AddItem (itemToAddB);

			Assert.AreEqual (5, result.Quantity);
			Assert.AreEqual (InventoryResult.OperationStatus.Complete, result.Status);
			Assert.AreEqual (itemToAddA, result.ItemAdded);
		}

		[Test, Parallelizable]
		public void LimitedSlotCapacityForMultipleStackableItems ()
		{
			var storageSlot = new ItemStorageSlot ()
			{
				MaxStackSize = 10
			};

			var template = new ProceduralItemTemplate ();

			var itemToAddA = new StackableItem (template, 5);
			var itemToAddB = new StackableItem (template, 10);

			storageSlot.AddItem (itemToAddA);
			var result = storageSlot.AddItem (itemToAddB);

			Assert.AreEqual (5, itemToAddB.Quantity);
			Assert.AreEqual (10, ((StackableItem)result.ItemAdded).Quantity);
		}

		[Test, Parallelizable]
		public void LimitedSlotCapacityForStackableItems ()
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

		[Test, Parallelizable]
		public void NoneOnTryAddToFullStackableSlot ()
		{
			var storageSlot = new ItemStorageSlot ()
			{
				MaxStackSize = 10
			};

			var template = new ProceduralItemTemplate ();

			var itemToAddA = new StackableItem (template, 10);
			var itemToAddB = new StackableItem (template, 15);

			storageSlot.AddItem (itemToAddA);
			var result = storageSlot.AddItem (itemToAddB);

			Assert.AreEqual (0, result.Quantity);
			Assert.AreEqual (InventoryResult.OperationStatus.None, result.Status);
			Assert.AreEqual (null, result.ItemAdded);
		}

		[Test, Parallelizable]
		public void NoneOnTryAddStackableOfDifferentTypeAddedToOccupiedSlot ()
		{
			var storageSlot = new ItemStorageSlot ();

			var oldItem = new StackableItem (new ProceduralItemTemplate (), 5);
			var newItem = new StackableItem (new ProceduralItemTemplate (), 10);

			storageSlot.AddItem (oldItem);

			var result = storageSlot.AddItem (newItem);

			Assert.AreEqual (0, result.Quantity);
			Assert.AreEqual (InventoryResult.OperationStatus.None, result.Status);
			Assert.AreEqual (null, result.ItemAdded);
		}

		[Test, Parallelizable]
		public void NoneOnTryAddUniqueItemAddedToOccupiedSlot ()
		{
			var storageSlot = new ItemStorageSlot ();

			var oldItem = new StackableItem (new ProceduralItemTemplate (), 15);
			var newItem = new UniqueItem (new ProceduralItemTemplate ());

			storageSlot.AddItem (oldItem);

			var result = storageSlot.AddItem (newItem);

			Assert.AreEqual (0, result.Quantity);
			Assert.AreEqual (InventoryResult.OperationStatus.None, result.Status);
			Assert.AreEqual (null, result.ItemAdded);
		}

		[Test, Parallelizable]
		public void StoreStackableItem ()
		{
			var storageSlot = new ItemStorageSlot ();

			var itemToAdd = new StackableItem (new ProceduralItemTemplate (), 10);

			storageSlot.AddItem (itemToAdd);

			Assert.AreEqual (itemToAdd, storageSlot.CurrentItem);
		}

		[Test, Parallelizable]
		public void StoreUniqueItem ()
		{
			var storageSlot = new ItemStorageSlot ();

			var itemToAdd = new UniqueItem (new ProceduralItemTemplate ());

			storageSlot.AddItem (itemToAdd);

			Assert.AreEqual (itemToAdd, storageSlot.CurrentItem);
		}
	}
}
