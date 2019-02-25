using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RPGCore/Misc/Project Configuration")]
public class ProjectConfiguration : ScriptableObject
{
	[Serializable]
	public struct ProjectReference
	{
		public string RelativePath;
	}
	public List<ProjectReference> References;
}
