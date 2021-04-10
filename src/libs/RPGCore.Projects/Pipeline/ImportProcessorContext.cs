using RPGCore.Projects.Pipeline;

namespace RPGCore.Projects
{
	public class ImportProcessorContext
	{
		public ProjectExplorer Explorer { get; internal set; }

		public ProjectResourceUpdate AuthorUpdate(string name)
		{
			return new ProjectResourceUpdate(Explorer, name);
		}
	}
}
