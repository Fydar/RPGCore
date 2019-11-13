using NUnit.Framework;
using System;

namespace RPGCore.Items.UnitTests
{
	public class StackableItemShould
	{
		[Test]
		public void TakeItemFromStack()
		{
			var sourceItem = new StackableItem (new ProceduralItemTemplate (), 12);

			var resultItem = sourceItem.Take (4);

			Assert.AreEqual (4, resultItem.Quantity);
			Assert.AreEqual (8, sourceItem.Quantity);
		}

		[Test]
		public void ThrowWhenTakeNegative()
		{
			Assert.Throws<InvalidOperationException> (new TestDelegate (() =>
			{
				var sourceItem = new StackableItem (new ProceduralItemTemplate (), 6);

				var resultItem = sourceItem.Take (-4);
			}));
		}

		[Test]
		public void ThrowWhenTakeNone()
		{
			Assert.Throws<InvalidOperationException> (new TestDelegate (() =>
			{
				var sourceItem = new StackableItem (new ProceduralItemTemplate (), 6);

				var resultItem = sourceItem.Take (0);
			}));
		}

		[Test]
		public void ThrowWhenUnableToTake()
		{
			Assert.Throws<InvalidOperationException> (new TestDelegate (() =>
			{
				var sourceItem = new StackableItem (new ProceduralItemTemplate (), 6);

				var resultItem = sourceItem.Take (8);
			}));
		}

		[Test]
		public void ThrowWhenTakeWholeStack()
		{
			Assert.Throws<InvalidOperationException> (new TestDelegate (() =>
			{
				var sourceItem = new StackableItem (new ProceduralItemTemplate (), 8);

				var resultItem = sourceItem.Take (8);
			}));
		}
	}
}
