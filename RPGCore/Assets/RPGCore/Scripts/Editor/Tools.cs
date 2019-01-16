#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace RPGCore.Editors
{
	public class ReserializeAllItems : MonoBehaviour
	{
		[MenuItem ("Tools/Reserialize")]
		public static void Reserialize ()
		{
			AssetDatabase.ForceReserializeAssets ();
		}

		[MenuItem ("Tools/Delete Empty Directories")]
		public static void DeleteEmptyDirectories ()
		{
			Process (Application.dataPath);
		}

		public static bool Process (string dir)
		{
			var subdirs = Directory.GetDirectories (dir);

			int deletedCount = 0;
			foreach (var subdir in subdirs)
			{
				if (Process (subdir))
				{
					deletedCount++;
				}
			}

			if (deletedCount >= subdirs.Length)
			{
				// optimisation - don't query the file list if we have subdirs
				var files = Directory.GetFiles (dir);

				var delete = false;
				if (files.Length == 0)
				{
					delete = true;
				}
				else if ((files.Length == 1) && files[0].EndsWith ("/.DS_Store", System.StringComparison.Ordinal))
				{
					File.Delete (Path.Combine (dir, ".DS_Store"));
					delete = true;
				}

				if (delete)
				{
					// empty, remove
					Directory.Delete (dir, recursive: false);

					// remove meta
					File.Delete (dir + ".meta");

					return true;
				}
			}

			return false;
		}
	}
}
#endif
