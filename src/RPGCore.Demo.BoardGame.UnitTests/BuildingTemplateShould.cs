using NUnit.Framework;
using RPGCore.Demo.BoardGame.Models;
using System;

namespace RPGCore.Demo.BoardGame.UnitTests
{
	[TestFixture(TestOf = typeof(BuildingTemplate))]
	public class BuildingTemplateShould
	{
		[Test, Parallelizable]
		public void RotateBuildings()
		{
			var buildingTemplate = new BuildingTemplate()
			{
				Recipe = new string[,]
				{
					{ "x", "x", "x" },
					{ "x", "-", "-" },
				}
			};

			Write2dArray(AllResourcesRotated(buildingTemplate, BuildingOrientation.None));
			Write2dArray(AllResourcesRotated(buildingTemplate, BuildingOrientation.MirrorX));
			Write2dArray(AllResourcesRotated(buildingTemplate, BuildingOrientation.MirrorY));
			Write2dArray(AllResourcesRotated(buildingTemplate, BuildingOrientation.MirrorXandY));

			Write2dArray(AllResourcesRotated(buildingTemplate, BuildingOrientation.Rotate90));
			Write2dArray(AllResourcesRotated(buildingTemplate, BuildingOrientation.Rotate90MirrorX));
			Write2dArray(AllResourcesRotated(buildingTemplate, BuildingOrientation.Rotate90MirrorY));
			Write2dArray(AllResourcesRotated(buildingTemplate, BuildingOrientation.Rotate90MirrorXandY));
		}

		private string[,] AllResourcesRotated(BuildingTemplate buildingTemplate, BuildingOrientation orientation)
		{
			string[,] recipe = new string[4, 4];

			for (int x = 0; x < 4; x++)
			{
				for (int y = 0; y < 4; y++)
				{
					recipe[x, y] = buildingTemplate.GetResourceAt(x, y, orientation);
				}
			}

			return recipe;
		}

		private void Write2dArray(string[,] recipe)
		{
			for (int x = 0; x < 4; x++)
			{
				for (int y = 0; y < 4; y++)
				{
					Console.Write(recipe[x, y]);
				}
				Console.WriteLine();
			}
		}
	}
}
