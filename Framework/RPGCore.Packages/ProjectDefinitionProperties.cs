using System.Xml;

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
}