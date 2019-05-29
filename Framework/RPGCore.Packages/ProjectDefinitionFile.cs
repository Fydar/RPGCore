using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace RPGCore.Packages
{
    public class ProjectDefinitionProperties
    {
        private XmlNode Element;

        public string Name
        {
            get
            {
                return Element.SelectSingleNode("Name").InnerXml;
            }
            set
            {
                Element.SelectSingleNode("Name").InnerXml = value;
            }
        }

        public string Version
        {
            get
            {
                return Element.SelectSingleNode("Version").InnerXml;
            }
            set
            {
                Element.SelectSingleNode("Version").InnerXml = value;
            }
        }

        public ProjectDefinitionProperties(XmlNode element)
        {
            Element = element;
        }
    }

    public class ProjectDefinitionFile : XmlProjectFile
    {
        public ProjectDefinitionProperties Properties;
        public List<Reference> References;

        public ProjectDefinitionFile(XmlDocument document)
            : base (document)
        {
            Properties = new ProjectDefinitionProperties(Document.GetElementsByTagName("Properties").Item(0));

            References = new List<Reference>();
            var referencesTag = Document.GetElementsByTagName("ProjectReference");
            for (int i = 0; i < referencesTag.Count; i++)
            {
                var projectReferenceElement = referencesTag.Item(i);

                if (projectReferenceElement is XmlElement element)
                {
                   References.Add(new ProjectReference(this, element));
                }
            }
        }

        public static new ProjectDefinitionFile Load(string path)
        {
            if (!File.Exists(path))
                return null;

            var doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            doc.Load(path);

            var model = new ProjectDefinitionFile(doc);
            model.Path = path;
            return model;
        }
    }

    public abstract class Reference
    {
        public abstract void IncludeInBuild(string output);
    }

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

        public override void IncludeInBuild(string output)
        {
            var accessPath = Path.Combine(File.Path, IncludePath);
            var projectExplorer = ProjectExplorer.Load(accessPath);
            projectExplorer.Export(Path.Combine(output));
        }
    }
}