using RPGCore.Behaviour.Editor;
using RPGCore.Packages;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;

namespace RPGCore.Unity.Editors
{
	internal class ResourceTreeView : TreeView
	{
		private ProjectExplorer projectExplorer;
		public Dictionary<int, ProjectResource> resourceMapping;

		public ResourceTreeView(TreeViewState treeViewState)
			: base(treeViewState)
		{
		}

		protected override bool CanMultiSelect(TreeViewItem item)
		{
			return false;
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

			int ids = 1;
			BuildRecursive(allItems, projectExplorer.RootDirectory, 0, ref ids);

			SetupParentsAndChildrenFromDepths(root, allItems);

			return root;
		}

		private void BuildRecursive(List<TreeViewItem> collection, IDirectory directory, int depth, ref int id)
		{
			foreach (var childDirectory in directory.Directories)
			{
				collection.Add(new TreeViewItem
				{
					displayName = childDirectory.Name,
					id = id++,
					depth = depth,
					icon = ContentEditorResources.Instance.FolderIcon
				});

				BuildRecursive(collection, childDirectory, depth + 1, ref id);
			}

			foreach (var resource in directory.Resources)
			{
				resourceMapping[id] = (ProjectResource)resource;

				collection.Add(new TreeViewItem
				{
					displayName = resource.Name,
					id = id++,
					depth = depth,
					icon = ContentEditorResources.Instance.DocumentIcon
				});
			}
		}
	}
}
