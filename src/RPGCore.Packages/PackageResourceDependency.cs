using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.Packages
{
	/// <summary>
	/// Represents a dependency on a <see cref="PackageResource"/>.
	/// </summary>
	public class PackageResourceDependency : IResourceDependency
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly PackageExplorer packageExplorer;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly Dictionary<string, string> metdadata;
		
		/// <summary>
		/// The resource referenced by this dependency.
		/// </summary>
		public IReadOnlyDictionary<string, string> Metadata => metdadata;

		/// <summary>
		/// Determines the validity of this dependency relationship.
		/// </summary>
		public bool IsValid => packageExplorer.Resources.Contains(Key);

		/// <summary>
		/// A key used to identify this dependency.
		/// </summary>
		public string Key { get; }

		/// <summary>
		/// The resource referenced by this dependency.
		/// </summary>
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

		internal PackageResourceDependency(PackageExplorer packageExplorer, PackageResourceMetadataDependencyModel dependencyModel)
		{
			this.packageExplorer = packageExplorer;

			Key = dependencyModel.Resource;
			metdadata = new Dictionary<string, string>();
		}
	}
}
