using NUnit.Framework;
using RPGCore.Demo.BoardGame.Models;

namespace RPGCore.Demo.BoardGame.UnitTests
{
	[TestFixture(TestOf = typeof(BuildingTemplate))]
	public class BuildingTemplateShould
	{
		[Test(Description = "Should be able to determine ever location a building can be built in."), Parallelizable]
		public void DeterminePatternSymmetryAndRotatable()
		{
			var pattern1x1 = new BuildingTemplate()
			{
				Recipe = new string[,]
				{
					{ "x" }
				}
			};

			var pattern2x1 = new BuildingTemplate()
			{
				Recipe = new string[,]
				{
					{ "x", "x" }
				}
			};

			var pattern2x2 = new BuildingTemplate()
			{
				Recipe = new string[,]
				{
					{ "y", "y" },
					{ "y", "y" }
				}
			};

			var pattern2x2MinusLowerLeft = new BuildingTemplate()
			{
				Recipe = new string[,]
				{
					{ "y", "y" },
					{ "y", null }
				}
			};

			var pattern2x2MinusTopRight = new BuildingTemplate()
			{
				Recipe = new string[,]
				{
					{ null, "y" },
					{ "y", "y" }
				}
			};

			var pattern3x2L = new BuildingTemplate()
			{
				Recipe = new string[,]
				{
					{ "x", "x", "x" },
					{ "x", null, null }
				}
			};

			var pattern4x2L = new BuildingTemplate()
			{
				Recipe = new string[,]
				{
					{ "x", "x", "x", "x" },
					{ "x", null, null, null }
				}
			};

			var pattern4x2T = new BuildingTemplate()
			{
				Recipe = new string[,]
				{
					{ "x", null },
					{ "x", "x" },
					{ "x", "x" },
					{ "x", null },
				}
			};

			var pattern4x2TRotated = new BuildingTemplate()
			{
				Recipe = new string[,]
				{
					{ "x", "x", "x", "x" },
					{ null, "x", "x", null }
				}
			};

			Assert.IsTrue(pattern1x1.IsVerticallySymmetric);
			Assert.IsTrue(pattern1x1.IsHorizontallySymmetric);
			Assert.IsFalse(pattern1x1.IsRotating);

			Assert.IsTrue(pattern2x1.IsVerticallySymmetric);
			Assert.IsTrue(pattern2x1.IsHorizontallySymmetric);
			Assert.IsTrue(pattern2x1.IsRotating);

			Assert.IsTrue(pattern2x2.IsVerticallySymmetric);
			Assert.IsTrue(pattern2x2.IsHorizontallySymmetric);
			Assert.IsFalse(pattern2x2.IsRotating);

			Assert.False(pattern2x2MinusLowerLeft.IsVerticallySymmetric);
			Assert.False(pattern2x2MinusLowerLeft.IsHorizontallySymmetric);
			Assert.IsTrue(pattern2x2MinusLowerLeft.IsRotating);

			Assert.False(pattern2x2MinusTopRight.IsVerticallySymmetric);
			Assert.False(pattern2x2MinusTopRight.IsHorizontallySymmetric);
			Assert.IsTrue(pattern2x2MinusLowerLeft.IsRotating);

			Assert.IsFalse(pattern3x2L.IsVerticallySymmetric);
			Assert.IsFalse(pattern3x2L.IsHorizontallySymmetric);
			Assert.IsTrue(pattern3x2L.IsRotating);

			Assert.IsFalse(pattern4x2L.IsVerticallySymmetric);
			Assert.IsFalse(pattern4x2L.IsHorizontallySymmetric);
			Assert.IsTrue(pattern4x2L.IsRotating);

			Assert.False(pattern4x2T.IsVerticallySymmetric);
			Assert.True(pattern4x2T.IsHorizontallySymmetric);
			Assert.IsTrue(pattern4x2T.IsRotating);

			Assert.IsTrue(pattern4x2TRotated.IsVerticallySymmetric);
			Assert.IsFalse(pattern4x2TRotated.IsHorizontallySymmetric);
			Assert.IsTrue(pattern4x2TRotated.IsRotating);
		}
	}
}
