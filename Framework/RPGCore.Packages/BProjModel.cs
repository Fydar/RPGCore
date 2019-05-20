using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace RPGCore.Packages
{
	public class BProjModelGeneralSection
	{
		public string Name;
		public string Version;
	}

	[Serializable]
	public abstract class Reference
	{
		
	}

	[Serializable]
	[XmlRoot(ElementName = "ProjectReference")]
	public class ProjectReference : Reference
	{
		[XmlAttribute]
		public string Include;
	}


	[XmlRoot(ElementName = "Project", IsNullable = false)]
	public class BProjModel
	{
		public BProjModelGeneralSection Properties;

		public XmlElement Plugins;

		[XmlArray]
		public BuildTarget[] Builds;
		
		[XmlArray]
   		[XmlArrayItem(typeof(ProjectReference))]
		public Reference[] References;

		public static BProjModel Load (string path)
		{
			if (!File.Exists (path))
				return null;

       		var serializer = new XmlSerializer(typeof(BProjModel));
 
       		var fs = new FileStream(path, FileMode.Open);

			var bProj = (BProjModel)serializer.Deserialize(fs);

			fs.Close();

			return bProj;
		}

		public void Save(string path)
		{
			var serializer = new XmlSerializer(typeof(BProjModel));

			var writer = new StreamWriter(path);
			serializer.Serialize(writer, this);
			writer.Close();
		}
	}
}