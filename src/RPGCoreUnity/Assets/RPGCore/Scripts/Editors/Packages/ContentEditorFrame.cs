using RPGCore.Packages;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace RPGCore.Unity.Editors
{
	public class ContentEditorFrame : WindowFrame
	{
		private ProjectImport CurrentPackage;

		[SerializeField] private TreeViewState resourceTreeViewState;
		private ResourceTreeView resourceTreeView;

		private ProjectResource CurrentResource;
		private readonly List<FrameTab> CurrentResourceTabs = new List<FrameTab>();
		private int CurrentResourceTabIndex;

		public override void OnEnable()
		{
		}

		public override void OnGUI()
		{
			if (resourceTreeViewState == null)
			{
				resourceTreeViewState = new TreeViewState();
			}
			if (resourceTreeView == null)
			{
				resourceTreeView = new ResourceTreeView(resourceTreeViewState);
			}

			CurrentPackage = (ProjectImport)EditorGUILayout.ObjectField(CurrentPackage, typeof(ProjectImport), true,
				GUILayout.Width(180));
			if (CurrentPackage == null)
			{
				return;
			}

			if (resourceTreeView != null)
			{
				resourceTreeView.SetTarget(CurrentPackage.Explorer);

				var treeViewRect = new Rect(
					0,
					EditorGUIUtility.singleLineHeight + 6,
					180,
					Position.height - EditorGUIUtility.singleLineHeight - 6);

				var remainingRect = new Rect(
					184,
					0,
					Position.width - 184,
					Position.height);

				resourceTreeView.OnGUI(treeViewRect);

				GUILayout.BeginArea(remainingRect);

				foreach (int selected in resourceTreeView.GetSelection())
				{
					if (resourceTreeView.resourceMapping.TryGetValue(selected, out var resource))
					{
						if (CurrentResource != resource)
						{
							CurrentResource = resource;

							CurrentResourceTabs.Clear();

							try
							{
								CurrentResourceTabs.Add(new FrameTab()
								{
									Title = new GUIContent("Editor"),
									Frame = new EditorSessionFrame(CurrentResource)
								});
							}
							catch
							{

							}

							CurrentResourceTabs.Add(new FrameTab()
							{
								Title = new GUIContent("Information"),
								Frame = new ResourceInformationFrame(CurrentResource)
							});
						}
					}

					EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
					for (int i = 0; i < CurrentResourceTabs.Count; i++)
					{
						var tab = CurrentResourceTabs[i];

						var originalColor = GUI.color;
						GUI.color = i == CurrentResourceTabIndex
							? GUI.color
							: GUI.color * 0.725f;

						if (GUILayout.Button(tab.Title, EditorStyles.toolbarButton))
						{
							CurrentResourceTabIndex = i;
						}

						GUI.color = originalColor;
					}
					EditorGUILayout.EndHorizontal();

					if (CurrentResourceTabIndex >= 0 && CurrentResourceTabIndex < CurrentResourceTabs.Count)
					{
						var currentTab = CurrentResourceTabs[CurrentResourceTabIndex];
						currentTab.Frame.OnGUI();
					}
				}

				GUILayout.EndArea();
			}
		}
	}
}
