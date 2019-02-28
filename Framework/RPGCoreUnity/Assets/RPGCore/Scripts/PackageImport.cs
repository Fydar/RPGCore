using RPGCore.Packages;
using UnityEngine;

namespace RPGCore.Unity
{
	[CreateAssetMenu(menuName = "RPGCore/Package Import")]
	public class PackageImport : ScriptableObject
	{
		public string RelativePath;

		private PackageExplorer explorer;

		public PackageExplorer Explorer
		{
			get
			{
				if (explorer == null)
				{
					explorer = PackageExplorer.Load (RelativePath);
				}
				return explorer;
			}
		}
	}
}