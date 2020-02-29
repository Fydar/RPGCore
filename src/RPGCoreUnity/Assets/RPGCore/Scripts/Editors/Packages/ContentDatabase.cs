using RPGCore.Packages;
using System.IO;
using UnityEngine;

public static class ContentDatabase
{
	private static PackageExplorer primaryPackage;

	public static PackageExplorer PrimaryPackage
	{
		get
		{
			if (primaryPackage == null)
			{
				string path = ProjectConfiguration.ActiveConfiguration.References[0].RelativePath;
				Debug.Log("Loading package at path: " + new FileInfo(path).FullName);
				primaryPackage = PackageExplorer.Load(path);
			}
			return primaryPackage;
		}
	}
}
