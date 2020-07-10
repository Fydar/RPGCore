using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace RPGCore.Packages
{
	public class ProjectDefinition : IDefinition
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly XmlDocument document;

		public string Path { get; }

		// Work-in-progress "References" feature.
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal List<Reference> References { get; }

		public ProjectDefinitionProperties Properties { get; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IDefinitionProperties IDefinition.Properties => Properties;

		private ProjectDefinition(string path, XmlDocument document)
		{
			var projectFile = new FileInfo(path);
			Properties = new ProjectDefinitionProperties(projectFile, document);

			References = new List<Reference>();
			var projectReferenceTags = document.GetElementsByTagName("ProjectReference");
			for (int i = 0; i < projectReferenceTags.Count; i++)
			{
				var projectReferenceElement = projectReferenceTags.Item(i);

				if (projectReferenceElement is XmlElement element)
				{
					References.Add(new ProjectReference(this, element));
				}
			}

			var resourceReferenceTags = document.GetElementsByTagName("ResourceReference");
			for (int i = 0; i < resourceReferenceTags.Count; i++)
			{
				var resourceReferenceElement = resourceReferenceTags.Item(i);

				if (resourceReferenceElement is XmlElement element)
				{
					References.Add(new ResourceReference(this, element));
				}
			}
			Path = path;
			this.document = document;
		}

		public void SaveChanges()
		{
			XmlProjectFile.Format(document);
			document.Save(Path);
		}

		public static ProjectDefinition Load(string path)
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

			var model = new ProjectDefinition(path, doc);
			return model;
		}
	}
}
