using System;
using System.IO;
using System.Security.Cryptography;
using System.Xml;

namespace RPGCore.Packages
{
	public class ResourceReference : Reference
	{
		public ProjectDefinitionFile DefinitionFile;
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

		public string Name
		{
			get
			{
				return Element.Attributes["Name"].Value;
			}
			set
			{
				Element.Attributes["Name"].Value = value;
			}
		}

		public ResourceReference(ProjectDefinitionFile file, XmlElement element)
		{
			DefinitionFile = file;
			Element = element;
		}

		public override void IncludeInBuild(ProjectBuildProcess build, string output)
		{
			string accessPath = Path.Combine(DefinitionFile.Path, IncludePath);
			var accessFile = new FileInfo(accessPath);

			File.Copy(accessPath, Path.Combine(output, accessFile.Name), true);

			string sha = GetChecksum(accessPath);

			build.PackageDefinition.ResourceDependancies.Add(
				accessFile.Name,
				new ResourceDependancyDefinition()
				{
					Name = Name,
					Sha = sha
				}
			);
		}

		private static string GetChecksum(string file)
		{
			using var stream = new FileStream(file, FileMode.OpenOrCreate,
				FileAccess.Read, FileShare.Read, 1024 * 32);

			var sha = new SHA256Managed();
			byte[] checksum = sha.ComputeHash(stream);
			return BitConverter.ToString(checksum).Replace("-", string.Empty);
		}
	}
}
