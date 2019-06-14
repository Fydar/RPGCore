using UnityEngine;
using UnityEditor;
using RPGCore.Packages;
using System.Text;

namespace RPGCore.Unity.Editors
{
    public class BehaviourEditor : EditorWindow
    {
        public ProjectImport CurrentPackage;

        public bool HasCurrentAsset;
        public IAsset CurrentAsset;
        public string CurrentAssetJson;

        [MenuItem("Window/Behaviour")]
        public static void Open()
        {
            var window = EditorWindow.GetWindow<BehaviourEditor>();

            window.Show();
        }

        public void OnGUI()
        {
            CurrentPackage = (ProjectImport)EditorGUILayout.ObjectField(CurrentPackage, typeof(ProjectImport), true);

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

	    public static IResource FindBhvr (IAsset asset)
        {
            foreach (var resource in asset.Resources)
            {
                if (resource.Name.EndsWith (".bhvr", System.StringComparison.Ordinal))
                {
                    return resource;
                }
            }
            return default (PackageResource);
        }
    }
}