using RPGCore.Behaviour;

namespace RPGCore.Demo.BoardGame.Models
{
	public class BuildingTemplate
	{
		public string DisplayName;

		public string[,] Recipe;

		public Graph GlobalEffectGraph;
		public Graph LocalEffectGraph;

		public string GetResourceAt(int x, int y, BuildingOrientation buildingOrientation)
		{
			int width = Recipe.GetLength(0);
			int height = Recipe.GetLength(1);

			var sample = new Integer2(x, y).SimpleTransform(buildingOrientation, width, height);

			if (sample.x < 0 || sample.x >= width
				|| sample.y < 0 || sample.y >= height)
			{
				return null;
			}

			return Recipe[sample.x, sample.y];
		}
	}
}
