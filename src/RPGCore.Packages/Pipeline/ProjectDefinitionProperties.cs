using System.Xml;

namespace RPGCore.Packages
{
	public class ProjectDefinitionProperties
	{
		private readonly XmlNode element;

		public string Name
		{
			get => element.SelectSingleNode("Name").InnerXml;
			set => element.SelectSingleNode("Name").InnerXml = value;
		}

		public string Version
		{
			get => element.SelectSingleNode("Version").InnerXml;
			set => element.SelectSingleNode("Version").InnerXml = value;
		}

		public ProjectDefinitionProperties(XmlNode element)
		{
			this.element = element;
		}
	}
}
