using RPGCore.Behaviour.Editor;
using RPGCore.Behaviour.Manifest;
using RPGCore.Packages;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace RPGCore.Unity.Editors
{
	internal class ResourceTreeView : TreeView
	{
		private ProjectImport[] projectImports;
		public Dictionary<int, object> itemMappings;

		public ResourceTreeView(TreeViewState treeViewState)
			: base(treeViewState)
		{
		}

		protected override bool CanStartDrag(CanStartDragArgs args)
		{
			foreach (int identifier in args.draggedItemIDs)
			{
				if (!itemMappings.TryGetValue(identifier, out object mappedItem))
				{
					return false;
				}

				if (!(mappedItem is IResource) && !(mappedItem is IDirectory))
				{
					return false;
				}
			}

			return true;
		}

		protected override void SetupDragAndDrop(SetupDragAndDropArgs args)
		{
			DragAndDrop.PrepareStartDrag();
			DragAndDrop.paths = null;
			DragAndDrop.objectReferences = new UnityEngine.Object[] { };
			DragAndDrop.SetGenericData("ResourceTreeViewDraggedIds", new List<int>(args.draggedItemIDs));
			DragAndDrop.visualMode = DragAndDropVisualMode.Move;
			DragAndDrop.StartDrag("ResourceTreeViewDraggedIds");
		}

		protected override bool CanBeParent(TreeViewItem item)
		{
			if (item.id == 0)
			{
				return false;
			}
			if (!itemMappings.TryGetValue(item.id, out object mappedItem))
			{
				return false;
			}

			return mappedItem is IResource
				|| mappedItem is IDirectory
				|| mappedItem is IExplorer;
		}

		protected override DragAndDropVisualMode HandleDragAndDrop(DragAndDropArgs args)
		{
			if (!args.performDrop)
			{
				return DragAndDropVisualMode.Move;
			}

			return DragAndDropVisualMode.Move;
		}

		protected override void ContextClickedItem(int id)
		{
			var menu = new GenericMenu();

			menu.AddItem(new GUIContent("Create"), false, CreateCallback, id);

			if (itemMappings.TryGetValue(id, out object mappedItem))
			{
				if (mappedItem is IResource
					|| mappedItem is IDirectory)
				{
					menu.AddItem(new GUIContent("Rename"), false, BeginRenameCallback, id);
					menu.AddItem(new GUIContent("Delete"), false, DeleteCallback, id);
				}
			}

			menu.ShowAsContext();
			Repaint();
		}

		private void DeleteCallback(object args)
		{
			Debug.Log($"Deleting asset id {args}");
		}

		private void CreateCallback(object args)
		{
			Debug.Log($"Deleting asset id {args}");
		}

		private void BeginRenameCallback(object args)
		{
			var item = FindItem((int)args, rootItem);
			BeginRename(item);
		}


		protected override void RenameEnded(RenameEndedArgs args)
		{
			if (args.acceptedRename)
			{
				Debug.Log($"Renaming {args.originalName} (id {args.itemID}) to {args.newName}");
			}
		}

		protected override bool CanRename(TreeViewItem item)
		{
			if (!itemMappings.TryGetValue(item.id, out object mappedItem))
			{
				return false;
			}

			return mappedItem is IResource
				|| mappedItem is IDirectory;
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
			itemMappings = new Dictionary<int, object>();

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
			itemMappings[id] = explorer;

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

			collection.Add(new TreeViewItem
			{
				displayName = "Manifests",
				id = id++,
				depth = depth + 2,
				icon = ContentEditorResources.Instance.ManifestDependancyIcon
			});

			itemMappings[id] = BehaviourManifest.CreateFromAppDomain(AppDomain.CurrentDomain);

			collection.Add(new TreeViewItem
			{
				displayName = "RPGCore 1.0.0",
				id = id++,
				depth = depth + 3,
				icon = ContentEditorResources.Instance.ManifestDependancyIcon
			});

			collection.Add(new TreeViewItem
			{
				displayName = "Projects",
				id = id++,
				depth = depth + 2,
				icon = ContentEditorResources.Instance.ProjectDependancyIcon
			});

			BuildDirectory(collection, explorer.RootDirectory, depth + 1, ref id);
		}

		private void BuildDirectory(List<TreeViewItem> collection, IDirectory directory, int depth, ref int id)
		{
			foreach (var childDirectory in directory.Directories)
			{
				itemMappings[id] = childDirectory;

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
				itemMappings[id] = resource;

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
