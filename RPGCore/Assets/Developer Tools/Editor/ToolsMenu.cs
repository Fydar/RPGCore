using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class ToolsMenu
{
	[MenuItem("Tools/Reserialize")]
	public static void Reserialize()
	{
		AssetDatabase.ForceReserializeAssets();
	}
}
