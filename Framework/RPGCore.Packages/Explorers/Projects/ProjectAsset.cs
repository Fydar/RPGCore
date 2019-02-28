using System.IO;

namespace RPGCore.Packages
{
	public struct ProjectAsset
	{
		public readonly DirectoryInfo Archive;
		public readonly ProjectResource[] Assets;

		public ProjectAsset (DirectoryInfo folder)
		{
			Archive = folder;

			var files = folder.GetFiles ("*", SearchOption.AllDirectories);

			Assets = new ProjectResource[files.Length];
			for (int i = 0; i < files.Length; i++)
			{
				Assets[i] = new ProjectResource (files[i]);
			}
		}
	}
}
