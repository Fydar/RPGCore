using RPGCore.Demo.BoardGame.Models;

namespace RPGCore.Demo.BoardGame
{
	public struct RotatedBuilding
	{
		public BuildingOrientation Orientation { get; }
		public BuildingTemplate Template { get; }

		public int Width => Orientation.HasFlag(BuildingOrientation.Rotate90)
					? Template.Height
					: Template.Width;

		public int Height => Orientation.HasFlag(BuildingOrientation.Rotate90)
					? Template.Width
					: Template.Height;

		public bool IsHorizontallySymmetric => Orientation.HasFlag(BuildingOrientation.Rotate90)
					? Template.IsVerticallySymmetric
					: Template.IsHorizontallySymmetric;

		public bool IsVerticallySymmetric => Orientation.HasFlag(BuildingOrientation.Rotate90)
					? Template.IsHorizontallySymmetric
					: Template.IsVerticallySymmetric;

		public bool IsRotating => Template.IsRotating;

		public string this[int x, int y]
		{
			get
			{
				int width = Template.Width;
				int height = Template.Height;

				var sample = SimpleTransform(x, y, Orientation, width, height);

				if (sample.x < 0 || sample.x >= width
					|| sample.y < 0 || sample.y >= height)
				{
					return null;
				}

				return Template.Recipe[sample.x, sample.y];
			}
		}

		public RotatedBuilding(BuildingTemplate template, BuildingOrientation orientation)
		{
			Template = template;
			Orientation = orientation;
		}

		private static Integer2 SimpleTransform(int x, int y, BuildingOrientation orientation, int width, int height)
		{
			int sampleX = x;
			int sampleY = y;

			if (orientation.HasFlag(BuildingOrientation.Rotate90))
			{
				int temp = sampleY;
				sampleY = height - sampleX - 1;
				sampleX = temp;

				if (orientation.HasFlag(BuildingOrientation.MirrorX))
				{
					sampleY = height - sampleY - 1;
				}
				if (orientation.HasFlag(BuildingOrientation.MirrorY))
				{
					sampleX = width - sampleX - 1;
				}
			}
			else
			{
				if (orientation.HasFlag(BuildingOrientation.MirrorX))
				{
					sampleX = width - sampleX - 1;
				}
				if (orientation.HasFlag(BuildingOrientation.MirrorY))
				{
					sampleY = height - sampleY - 1;
				}
			}

			return new Integer2(sampleX, sampleY);
		}
	}
}
