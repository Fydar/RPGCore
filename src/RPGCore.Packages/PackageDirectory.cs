using System.Diagnostics;

namespace RPGCore.Packages
{
	/// <summary>
	/// <para>Represents a <see cref="PackageDirectory"/> loaded from an <see cref="PackageExplorer"/>.</para>
	/// </summary>
	public class PackageDirectory : IDirectory
	{
		/// <inheritdoc/>
		public string Name { get; }

		/// <inheritdoc/>
		public string FullName { get; }

		/// <inheritdoc/>
		public IDirectory? Parent { get; }

		/// <summary>
		/// <para>The <see cref="PackageDirectory"/>s contained within this <see cref="PackageDirectory"/>.</para>
		/// </summary>
		public PackageDirectoryCollection Directories { get; }

		/// <summary>
		/// <para>The <see cref="PackageResource"/>s contained within this <see cref="PackageResource"/>.</para>
		/// </summary>
		public PackageResourceCollection Resources { get; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IDirectoryCollection IDirectory.Directories => Directories;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IResourceCollection IDirectory.Resources => Resources;

		internal PackageDirectory(string name, PackageDirectory? parent)
		{
			Name = name;
			FullName = MakeFullName(parent, name);
			Parent = parent;

			Directories = new PackageDirectoryCollection();
			Resources = new PackageResourceCollection();
		}

		private static string MakeFullName(IDirectory? parent, string key)
		{
			if (parent == null || string.IsNullOrEmpty(parent.FullName))
			{
				return key;
			}
			else
			{
				return $"{parent.FullName}/{key}";
			}
		}
	}
}
