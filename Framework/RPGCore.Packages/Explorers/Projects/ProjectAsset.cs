using System.Collections.Generic;
using System.IO;

namespace RPGCore.Packages
{
	public struct ProjectAsset : IAsset
	{
		public readonly DirectoryInfo Archive;
		public readonly ProjectResource[] ProjectResources;

		public string Name => Archive.Name;

		public IEnumerable<IResource> Resources => ProjectResources;

		public ProjectAsset (DirectoryInfo folder)
		{
			Archive = folder;

			var files = folder.GetFiles ("*", SearchOption.AllDirectories);
			
			var projectResources = new List<ProjectResource>();
			for (int i = 0; i < files.Length; i++)
			{
				var file = files[i];

				if (file.Extension == ".bpkg")
					continue;

				projectResources.Add(new ProjectResource (file.FullName, file));
			}

			ProjectResources = projectResources.ToArray();
		}

		public IResource GetResource (string path)
		{
			foreach (var resource in ProjectResources)
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
