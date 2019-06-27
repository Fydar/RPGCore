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
            get
            {
                return Element.Attributes["Include"].Value;
            }
            set
            {
               Element.Attributes["Include"].Value = value;
            }
        }
        
        public ProjectReference(ProjectDefinitionFile file, XmlElement element)
        {
            File = file;
            Element = element;
        }

        public override void IncludeInBuild(ProjectBuildProcess build, string output)
        {
            var accessPath = Path.Combine(File.Path, IncludePath);
            var projectExplorer = ProjectExplorer.Load(accessPath, build.Source.Importers);
            projectExplorer.Export(Path.Combine(output));
        }
    }
}