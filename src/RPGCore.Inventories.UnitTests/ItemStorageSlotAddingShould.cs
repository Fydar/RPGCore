using NUnit.Framework;
using RPGCore.Inventory.Slots;
using RPGCore.Items;

namespace RPGCore.Inventories.UnitTests
{
	public class ItemStorageSlotAddingShould
	{
		[Test, Parallelizable]
		public void CompleteOnFilledFromEmpty()
		{
			var storageSlot = new ItemStorageSlot ()
			{
				MaxStackSize = 10
			};

			var template = new ProceduralItemTemplate ();

			var itemToAddA = new StackableItem (template, 10);

			var result = storageSlot.AddItem (itemToAddA);

			Assert.That (result.Status, Is.EqualTo(TransactionStatus.Complete));
			Assert.That (result.Items, Has.Count.EqualTo (1));
			Assert.That (result.Items[0], Is.EqualTo (new ItemTransaction ()
			{
				Quantity = 10, 
				Item = itemToAddA,
				FromInventory = null,
				ToInventory = storageSlot
			}));
		}

		[Test, Parallelizable]
		public void CompleteOnFilledFromPartial()
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

			Assert.That (result.Status, Is.EqualTo (TransactionStatus.Complete));
			Assert.That (result.Items, Has.Count.EqualTo (1));
			Assert.That (result.Items[0], Is.EqualTo (new ItemTransaction ()
			{
				Quantity = 5,
				Item = itemToAddA,
				FromInventory = null,
				ToInventory = storageSlot
			}));
		}

		[Test, Parallelizable]
		public void LimitedSlotCapacityForMultipleStackableItems()
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

			Assert.That (result.Status, Is.EqualTo (TransactionStatus.Partial));
			Assert.That (result.Items, Has.Count.EqualTo (1));
			Assert.That (result.Items[0], Is.EqualTo (new ItemTransaction ()
			{
				Quantity = 5,
				Item = itemToAddA,
				FromInventory = null,
				ToInventory = storageSlot
			}));

			Assert.That (itemToAddB.Quantity, Is.EqualTo(5));
			Assert.That (((StackableItem)result.Items[0].Item).Quantity, Is.EqualTo(10));
		}

		[Test, Parallelizable]
		public void LimitedSlotCapacityForSingleStackableItem()
		{
			var storageSlot = new ItemStorageSlot ()
			{
				MaxStackSize = 10
			};

			var itemToAdd = new StackableItem (new ProceduralItemTemplate (), 15);

			Assert.AreEqual (15, itemToAdd.Quantity);

			var result = storageSlot.AddItem (itemToAdd);

			Assert.That (result.Status, Is.EqualTo (TransactionStatus.Partial));
			Assert.That (result.Items, Has.Count.EqualTo (1));
			Assert.That (result.Items[0], Is.EqualTo (new ItemTransaction ()
			{
				Quantity = 10,
				Item = storageSlot.CurrentItem,
				FromInventory = null,
				ToInventory = storageSlot
			}));

			Assert.That (itemToAdd.Quantity, Is.EqualTo (5));
			Assert.That (((StackableItem)result.Items[0].Item).Quantity, Is.EqualTo (10));
		}

		[Test, Parallelizable]
		public void NoneOnTryAddStackableOfDifferentTypeAddedToOccupiedSlot()
		{
			var storageSlot = new ItemStorageSlot ();

			var oldItem = new StackableItem (new ProceduralItemTemplate (), 5);
			var newItem = new StackableItem (new ProceduralItemTemplate (), 10);

			storageSlot.AddItem (oldItem);

			var result = storageSlot.AddItem (newItem);

			Assert.That (result, Is.EqualTo (InventoryTransaction.None));
		}

		[Test, Parallelizable]
		public void NoneOnTryAddToFullStackableSlot()
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

			Assert.That (result, Is.EqualTo (InventoryTransaction.None));
		}

		[Test, Parallelizable]
		public void NoneOnTryAddUniqueItemAddedToOccupiedSlot()
		{
			var storageSlot = new ItemStorageSlot ();

			var oldItem = new StackableItem (new ProceduralItemTemplate (), 15);
			var newItem = new UniqueItem (new ProceduralItemTemplate ());

			storageSlot.AddItem (oldItem);

			var result = storageSlot.AddItem (newItem);

			Assert.That (result, Is.EqualTo (InventoryTransaction.None));
		}

		[Test, Parallelizable]
		public void StoreStackableItem()
		{
			var storageSlot = new ItemStorageSlot ();

			var itemToAdd = new StackableItem (new ProceduralItemTemplate (), 10);

			var result = storageSlot.AddItem (itemToAdd);

			Assert.That (result.Status, Is.EqualTo (TransactionStatus.Complete));
			Assert.That (result.Items, Has.Count.EqualTo (1));
			Assert.That (result.Items[0], Is.EqualTo (new ItemTransaction ()
			{
				Quantity = 10,
				Item = itemToAdd,
				FromInventory = null,
				ToInventory = storageSlot
			}));

			Assert.AreEqual (TransactionStatus.Complete, result.Status);
			Assert.AreEqual (itemToAdd, storageSlot.CurrentItem);

			var firstTransaction = result.Items[0];
			Assert.AreEqual (null, firstTransaction.FromInventory);
			Assert.AreEqual (storageSlot, firstTransaction.ToInventory);
		}

		[Test, Parallelizable]
		public void StoreUniqueItem()
		{
			var storageSlot = new ItemStorageSlot ();

			var itemToAdd = new UniqueItem (new ProceduralItemTemplate ());

			var result = storageSlot.AddItem (itemToAdd);

			var firstTransaction = result.Items[0];

			Assert.That (result.Status, Is.EqualTo (TransactionStatus.Complete));
			Assert.That (result.Items, Has.Count.EqualTo (1));
			Assert.That (result.Items[0], Is.EqualTo (new ItemTransaction ()
			{
				Quantity = 1,
				Item = itemToAdd,
				FromInventory = null,
				ToInventory = storageSlot
			}));

			Assert.AreEqual (TransactionStatus.Complete, result.Status);
			Assert.AreEqual (itemToAdd, storageSlot.CurrentItem);

			Assert.AreEqual (null, firstTransaction.FromInventory);
			Assert.AreEqual (storageSlot, firstTransaction.ToInventory);
		}
	}
}
