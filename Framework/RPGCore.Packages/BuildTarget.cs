using System.Xml;
using System.Xml.Serialization;

namespace RPGCore.Packages
{
	public class BuildTarget
	{
		[XmlAttribute ("name")]
		public string Name;

	}
}