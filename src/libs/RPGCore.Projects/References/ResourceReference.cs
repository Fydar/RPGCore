using System;
using System.IO;
using System.Security.Cryptography;
using System.Xml;

namespace RPGCore.Projects;

public class ResourceReference : Reference
{
	public ProjectDefinition DefinitionFile;
	public XmlElement Element;

	public string IncludePath
	{
		get => Element.Attributes["Include"].Value;
		set => Element.Attributes["Include"].Value = value;
	}

	public string Name
	{
		get => Element.Attributes["Name"].Value;
		set => Element.Attributes["Name"].Value = value;
	}

	public ResourceReference(ProjectDefinition file, XmlElement element)
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
