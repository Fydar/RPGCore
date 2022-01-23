namespace RPGCore.FileTree;

public static class IArchiveDirectoryExtensions
{
	public static IArchiveFile GetFileRelative(this IArchiveDirectory directory, string path)
	{
		string[] elements = path.Split(new char[] { '/' });

		var placementDirectory = directory;
		for (int i = 0; i < elements.Length - 1; i++)
		{
			placementDirectory = placementDirectory.Directories.GetDirectory(elements[i]);
		}

		string fileName = elements[elements.Length - 1];
		return placementDirectory.Files.GetFile(fileName);
	}
}
