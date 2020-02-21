using System.IO;

namespace RPGCore.Packages.Pipeline
{
	/// <summary>
	/// Responcible for loading resources into this project from the project files and processing
	/// the <see cref="ProjectResource"/>.
	/// </summary>
	public abstract class ProjectResourceImporter
	{
		public abstract ProjectResource ImportResource(ProjectExplorer projectExplorer, FileInfo fileInfo, string projectKey);
	}
}
