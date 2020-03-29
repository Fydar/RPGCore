using RPGCore.Behaviour.Manifest;
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
		private class Styles
		{
			public GUIStyle TopBarBg { get; } = "ProjectBrowserTopBarBg";
			public GUIStyle Separator { get; } = "ArrowNavigationRight";
		}

		private static Styles styles;

		[SerializeField] private TreeViewState resourceTreeViewState;
		[SerializeField] private float treeViewWidth = 190;

		private bool isDraggingTreeView;

		private ProjectImport[] availableProjects;

		private ResourceTreeView resourceTreeView;

		public ResourceTreeViewItem Selection
		{
			get
			{
				return selection;
			}
			set
			{
				if (selection == value)
				{
					return;
				}

				selection = value;
				selectionTabs.Clear();

				if (selection == null)
				{
					return;
				}

				resourceTreeView.SetSelection(new int[] { selection.id });

				if (selection.item is IResource resource)
				{
					if (resource.Extension == ".json")
					{
						try
						{
							selectionTabs.Add(new FrameTab()
							{
								Title = new GUIContent("Editor"),
								Frame = new EditorSessionFrame(resource)
							});
						}
						catch (Exception exception)
						{
							Debug.LogError(exception.ToString());
						}
					}

					selectionTabs.Add(new FrameTab()
					{
						Title = new GUIContent("Information"),
						Frame = new ResourceInformationFrame(resource)
					});
				}
				else if (selection.item is BehaviourManifest manifest)
				{
					selectionTabs.Add(new FrameTab()
					{
						Title = new GUIContent("Manifest"),
						Frame = new ManifestInspectFrame(manifest)
					});
					selectionTabIndex = 0;
				}
			}
		}
		private ResourceTreeViewItem selection;
		private readonly List<FrameTab> selectionTabs = new List<FrameTab>();
		private int selectionTabIndex;


		public override void OnEnable()
		{
		}

		public override void OnGUI()
		{
			if (styles == null)
			{
				styles = new Styles();
			}
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

			var breadCrumbsBar = new Rect(
				treeViewWidth + 1,
				EditorGUIUtility.singleLineHeight + 3,
				Position.width - treeViewWidth - 1,
				21f);

			var remainingRect = new Rect(
				treeViewWidth + 1,
				EditorGUIUtility.singleLineHeight + 3 + breadCrumbsBar.height,
				Position.width - treeViewWidth - 1,
				Position.height - EditorGUIUtility.singleLineHeight - breadCrumbsBar.height);

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

			DrawBreadcrumbs(breadCrumbsBar);

			GUILayout.BeginArea(remainingRect);

			foreach (int selected in resourceTreeView.GetSelection())
			{
				if (resourceTreeView.idToItemMapping.TryGetValue(selected, out var item))
				{
					if (item != Selection)
					{
						Selection = item;
					}
				}
				else
				{
					Selection = null;
				}

				if (selectionTabs != null && selectionTabs.Count != 0)
				{
					EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
					for (int i = 0; i < selectionTabs.Count; i++)
					{
						var tab = selectionTabs[i];

						var originalColor = GUI.color;
						GUI.color = i == selectionTabIndex
							? GUI.color
							: GUI.color * 0.725f;

						if (GUILayout.Button(tab.Title, EditorStyles.toolbarButton))
						{
							selectionTabIndex = i;
						}

						GUI.color = originalColor;
					}
					EditorGUILayout.EndHorizontal();

					if (selectionTabIndex >= 0 && selectionTabIndex < selectionTabs.Count)
					{
						var currentTab = selectionTabs[selectionTabIndex];
						currentTab.Frame.OnGUI();
					}
				}
			}

			GUILayout.EndArea();
		}

		private void DrawBreadcrumbs(Rect rect)
		{
			var breadcrumbs = new List<ResourceTreeViewItem>();
			var currentObj = Selection;
			int itr = 0;
			while (currentObj != null)
			{
				breadcrumbs.Add(currentObj);
				if (currentObj.parent is ResourceTreeViewItem parentObj)
				{
					currentObj = parentObj;
				}
				else
				{
					break;
				}


				itr++;

				if (itr > 20)
				{
					break;
				}
			}

			EditorGUI.LabelField(rect, "", styles.TopBarBg);

			for (int i = breadcrumbs.Count - 1; i >= 0; i--)
			{
				var breadcrumbItem = breadcrumbs[i];
				bool lastElement = i == 0;

				GUIContent labelContent;

				if (breadcrumbItem.item is IResource currentResource)
				{
					labelContent = new GUIContent(currentResource.Name);
				}
				else if (breadcrumbItem.item is IDirectory currentDirectory)
				{
					labelContent = new GUIContent(currentDirectory.Name);
				}
				else
				{
					labelContent = new GUIContent(breadcrumbItem.item?.ToString() ?? "null");
				}

				var labelStyle = lastElement ? EditorStyles.boldLabel : EditorStyles.label;
				var size = labelStyle.CalcSize(labelContent);
				rect.width = size.x;
				if (GUI.Button(rect, labelContent, labelStyle))
				{
					Selection = breadcrumbItem;
				}

				rect.x += size.x;
				if (!lastElement)
				{
					var buttonRect = new Rect(rect.x, rect.y + (rect.height - styles.Separator.fixedHeight) / 2, styles.Separator.fixedWidth, styles.Separator.fixedHeight);
					if (EditorGUI.DropdownButton(buttonRect, GUIContent.none, FocusType.Passive, styles.Separator))
					{
					}
				}
				rect.x += styles.Separator.fixedWidth;
			}
		}
	}
}
