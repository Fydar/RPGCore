using RPGCore.Packages;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace RPGCore.Unity.Editors
{
	public class ContentEditorFrame : WindowFrame
	{
		[SerializeField] private TreeViewState resourceTreeViewState;
		[SerializeField] private float treeViewWidth = 190;

		private bool isDraggingTreeView;

		private ProjectImport[] availableProjects;

		private ResourceTreeView resourceTreeView;

		private IResource currentResource;
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

			if (availableProjects == null)
			{
				availableProjects = Resources.LoadAll<ProjectImport>("");
			}

			var headerRect = new Rect(
				0,
				0,
				Position.width,
				EditorGUIUtility.singleLineHeight + 8);

			var treeViewRect = new Rect(
				0,
				EditorGUIUtility.singleLineHeight + 3,
				treeViewWidth,
				Position.height - EditorGUIUtility.singleLineHeight - 3);

			var treeViewDivierLineRect = new Rect(
				treeViewRect.xMax,
				treeViewRect.y,
				1,
				treeViewRect.height);

			var treeViewDivierDragRect = new Rect(
				treeViewRect.xMax - 1,
				treeViewRect.y,
				5,
				treeViewRect.height);

			var remainingRect = new Rect(
				treeViewWidth + 4,
				EditorGUIUtility.singleLineHeight + 3,
				Position.width - treeViewWidth - 4,
				Position.height - EditorGUIUtility.singleLineHeight);

			EditorGUI.DrawRect(treeViewDivierLineRect, new Color(0.0f, 0.0f, 0.0f, 0.2f));

			EditorGUIUtility.AddCursorRect(treeViewDivierDragRect, MouseCursor.ResizeHorizontal);
			if (treeViewDivierDragRect.Contains(Event.current.mousePosition))
			{
				if (Event.current.type == EventType.MouseDown)
				{
					isDraggingTreeView = true;
				}
			}
			if (Event.current.type == EventType.MouseUp)
			{
				isDraggingTreeView = false;
			}

			if (isDraggingTreeView)
			{
				treeViewWidth = Mathf.Clamp(Event.current.mousePosition.x, 120, 360);
				Window.Repaint();
			}


			GUILayout.BeginArea(headerRect, EditorStyles.toolbar);
			EditorGUILayout.BeginHorizontal();


			EditorGUILayout.EndHorizontal();
			GUILayout.EndArea();


			resourceTreeView.SetTarget(availableProjects);
			resourceTreeView.OnGUI(treeViewRect);

			GUILayout.BeginArea(remainingRect);

			foreach (int selected in resourceTreeView.GetSelection())
			{
				if (resourceTreeView.itemMappings.TryGetValue(selected, out object item))
				{
					if (item is IResource resource)
					{
						if (currentResource != resource)
						{
							currentResource = resource;

							currentResourceTabs.Clear();

							if (currentResource.Extension == ".json")
							{
								try
								{
									currentResourceTabs.Add(new FrameTab()
									{
										Title = new GUIContent("Editor"),
										Frame = new EditorSessionFrame(currentResource)
									});
								}
								catch (Exception exception)
								{
									Debug.LogError(exception.ToString());
								}
							}

							currentResourceTabs.Add(new FrameTab()
							{
								Title = new GUIContent("Information"),
								Frame = new ResourceInformationFrame(currentResource)
							});
						}
					}
				}
				else
				{
					currentResource = null;
				}

				if (currentResource != null)
				{
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
			}

			GUILayout.EndArea();
		}
	}
}
