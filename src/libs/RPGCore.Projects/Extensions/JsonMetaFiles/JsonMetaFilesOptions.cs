namespace RPGCore.Projects;

/// <summary>
/// <para>Configuration model for metafiles.</para>
/// </summary>
public class JsonMetaFilesOptions
{
	/// <summary>
	/// <para>The file extension used to locate meta files.</para>
	/// </summary>
	public string MetaFileSuffix { get; set; } = ".meta";

	/// <summary>
	/// <para>Used to determine whether a missing meta file will result in an error.</para>
	/// </summary>
	public bool IsMetaFilesOptional { get; set; } = true;

	internal JsonMetaFilesOptions()
	{
	}
}
