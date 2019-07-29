using System.IO;
using System.Xml;

namespace RPGCore.Packages
{
	public class ProjectReference : Reference
	{
		public ProjectDefinitionFile File;
		public XmlElement Element;

		public string IncludePath
		{
			get => Element.Attributes["Include"].Value;
			set => Element.Attributes["Include"].Value = value;
		}

		public ProjectReference (ProjectDefinitionFile file, XmlElement element)
		{
			File = file;
			Element = element;
		}

		public override void IncludeInBuild (ProjectBuildProcess build, string output)
		{
			string accessPath = Path.Combine (File.Path, IncludePath);
			var projectExplorer = ProjectExplorer.Load (accessPath);
			projectExplorer.Export (build.Pipeline, Path.Combine (output));
		}
	}
}
