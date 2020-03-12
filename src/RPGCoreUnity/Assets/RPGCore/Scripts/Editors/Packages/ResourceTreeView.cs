using RPGCore.Behaviour.Editor;
using RPGCore.Packages;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;

namespace RPGCore.Unity.Editors
{
	internal class ResourceTreeView : TreeView
	{
		private ProjectImport[] projectImports;
		public Dictionary<int, ProjectResource> resourceMapping;

		public ResourceTreeView(TreeViewState treeViewState)
			: base(treeViewState)
		{
		}

		protected override bool CanMultiSelect(TreeViewItem item)
		{
			return false;
		}

		public void SetTarget(ProjectImport[] projectImports)
		{
			if (this.projectImports == projectImports)
			{
				return;
			}

			this.projectImports = projectImports;
			Reload();
		}

		protected override TreeViewItem BuildRoot()
		{
			resourceMapping = new Dictionary<int, ProjectResource>();

			var root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };
			var collection = new List<TreeViewItem>();

			if (projectImports != null)
			{
				int id = 1;
				foreach (var projectImport in projectImports)
				{
					BuildProject(collection, projectImport.Explorer, 0, ref id);
				}
			}

			SetupParentsAndChildrenFromDepths(root, collection);

			return root;
		}

		private void BuildProject(List<TreeViewItem> collection, IExplorer explorer, int depth, ref int id)
		{
			collection.Add(new TreeViewItem
			{
				displayName = explorer.Name,
				id = id++,
				depth = depth,
				icon = ContentEditorResources.Instance.ProjectIcon
			});

			collection.Add(new TreeViewItem
			{
				displayName = "Dependancies",
				id = id++,
				depth = depth + 1,
				icon = ContentEditorResources.Instance.DependanciesIcon
			});

			BuildDirectory(collection, explorer.RootDirectory, depth + 1, ref id);
		}

		private void BuildDirectory(List<TreeViewItem> collection, IDirectory directory, int depth, ref int id)
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

				BuildDirectory(collection, childDirectory, depth + 1, ref id);
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
