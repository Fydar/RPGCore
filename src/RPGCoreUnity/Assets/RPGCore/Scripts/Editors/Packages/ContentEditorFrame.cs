using RPGCore.Packages;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace RPGCore.Unity.Editors
{
	public class ContentEditorFrame : WindowFrame
	{
		private ProjectImport currentPackage;

		[SerializeField] private TreeViewState resourceTreeViewState;
		private ResourceTreeView resourceTreeView;

		private ProjectResource currentResource;
		private readonly List<FrameTab> currentResourceTabs = new List<FrameTab>();
		private int currentResourceTabIndex;

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

			currentPackage = (ProjectImport)EditorGUILayout.ObjectField(currentPackage, typeof(ProjectImport), true,
				GUILayout.Width(180));
			if (currentPackage == null)
			{
				return;
			}

			if (resourceTreeView != null)
			{
				resourceTreeView.SetTarget(currentPackage.Explorer);

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
						if (currentResource != resource)
						{
							currentResource = resource;

							currentResourceTabs.Clear();

							try
							{
								currentResourceTabs.Add(new FrameTab()
								{
									Title = new GUIContent("Editor"),
									Frame = new EditorSessionFrame(currentResource)
								});
							}
							catch
							{

							}

							currentResourceTabs.Add(new FrameTab()
							{
								Title = new GUIContent("Information"),
								Frame = new ResourceInformationFrame(currentResource)
							});
						}
					}

					EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
					for (int i = 0; i < currentResourceTabs.Count; i++)
					{
						var tab = currentResourceTabs[i];

						var originalColor = GUI.color;
						GUI.color = i == currentResourceTabIndex
							? GUI.color
							: GUI.color * 0.725f;

						if (GUILayout.Button(tab.Title, EditorStyles.toolbarButton))
						{
							currentResourceTabIndex = i;
						}

						GUI.color = originalColor;
					}
					EditorGUILayout.EndHorizontal();

					if (currentResourceTabIndex >= 0 && currentResourceTabIndex < currentResourceTabs.Count)
					{
						var currentTab = currentResourceTabs[currentResourceTabIndex];
						currentTab.Frame.OnGUI();
					}
				}

				GUILayout.EndArea();
			}
		}
	}
}
