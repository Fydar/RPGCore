using RPGCore.Packages;
using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.Projects
{
	public sealed class ProjectResourceDependency : IResourceDependency
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly ProjectExplorer projectExplorer;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal readonly Dictionary<string, string> metdadata;
		public IReadOnlyDictionary<string, string> Metadata => metdadata;

		public string Key { get; }

		public bool IsValid => projectExplorer.Resources.Contains(Key);

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

		internal ProjectResourceDependency(ProjectExplorer projectExplorer, string key, Dictionary<string, string> metdadata)
		{
			this.projectExplorer = projectExplorer;
			this.metdadata = metdadata;

			Key = key;
		}
	}
}
