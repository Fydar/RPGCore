using NUnit.Framework;
using RPGCore.Inventory.Slots;
using RPGCore.Items;

namespace RPGCore.Inventories.UnitTests
{
	public class WeightedInventoryConstraintShould
	{
		[Test, Parallelizable]
		public void LimitCapacity ()
		{
			var storageSlot = new ItemStorageSlot
			(
				new IInventoryConstraint[]
				{
					new WeightedInventoryConstraint(3)
				}
			);

			var template = new ProceduralItemTemplate ()
			{
				Weight = 1
			};

			var itemToAddA = new StackableItem (template, 2);
			var itemToAddB = new StackableItem (template, 2);

			storageSlot.AddItem (itemToAddA);
			var result = storageSlot.AddItem (itemToAddB);

			Assert.AreEqual (3, ((StackableItem)result.ItemAdded).Quantity);
		}
	}
}
