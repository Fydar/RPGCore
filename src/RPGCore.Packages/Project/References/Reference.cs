using RPGCore.Packages.Archives;

namespace RPGCore.Packages
{
	public abstract class Reference
	{
		public abstract void IncludeInBuild(ProjectBuildProcess build, string outputFolder);
	}
}
