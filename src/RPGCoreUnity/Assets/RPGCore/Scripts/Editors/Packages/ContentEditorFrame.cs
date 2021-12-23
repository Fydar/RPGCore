using RPGCore.Packages;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace RPGCoreUnity.Editors
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
		private ContentTreeView resourceTreeView;

		public ContentTreeViewItem Selection
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

				if (selection == null)
				{
					return;
				}

				Selection.Open(Window);

				resourceTreeView.SetSelection(new int[] { selection.id });

			}
		}
		private ContentTreeViewItem selection;

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
				resourceTreeView = new ContentTreeView(resourceTreeViewState);
			}
			if (availableProjects == null)
			{
				availableProjects = Resources.LoadAll<ProjectImport>("");
			}

			var toolbarRect = new Rect(
				0, 0,
				Position.width, EditorGUIUtility.singleLineHeight + 3);

			var treeViewRect = new Rect(
				0, toolbarRect.yMax,
				treeViewWidth, Position.height - toolbarRect.yMax);

			var treeViewDivierRect = new Rect(
				treeViewRect.xMax, treeViewRect.y,
				1, treeViewRect.height);

			var treeViewDivierDragRect = new Rect(
				treeViewRect.xMax - 1, treeViewRect.y,
				5, treeViewRect.height);

			var breadcrumbsBarRect = new Rect(
				treeViewWidth + 1, toolbarRect.yMax,
				Position.width - treeViewDivierRect.xMin, 21f);

			var tabsBarRect = new Rect(
				treeViewWidth + 1, breadcrumbsBarRect.yMax,
				Position.width - treeViewDivierRect.xMin, 21f);

			var rightPanelRect = new Rect(
				treeViewWidth + 1, tabsBarRect.yMax,
				Position.width - treeViewDivierRect.xMin, Position.height - tabsBarRect.yMax);

			EditorGUI.DrawRect(treeViewDivierRect, new Color(0.0f, 0.0f, 0.0f, 0.2f));

			EditorGUIUtility.AddCursorRect(treeViewDivierDragRect, MouseCursor.ResizeHorizontal);
			if (treeViewDivierDragRect.Contains(Event.current.mousePosition))
			{
				if (Event.current.type == EventType.MouseDown)
				{
					isDraggingTreeView = true;
					Event.current.Use();
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

			// Toolbar
			GUILayout.BeginArea(toolbarRect, EditorStyles.toolbar);
			EditorGUILayout.BeginHorizontal();


			EditorGUILayout.EndHorizontal();
			GUILayout.EndArea();

			// Treeview and update selectrion
			resourceTreeView.SetTarget(availableProjects);
			resourceTreeView.OnGUI(treeViewRect);

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
			}

			// Breadcrumbs
			DrawBreadcrumbs(breadcrumbsBarRect);

			// Tabs
			if (Selection?.Tabs != null && Selection?.Tabs.Count != 0)
			{
				GUILayout.BeginArea(tabsBarRect, EditorStyles.toolbar);
				EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

				foreach (var tab in Selection.Tabs)
				{
					var originalColor = GUI.color;
					GUI.color = tab == Selection.CurrentTab
						? GUI.color
						: GUI.color * 0.725f;

					if (GUILayout.Button(tab.Title, EditorStyles.toolbarButton))
					{
						Selection.CurrentTab = tab;
					}

					GUI.color = originalColor;
				}

				EditorGUILayout.EndHorizontal();
				GUILayout.EndArea();
			}

			// Right panel
			if (Selection?.CurrentTab != null)
			{
				Selection.CurrentTab.Frame.Position = rightPanelRect;
				Selection.CurrentTab.Frame.OnGUI();
			}
		}

		private void DrawBreadcrumbs(Rect rect)
		{
			var breadcrumbs = new List<ContentTreeViewItem>();
			var currentObj = Selection;
			int itr = 0;
			while (currentObj != null)
			{
				breadcrumbs.Add(currentObj);
				if (currentObj.parent is ContentTreeViewItem parentObj)
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

			rect.xMin += 8.0f;

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
				else if (breadcrumbItem.item is IExplorer currentExplorer)
				{
					labelContent = new GUIContent(currentExplorer.Definition.Properties.Name);
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
