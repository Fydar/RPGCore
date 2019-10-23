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

			var firstTransaction = result.Items[0];

			Assert.AreEqual (TransactionStatus.Complete, result.Status);
			Assert.AreEqual (10, firstTransaction.Quantity);
			Assert.AreEqual (itemToAddA, firstTransaction.Item);
			Assert.AreEqual (null, firstTransaction.FromInventory);
			Assert.AreEqual (storageSlot, firstTransaction.ToInventory);
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

			var firstTransaction = result.Items[0];

			Assert.AreEqual (TransactionStatus.Complete, result.Status);
			Assert.AreEqual (5, firstTransaction.Quantity);
			Assert.AreEqual (itemToAddA, firstTransaction.Item);
			Assert.AreEqual (null, firstTransaction.FromInventory);
			Assert.AreEqual (storageSlot, firstTransaction.ToInventory);
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

			var firstTransaction = result.Items[0];

			Assert.AreEqual (5, itemToAddB.Quantity);
			Assert.AreEqual (10, ((StackableItem)firstTransaction.Item).Quantity);
			Assert.AreEqual (null, firstTransaction.FromInventory);
			Assert.AreEqual (storageSlot, firstTransaction.ToInventory);
		}

		[Test, Parallelizable]
		public void LimitedSlotCapacityForSingleStackableItem ()
		{
			var storageSlot = new ItemStorageSlot ()
			{
				MaxStackSize = 10
			};

			var itemToAdd = new StackableItem (new ProceduralItemTemplate (), 15);

			Assert.AreEqual (15, itemToAdd.Quantity);

			var result = storageSlot.AddItem (itemToAdd);

			var firstTransaction = result.Items[0];

			Assert.AreEqual (10, firstTransaction.Quantity);
			Assert.AreEqual (10, ((StackableItem)firstTransaction.Item).Quantity);
			Assert.AreEqual (null, firstTransaction.FromInventory);
			Assert.AreEqual (storageSlot, firstTransaction.ToInventory);

			Assert.AreEqual (5, itemToAdd.Quantity);
		}

		[Test, Parallelizable]
		public void NoneOnTryAddStackableOfDifferentTypeAddedToOccupiedSlot ()
		{
			var storageSlot = new ItemStorageSlot ();

			var oldItem = new StackableItem (new ProceduralItemTemplate (), 5);
			var newItem = new StackableItem (new ProceduralItemTemplate (), 10);

			storageSlot.AddItem (oldItem);

			var result = storageSlot.AddItem (newItem);

			Assert.AreEqual (TransactionStatus.None, result.Status);
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

			Assert.AreEqual (TransactionStatus.None, result.Status);
		}

		[Test, Parallelizable]
		public void NoneOnTryAddUniqueItemAddedToOccupiedSlot ()
		{
			var storageSlot = new ItemStorageSlot ();

			var oldItem = new StackableItem (new ProceduralItemTemplate (), 15);
			var newItem = new UniqueItem (new ProceduralItemTemplate ());

			storageSlot.AddItem (oldItem);

			var result = storageSlot.AddItem (newItem);

			Assert.AreEqual (TransactionStatus.None, result.Status);
		}

		[Test, Parallelizable]
		public void StoreStackableItem ()
		{
			var storageSlot = new ItemStorageSlot ();

			var itemToAdd = new StackableItem (new ProceduralItemTemplate (), 10);

			var result = storageSlot.AddItem (itemToAdd);

			var firstTransaction = result.Items[0];

			Assert.AreEqual (TransactionStatus.Complete, result.Status);
			Assert.AreEqual (itemToAdd, storageSlot.CurrentItem);

			Assert.AreEqual (null, firstTransaction.FromInventory);
			Assert.AreEqual (storageSlot, firstTransaction.ToInventory);
		}

		[Test, Parallelizable]
		public void StoreUniqueItem ()
		{
			var storageSlot = new ItemStorageSlot ();

			var itemToAdd = new UniqueItem (new ProceduralItemTemplate ());

			var result = storageSlot.AddItem (itemToAdd);

			var firstTransaction = result.Items[0];

			Assert.AreEqual (TransactionStatus.Complete, result.Status);
			Assert.AreEqual (itemToAdd, storageSlot.CurrentItem);

			Assert.AreEqual (null, firstTransaction.FromInventory);
			Assert.AreEqual (storageSlot, firstTransaction.ToInventory);
		}
	}
}
