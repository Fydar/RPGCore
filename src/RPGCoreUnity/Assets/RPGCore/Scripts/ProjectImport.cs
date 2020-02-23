using RPGCore.Packages;
using UnityEngine;

namespace RPGCore.Unity
{
	[CreateAssetMenu(menuName = "RPGCore/Package Import")]
	public class ProjectImport : ScriptableObject
	{
		public string RelativePath;

		private ProjectExplorer explorer;

		public ProjectExplorer Explorer
		{
			get
			{
				if (explorer == null)
				{
					var importPipeline = new ImportPipeline();

					explorer = ProjectExplorer.Load(RelativePath, importPipeline);
				}
				return explorer;
			}
		}

		public void Reload()
		{
			explorer = null;
		}
	}
}
