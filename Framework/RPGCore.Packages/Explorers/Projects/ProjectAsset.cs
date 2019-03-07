using System.IO;

namespace RPGCore.Packages
{
	public struct ProjectAsset
	{
		public readonly DirectoryInfo Archive;
		public readonly ProjectResource[] Resources;

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

		public override string ToString ()
		{
			return Archive.Name;
		}

		public ProjectResource GetResource(string path)
		{
			foreach (var Resources in Resources)
			{
				if (Archive.Name == path)
				{
					return Resources;
				}
			}
			return default(ProjectResource);
		}
	}
}
