using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace RPGCore.Packages
{
	public class ProjectDefinitionFile : XmlProjectFile
	{
		public ProjectDefinitionProperties Properties;
		public List<Reference> References;

		public ProjectDefinitionFile(XmlDocument document)
			: base(document)
		{
			Properties = new ProjectDefinitionProperties(Document.GetElementsByTagName("Properties").Item(0));

			References = new List<Reference>();
			var projectReferenceTags = Document.GetElementsByTagName("ProjectReference");
			for (int i = 0; i < projectReferenceTags.Count; i++)
			{
				var projectReferenceElement = projectReferenceTags.Item(i);

				if (projectReferenceElement is XmlElement element)
				{
					References.Add(new ProjectReference(this, element));
				}
			}

			var resourceReferenceTags = Document.GetElementsByTagName("ResourceReference");
			for (int i = 0; i < resourceReferenceTags.Count; i++)
			{
				var resourceReferenceElement = resourceReferenceTags.Item(i);

				if (resourceReferenceElement is XmlElement element)
				{
					References.Add(new ResourceReference(this, element));
				}
			}
		}

		public static new ProjectDefinitionFile Load(string path)
		{
			if (!File.Exists(path))
			{
				return null;
			}

			var doc = new XmlDocument
			{
				PreserveWhitespace = true
			};
			doc.Load(path);

			var model = new ProjectDefinitionFile(doc)
			{
				Path = path
			};
			return model;
		}
	}
}
