using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using RPGCore.Behaviour.Packages;

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
