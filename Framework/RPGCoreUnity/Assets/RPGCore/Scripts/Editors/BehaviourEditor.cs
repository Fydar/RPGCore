using UnityEngine;
using UnityEditor;
using RPGCore.Packages;
using System.Text;

namespace RPGCore.Unity.Editors
{
    public class BehaviourEditor : EditorWindow
    {
        public PackageImport CurrentPackage;

        public bool HasCurrentAsset;
        public PackageAsset CurrentAsset;
        public string CurrentAssetJson;

        [MenuItem("Window/Behaviour")]
        public static void Open()
        {
            var window = EditorWindow.GetWindow<BehaviourEditor>();

            window.Show();
        }

        public void OnGUI()
        {
            CurrentPackage = (PackageImport)EditorGUILayout.ObjectField(CurrentPackage, typeof(PackageImport));

            var explorer = CurrentPackage.Explorer;

            foreach(var asset in explorer.Assets)
            {
                if (GUILayout.Button(asset.ToString()))
                {
                    CurrentAsset = asset;
                    HasCurrentAsset = true;
                    CurrentAssetJson = null;
                }
            }

            if (HasCurrentAsset)
            {
                if (string.IsNullOrEmpty(CurrentAssetJson))
                {
                    var bhvr = FindBhvr(CurrentAsset);
                    Debug.Log(bhvr);
                    CurrentAssetJson = Encoding.UTF8.GetString(bhvr.LoadData());
                }

                CurrentAssetJson = EditorGUILayout.TextArea(CurrentAssetJson);
            }
        }

	    public static PackageResource FindBhvr (PackageAsset asset)
        {
            for (int i = 0; i < asset.Resources.Length; i++)
            {
                if (asset.Resources[i].Name.EndsWith (".bhvr", System.StringComparison.Ordinal))
                {
                    return asset.Resources[i];
                }
            }
            return default (PackageResource);
        }
    }
}