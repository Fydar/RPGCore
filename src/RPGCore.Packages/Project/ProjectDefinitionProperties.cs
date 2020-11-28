using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace RPGCore.Packages
{
	public class ProjectDefinitionProperties : IDefinitionProperties
	{
		private readonly FileInfo file;
		private readonly XmlDocument document;

		public string Name
		{
			get
			{
				string name = GetNodeForProperty("Name")?.InnerText;
				return name ?? file.Name.Replace(".bproj", "");
			}
			set => GetOrCreateNodeForProperty("Name").InnerText = value;
		}

		public string Version
		{
			get => GetNodeForProperty("Version")?.InnerText;
			set => GetOrCreateNodeForProperty("Version").InnerText = value;
		}

		public ProjectDefinitionProperties(FileInfo file, XmlDocument document)
		{
			this.file = file;
			this.document = document;
		}

		private XmlNode GetNodeForProperty(string name)
		{
			foreach (var propertyGroup in PropertyGroups())
			{
				var properties = propertyGroup.ChildNodes;
				for (int j = 0; j < properties.Count; j++)
				{
					var property = properties.Item(j);

					if (property.Name == name)
					{
						return property;
					}
				}
			}
			return null;
		}

		private XmlNode GetOrCreateNodeForProperty(string name)
		{
			var node = GetNodeForProperty(name);

			if (node != null)
			{
				return node;
			}

			var propertyGroup = PropertyGroups().FirstOrDefault();
			if (propertyGroup == null)
			{
				var prefix = document.CreateWhitespace("\n  ");
				document.DocumentElement.AppendChild(prefix);

				var newPropertyGroup = document.CreateElement("PropertyGroup", null);
				document.DocumentElement.AppendChild(newPropertyGroup);
				propertyGroup = newPropertyGroup;

				prefix = document.CreateWhitespace("\n\n");
				document.DocumentElement.AppendChild(prefix);
			}

			var whitespace = document.CreateWhitespace("\n    ");
			propertyGroup.AppendChild(whitespace);

			var propertyNode = document.CreateElement(name, null);
			propertyGroup.AppendChild(propertyNode);

			whitespace = document.CreateWhitespace("\n  ");
			propertyGroup.AppendChild(whitespace);

			return propertyNode;
		}

		private IEnumerable<XmlNode> PropertyGroups()
		{
			var rootNodes = document.DocumentElement.ChildNodes;
			for (int j = 0; j < rootNodes.Count; j++)
			{
				var property = rootNodes.Item(j);

				if (property.Name == "PropertyGroup")
				{
					yield return property;
				}
			}
		}
	}
}
