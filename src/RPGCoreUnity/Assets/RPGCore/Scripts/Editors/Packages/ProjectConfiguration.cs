using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RPGCore/Misc/Project Configuration")]
public class ProjectConfiguration : ScriptableObject
{
	private static ProjectConfiguration activeConfiguration;

	public static ProjectConfiguration ActiveConfiguration
	{
		get
		{
			if (activeConfiguration == null)
			{
				activeConfiguration = Resources.Load<ProjectConfiguration>("ProjectConfiguration");
			}
			return activeConfiguration;
		}
	}

	[Serializable]
	public struct ProjectReference
	{
		public string RelativePath;
	}
	public List<ProjectReference> References;
}
