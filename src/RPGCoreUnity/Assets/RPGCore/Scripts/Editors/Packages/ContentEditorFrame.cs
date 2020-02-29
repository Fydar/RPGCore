using RPGCore.Behaviour.Editor;
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
		private ProjectResource CurrentResource;

		[SerializeField] private TreeViewState resourceTreeViewState;
		private ResourceTreeView resourceTreeView;

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
						EditorGUILayout.LabelField(resource.FullName, string.Join(", ", resource.Tags));
					}
				}

				GUILayout.EndArea();
			}
		}
	}

	internal class ResourceTreeView : TreeView
	{
		private ProjectExplorer projectExplorer;
		public Dictionary<int, ProjectResource> resourceMapping;

		public ResourceTreeView(TreeViewState treeViewState)
			: base(treeViewState)
		{
		}

		public void SetTarget(ProjectExplorer projectExplorer)
		{
			if (this.projectExplorer == projectExplorer)
			{
				return;
			}

			this.projectExplorer = projectExplorer;
			Reload();
		}

		protected override TreeViewItem BuildRoot()
		{
			resourceMapping = new Dictionary<int, ProjectResource>();
			var root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };

			var allItems = new List<TreeViewItem>();
			int idCounter = 1;

			foreach (var resource in projectExplorer.Resources)
			{
				allItems.Add(new TreeViewItem
				{
					displayName = resource.FullName,
					id = idCounter,
					depth = 0,
					icon = ContentEditorResources.Instance.DocumentIcon
				});

				resourceMapping[idCounter] = resource;

				idCounter++;
			}

			SetupParentsAndChildrenFromDepths(root, allItems);

			return root;
		}
	}

}
