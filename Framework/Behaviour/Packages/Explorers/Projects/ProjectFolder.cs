using System.IO;

namespace RPGCore.Behaviour.Packages
{
	public struct ProjectFolder
	{
		public readonly DirectoryInfo Archive;
		public readonly ProjectAsset[] Assets;

		public ProjectFolder (DirectoryInfo folder)
		{
			Archive = folder;

			var files = folder.GetFiles ("*", SearchOption.AllDirectories);

			Assets = new ProjectAsset[files.Length];
			for (int i = 0; i < files.Length; i++)
			{
				Assets[i] = new ProjectAsset (files[i]);
			}
		}
	}
}
