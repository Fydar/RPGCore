using System.IO;

namespace RPGCore.Packages
{
	public struct ProjectAsset : IAsset
	{
		public readonly DirectoryInfo Archive;
		public readonly ProjectResource[] Resources;

		public string Name => Archive.Name;

		public ProjectAsset (DirectoryInfo folder)
		{
			Archive = folder;

			var files = folder.GetFiles ("*", SearchOption.AllDirectories);

			Resources = new ProjectResource[files.Length];
			for (int i = 0; i < files.Length; i++)
			{
				Resources[i] = new ProjectResource (files[i]);
			}
		}

		public IResource GetResource (string path)
		{
			foreach (var resource in Resources)
			{
				if (Archive.Name == path)
				{
					return resource;
				}
			}
			return default (ProjectResource);
		}

		public override string ToString ()
		{
			return Archive.Name;
		}
	}
}
