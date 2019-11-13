using System;

namespace RPGCore.Packages
{
	public class BuildConsoleRenderer : IResourceBuildStep
	{
		public int ProgressBarLeft { get; private set; }
		public int ProgressBarTop { get; private set; }
		public int ProgressBarLength { get; private set; }
		public int ProgressBarFill { get; private set; }

		private bool Animate = true;

		public void DrawProgressBar(int length)
		{
			ProgressBarLength = length;
			try
			{
				ProgressBarLeft = Console.CursorLeft;
				ProgressBarTop = Console.CursorTop;
			}
			catch
			{
				Animate = false;
			}
			DrawBar (ProgressBarLeft, ProgressBarTop, 0, ProgressBarLength);
		}

		public void OnAfterBuildResource(ProjectBuildProcess process, ProjectResource resource)
		{
			DrawBar (ProgressBarLeft, ProgressBarTop, process.Progress, ProgressBarLength, process.Project.Name);
		}

		private void DrawBar(int left, int top, double complete, int total, string suffix = null)
		{
			int filled = (int)(complete * total);

			if (filled != ProgressBarFill)
			{
				lock (Console.Out)
				{
					if (Animate)
					{
						Console.SetCursorPosition (left, top);
					}

					Console.Write ("Download ");
					Console.Write (complete.ToString ("00%"));

					Console.Write (" [");
					int j = 0;
					for (; j < filled; j++)
					{
						Console.Write ("=");
					}
					for (; j < total; j++)
					{
						Console.Write (" ");
					}
					Console.Write ("] ");
					ProgressBarFill = filled;

					Console.WriteLine (suffix);
				}
			}
		}
	}
}
