using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.Packages
{
	public class PackageResourceDependency : IResourceDependency
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly PackageExplorer packageExplorer;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly Dictionary<string, string> metdadata;

		public IReadOnlyDictionary<string, string> Metadata => metdadata;

		public bool IsValid => packageExplorer.Resources.Contains(Key);

		public string Key { get; }

		public PackageResource? Resource
		{
			get
			{
				try
				{
					return packageExplorer.Resources[Key];
				}
				catch
				{
					return null;
				}
			}
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IResource? IResourceDependency.Resource => Resource;

		public PackageResourceDependency(PackageExplorer packageExplorer, PackageResourceMetadataDependencyModel dependencyModel)
		{
			this.packageExplorer = packageExplorer;

			Key = dependencyModel.Resource;
			metdadata = new Dictionary<string, string>();
		}
	}
}
