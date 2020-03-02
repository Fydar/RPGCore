using RPGCore.Packages;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPGCore.Unity.Editors
{
	public class ResourceInformationFrame : WindowFrame
	{
		public IResource Resource { get; private set; }

		private readonly List<FrameTab> VisualiserTabs = new List<FrameTab>();
		private int VisualiserTabsIndex;

		public ResourceInformationFrame(IResource resource)
		{
			Resource = resource;

			VisualiserTabs.Add(new FrameTab()
			{
				Title = new GUIContent("Raw"),
				Frame = new RawTextWindowFrame(resource)
			});

			VisualiserTabs.Add(new FrameTab()
			{
				Title = new GUIContent("Json"),
				Frame = new JsonTextWindowFrame(resource)
			});

			VisualiserTabs.Add(new FrameTab()
			{
				Title = new GUIContent("Image"),
				Frame = new TextureWindowFrame(resource)
			});
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
			for (int i = 0; i < VisualiserTabs.Count; i++)
			{
				var tab = VisualiserTabs[i];

				var originalColor = GUI.color;
				GUI.color = i == VisualiserTabsIndex
					? GUI.color
					: GUI.color * 0.725f;

				if (GUILayout.Button(tab.Title, EditorStyles.toolbarButton))
				{
					VisualiserTabsIndex = i;
				}

				GUI.color = originalColor;
			}
			EditorGUILayout.EndHorizontal();


			if (VisualiserTabsIndex >= 0 && VisualiserTabsIndex < VisualiserTabs.Count)
			{
				var currentTab = VisualiserTabs[VisualiserTabsIndex];
				currentTab.Frame.OnGUI();
			}
		}
	}
}
