using Newtonsoft.Json;
using RPGCore.Behaviour;
using RPGCore.Behaviour.Manifest;
using System.Collections.Generic;

namespace RPGCore.Demo.BoardGame.Models
{
	[EditorType]
	public class BuildingTemplate : IResourceModel
	{
		[JsonIgnore]
		public string Identifier { get; set; }

		public string DisplayName { get; set; }
		public string BodyText { get; set; }
		public string PackIdentifier { get; set; }

		public string[,] Recipe { get; set; }

		public SerializedGraph LocalEffectGraph { get; set; }
		public SerializedGraph BoardEffectGraph { get; set; }
		public SerializedGraph GlobalEffectGraph { get; set; }

		[JsonIgnore]
		public int Width => Recipe?.GetLength(0) ?? 0;

		[JsonIgnore]
		public int Height => Recipe?.GetLength(1) ?? 0;

		[JsonIgnore]
		public bool IsHorizontallySymmetric
		{
			get
			{
				int width = Width;

				if (width == 1)
				{
					return true;
				}

				int height = Height;

				for (int y = 0; y < height; y++)
				{
					for (int x = 0; x < width - x - 1; x++)
					{
						if (Recipe[x, y] != Recipe[width - x - 1, y])
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		[JsonIgnore]
		public bool IsVerticallySymmetric
		{
			get
			{
				int height = Height;

				if (height == 1)
				{
					return true;
				}

				int width = Width;

				for (int x = 0; x < width; x++)
				{
					for (int y = 0; y < height - y - 1; y++)
					{
						if (Recipe[x, y] != Recipe[x, height - y - 1])
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		[JsonIgnore]
		public bool IsRotating
		{
			get
			{
				if (IsTheSameWhenRotated)
				{
					return false;
				}
				if (IsRotatedSameAsMirror)
				{
					return false;
				}
				return true;
			}
		}

		[JsonIgnore]
		private bool IsTheSameWhenRotated
		{
			get
			{
				int height = Height;
				int width = Width;

				if (width != height)
				{
					return false;
				}

				for (int x = 0; x < width; x++)
				{
					for (int y = 0; y < height; y++)
					{
						int rotatedY = height - x - 1;
						int rotatedX = y;

						if (Recipe[x, y] != Recipe[rotatedX, rotatedY])
						{

							return false;
						}
					}
				}

				return true;
			}
		}

		[JsonIgnore]
		private bool IsRotatedSameAsMirror
		{
			get
			{
				int height = Height;
				int width = Width;

				if (width != height)
				{
					return false;
				}

				for (int x = 0; x < width; x++)
				{
					for (int y = 0; y < height; y++)
					{
						int rotatedY = height - x - 1;
						int rotatedX = y;

						if (Recipe[width - x - 1, y] != Recipe[rotatedX, rotatedY])
						{
							return false;
						}
					}
				}

				return true;
			}
		}

		public IEnumerable<BuildingOrientation> MeaningfulOrientations()
		{
			bool horizontally = IsHorizontallySymmetric;
			bool vertically = IsVerticallySymmetric;
			bool isRotating = IsRotating;

			yield return BuildingOrientation.None;

			if (!horizontally)
			{
				yield return BuildingOrientation.MirrorX;
			}
			if (!vertically)
			{
				yield return BuildingOrientation.MirrorY;
			}
			if (!horizontally && !vertically)
			{
				yield return BuildingOrientation.MirrorXandY;
			}
			if (isRotating)
			{
				yield return BuildingOrientation.Rotate90;

				if (!vertically)
				{
					yield return BuildingOrientation.Rotate90MirrorX;
				}
				if (!horizontally)
				{
					yield return BuildingOrientation.Rotate90MirrorY;

					if (!vertically)
					{
						yield return BuildingOrientation.Rotate90MirrorXandY;
					}
				}
			}
		}

		public override string ToString()
		{
			return $"{nameof(BuildingTemplate)}({DisplayName})";
		}
	}
}
