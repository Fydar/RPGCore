using System.Diagnostics;

namespace RPGCore.Packages
{
	public sealed class ProjectResourceDependency : IResourceDependency
	{
		private readonly ProjectExplorer projectExplorer;

		public string Key { get; }

		public bool IsValid
		{
			get
			{
				return projectExplorer.Resources.Contains(Key);
			}
		}

		public ProjectResource Resource
		{
			get
			{
				try
				{
					return projectExplorer.Resources[Key];
				}
				catch
				{
					return null;
				}
			}
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IResource IResourceDependency.Resource => Resource;

		internal ProjectResourceDependency(ProjectExplorer projectExplorer, string key)
		{
			this.projectExplorer = projectExplorer;

			Key = key;
		}
	}
}
