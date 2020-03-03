using RPGCore.Packages;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPGCore.Unity.Editors
{
	public class ResourceInformationFrame : WindowFrame
	{
		public IResource Resource { get; private set; }

		private readonly List<FrameTab> visualiserTabs = new List<FrameTab>();
		private int visualiserTabsIndex;

		public ResourceInformationFrame(IResource resource)
		{
			Resource = resource;

			if (resource.Extension == ".png"
				|| resource.Extension == ".jpeg")
			{
				visualiserTabs.Add(new FrameTab()
				{
					Title = new GUIContent("Image"),
					Frame = new TextureWindowFrame(resource)
				});
			}
			else if (resource.Extension == ".json")
			{
				visualiserTabs.Add(new FrameTab()
				{
					Title = new GUIContent("Json"),
					Frame = new JsonTextWindowFrame(resource)
				});

				visualiserTabs.Add(new FrameTab()
				{
					Title = new GUIContent("Raw"),
					Frame = new RawTextWindowFrame(resource)
				});
			}
			else
			{
				visualiserTabs.Add(new FrameTab()
				{
					Title = new GUIContent("Raw"),
					Frame = new RawTextWindowFrame(resource)
				});
			}
		}

		public override void OnEnable()
		{
		}

		public override void OnGUI()
		{
			EditorGUILayout.LabelField("Resource Name", Resource.Name);
			EditorGUILayout.LabelField("Resource Full Name", Resource.FullName);
			EditorGUILayout.LabelField("Compressed Size", Resource.CompressedSize.ToString());
			EditorGUILayout.LabelField("Uncompressed Size", Resource.UncompressedSize.ToString());

			GUILayout.Space(8);

			EditorGUILayout.LabelField("Tags", EditorStyles.centeredGreyMiniLabel);
			EditorGUILayout.BeginVertical(EditorStyles.helpBox);
			foreach (string tag in Resource.Tags)
			{
				EditorGUILayout.LabelField(tag, EditorStyles.boldLabel);
			}
			EditorGUILayout.EndVertical();


			EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
			for (int i = 0; i < visualiserTabs.Count; i++)
			{
				var tab = visualiserTabs[i];

				var originalColor = GUI.color;
				GUI.color = i == visualiserTabsIndex
					? GUI.color
					: GUI.color * 0.725f;

				if (GUILayout.Button(tab.Title, EditorStyles.toolbarButton))
				{
					visualiserTabsIndex = i;
				}

				GUI.color = originalColor;
			}
			EditorGUILayout.EndHorizontal();


			if (visualiserTabsIndex >= 0 && visualiserTabsIndex < visualiserTabs.Count)
			{
				var currentTab = visualiserTabs[visualiserTabsIndex];
				currentTab.Frame.OnGUI();
			}
		}
	}
}
