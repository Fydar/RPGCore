using NUnit.Framework;
using RPGCore.Demo.BoardGame.Models;
using System.Collections.Generic;
using System.Linq;

namespace RPGCore.Demo.BoardGame.UnitTests;

[TestFixture(TestOf = typeof(BuildingTemplate))]
public class BuildingTemplateShould
{
	[Test(Description = "Should be able to describe a patterns symmetry and whether it's rotatable."), Parallelizable]
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

		Assert.IsFalse(pattern2x2MinusLowerLeft.IsVerticallySymmetric);
		Assert.IsFalse(pattern2x2MinusLowerLeft.IsHorizontallySymmetric);
		// Can achieve the same pattern without using rotation
		Assert.IsFalse(pattern2x2MinusLowerLeft.IsRotating);

		Assert.IsFalse(pattern2x2MinusTopRight.IsVerticallySymmetric);
		Assert.IsFalse(pattern2x2MinusTopRight.IsHorizontallySymmetric);
		// Can achieve the same pattern without using rotation
		Assert.IsFalse(pattern2x2MinusLowerLeft.IsRotating);

		Assert.IsFalse(pattern3x2L.IsVerticallySymmetric);
		Assert.IsFalse(pattern3x2L.IsHorizontallySymmetric);
		Assert.IsTrue(pattern3x2L.IsRotating);

		Assert.IsFalse(pattern4x2L.IsVerticallySymmetric);
		Assert.IsFalse(pattern4x2L.IsHorizontallySymmetric);
		Assert.IsTrue(pattern4x2L.IsRotating);

		Assert.IsFalse(pattern4x2T.IsVerticallySymmetric);
		Assert.IsTrue(pattern4x2T.IsHorizontallySymmetric);
		Assert.IsTrue(pattern4x2T.IsRotating);

		Assert.IsTrue(pattern4x2TRotated.IsVerticallySymmetric);
		Assert.IsFalse(pattern4x2TRotated.IsHorizontallySymmetric);
		Assert.IsTrue(pattern4x2TRotated.IsRotating);
	}

	[Test(Description = "Should be able to list all meaningful orientations of a building."), Parallelizable]
	public void ListAllMeaningfulOrientations()
	{
		// 2x2 Square Building
		AssertThatSequenceEquals(new BuildingTemplate()
		{
			Recipe = new string[,]
			{
				{ "y", "y" },
				{ "y", "y" }
			}
		}.MeaningfulOrientations(), new[]
		{
			BuildingOrientation.None
		});

		// 3x3 Cross Building
		AssertThatSequenceEquals(new BuildingTemplate()
		{
			Recipe = new string[,]
			{
				{ null, "x", null },
				{ "x", "x", "x" },
				{ null, "x", null },
			}
		}.MeaningfulOrientations(), new[]
		{
			BuildingOrientation.None
		});

		// 4x2 Pyramid Building
		AssertThatSequenceEquals(new BuildingTemplate()
		{
			Recipe = new string[,]
			{
				{ "x", null },
				{ "x", "x" },
				{ "x", "x" },
				{ "x", null },
			}
		}.MeaningfulOrientations(), new[]
		{
			BuildingOrientation.None,
			BuildingOrientation.MirrorY,
			BuildingOrientation.Rotate90,
			BuildingOrientation.Rotate90MirrorX,
		});

		// 5x2 Pyramid Building
		AssertThatSequenceEquals(new BuildingTemplate()
		{
			Recipe = new string[,]
			{
				{ "x", null, null },
				{ "x", "x", null },
				{ "x", "x", "x", },
				{ "x", "x", null },
				{ "x", null, null },
			}
		}.MeaningfulOrientations(), new[]
		{
			BuildingOrientation.None,
			BuildingOrientation.MirrorY,
			BuildingOrientation.Rotate90,
			BuildingOrientation.Rotate90MirrorX,
		});

		// 2x2 Square with lower-right removed
		AssertThatSequenceEquals(new BuildingTemplate()
		{
			Recipe = new string[,]
			{
				{ "y", "y" },
				{ "y", null }
			}
		}.MeaningfulOrientations(), new[]
		{
			BuildingOrientation.None,
			BuildingOrientation.MirrorX,
			BuildingOrientation.MirrorY,
			BuildingOrientation.MirrorXandY,
		});

		// 2x2 Square with upper-left removed
		AssertThatSequenceEquals(new BuildingTemplate()
		{
			Recipe = new string[,]
			{
				{ null, "y" },
				{ "y", "y" }
			}
		}.MeaningfulOrientations(), new[]
		{
			BuildingOrientation.None,
			BuildingOrientation.MirrorX,
			BuildingOrientation.MirrorY,
			BuildingOrientation.MirrorXandY,
		});

		// 2x3 "L" shapred building
		AssertThatSequenceEquals(new BuildingTemplate()
		{
			Recipe = new string[,]
			{
				{ "x", "x", "x" },
				{ "x", null, null }
			}
		}.MeaningfulOrientations(), new[]
		{
			BuildingOrientation.None,
			BuildingOrientation.MirrorX,
			BuildingOrientation.MirrorY,
			BuildingOrientation.MirrorXandY,
			BuildingOrientation.Rotate90,
			BuildingOrientation.Rotate90MirrorX,
			BuildingOrientation.Rotate90MirrorY,
			BuildingOrientation.Rotate90MirrorXandY
		});

		// 3x2 "L" shapred building rotated
		AssertThatSequenceEquals(new BuildingTemplate()
		{
			Recipe = new string[,]
			{
				{ "x", null },
				{ "x", null },
				{ "x", "x" },
			}
		}.MeaningfulOrientations(), new[]
		{
			BuildingOrientation.None,
			BuildingOrientation.MirrorX,
			BuildingOrientation.MirrorY,
			BuildingOrientation.MirrorXandY,
			BuildingOrientation.Rotate90,
			BuildingOrientation.Rotate90MirrorX,
			BuildingOrientation.Rotate90MirrorY,
			BuildingOrientation.Rotate90MirrorXandY
		});
	}

	private static void AssertThatSequenceEquals(IEnumerable<BuildingOrientation> actual, BuildingOrientation[] expected)
	{
		var actualArray = actual.ToArray();

		bool equal;
		if (actualArray.Length != expected.Length)
		{
			equal = false;
		}
		else
		{
			equal = true;
			for (int i = 0; i < expected.Length; i++)
			{
				var expectedElement = expected[i];
				var actualElement = actualArray[i];

				if (expectedElement != actualElement)
				{
					equal = false;
					break;
				}
			}
		}

		if (!equal)
		{
			Assert.Fail($"{nameof(BuildingOrientation)} sequence differs from the expected sequence.\n  Expected: \"{string.Join("\", \"", expected)}\"\n  Actual:   \"{string.Join("\", \"", actualArray)}\"");
		}
	}
}
