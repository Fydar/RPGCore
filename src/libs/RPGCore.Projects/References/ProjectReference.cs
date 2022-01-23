using System.IO;
using System.Xml;

namespace RPGCore.Projects;

public class ProjectReference : Reference
{
	public ProjectDefinition File;
	public XmlElement Element;

	public string IncludePath
	{
		get => Element.Attributes["Include"].Value;
		set => Element.Attributes["Include"].Value = value;
	}

	public ProjectReference(ProjectDefinition file, XmlElement element)
	{
		File = file;
		Element = element;
	}

	public override void IncludeInBuild(ProjectBuildProcess build, string output)
	{
		string accessPath = Path.Combine(File.Path, IncludePath);

		using var projectExplorer = ProjectExplorer.Load(accessPath, build.Pipeline.ImportPipeline);
		projectExplorer.ExportZippedToDirectory(build.Pipeline, Path.Combine(output));
	}
}
