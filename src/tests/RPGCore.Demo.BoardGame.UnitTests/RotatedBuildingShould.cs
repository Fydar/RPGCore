using NUnit.Framework;
using RPGCore.Demo.BoardGame.Models;
using System;

namespace RPGCore.Demo.BoardGame.UnitTests;

[TestFixture(TestOf = typeof(RotatedBuilding))]
public class RotatedBuildingShould
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

		WriteAllOrientations(buildingTemplate);
	}

	[Test, Parallelizable]
	public void RotateLongBuildings()
	{
		var buildingTemplate = new BuildingTemplate()
		{
			Recipe = new string[,]
			{
				{ "1", "2", "3", "4", "5", "6" },
				{ "x", "-", "-", "-", "-", "-" },
			}
		};

		WriteAllOrientations(buildingTemplate);
	}

	[Test, Parallelizable]
	public void RotateTallBuildings()
	{
		var buildingTemplate = new BuildingTemplate()
		{
			Recipe = new string[,]
			{
				{ "1", "x" },
				{ "2", "-" },
				{ "3", "-" },
				{ "4", "-" },
				{ "5", "-" },
				{ "6", "-" },
			}
		};

		WriteAllOrientations(buildingTemplate);
	}

	private void WriteAllOrientations(BuildingTemplate buildingTemplate)
	{
		WriteBuilding(new RotatedBuilding(buildingTemplate, BuildingOrientation.None));
		WriteBuilding(new RotatedBuilding(buildingTemplate, BuildingOrientation.MirrorX));
		WriteBuilding(new RotatedBuilding(buildingTemplate, BuildingOrientation.MirrorY));
		WriteBuilding(new RotatedBuilding(buildingTemplate, BuildingOrientation.MirrorXandY));
		WriteBuilding(new RotatedBuilding(buildingTemplate, BuildingOrientation.Rotate90));
		WriteBuilding(new RotatedBuilding(buildingTemplate, BuildingOrientation.Rotate90MirrorX));
		WriteBuilding(new RotatedBuilding(buildingTemplate, BuildingOrientation.Rotate90MirrorY));
		WriteBuilding(new RotatedBuilding(buildingTemplate, BuildingOrientation.Rotate90MirrorXandY));
	}

	private void WriteBuilding(RotatedBuilding transformed)
	{
		Console.WriteLine($"Building {transformed.Width}x{transformed.Height} ({transformed.Orientation})");
		for (int y = transformed.Height - 1; y >= 0; y--)
		{
			Console.Write("    ");

			for (int x = 0; x < transformed.Width; x++)
			{
				string building = transformed[x, y];

				if (building == null)
				{
					Console.Write("#");
				}
				else
				{
					Console.Write(building);
				}
				Console.Write(" ");
			}
			Console.WriteLine();
		}
		Console.WriteLine();
	}
}
