using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBrowser : MonoBehaviour
{
	private void Start()
	{
		var explorer = ContentDatabase.PrimaryPackage;

		foreach (var asset in explorer.Folders)
		{
			Debug.Log(asset);
		}
	}
}
