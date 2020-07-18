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
	internal class ContentTreeView : TreeView
	{
		private ProjectImport[] projectImports;
		public Dictionary<int, ContentTreeViewItem> idToItemMapping = new Dictionary<int, ContentTreeViewItem>();

		public ContentTreeView(TreeViewState treeViewState)
			: base(treeViewState)
		{
		}

		protected override bool CanStartDrag(CanStartDragArgs args)
		{
			foreach (int identifier in args.draggedItemIDs)
			{
				if (!idToItemMapping.TryGetValue(identifier, out var mappedItem))
				{
					return false;
				}

				if (mappedItem.item is IResource || mappedItem.item is IDirectory)
				{
					continue;
				}
				return false;
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
			if (!idToItemMapping.TryGetValue(item.id, out var mappedItem))
			{
				return false;
			}

			return mappedItem.item is IResource
				|| mappedItem.item is IDirectory
				|| mappedItem.item is IExplorer;
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

			if (idToItemMapping.TryGetValue(id, out var mappedItem))
			{
				if (mappedItem.item is IResource
					|| mappedItem.item is IDirectory)
				{
					menu.AddItem(new GUIContent("Rename"), false, BeginRenameCallback, id);
					menu.AddItem(new GUIContent("Delete"), false, DeleteCallback, id);
				}
				if (mappedItem.item is ProjectDirectory projectDirectory)
				{
					menu.AddItem(new GUIContent("Open in File Explorer"), false, OpenInFileExplorer, projectDirectory);
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

		private void OpenInFileExplorer(object args)
		{
			// var projectDirectory = args as ProjectDirectory;
			// EditorUtility.RevealInFinder(projectDirectory.FullName);
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
			if (!idToItemMapping.TryGetValue(item.id, out var mappedItem))
			{
				return false;
			}

			return mappedItem.item is IResource
				|| mappedItem.item is IDirectory;
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
			idToItemMapping.Clear();

			var root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };
			var collection = new List<TreeViewItem>();

			if (projectImports != null)
			{
				int id = 1;
				foreach (var projectImport in projectImports)
				{
					BuildProject(collection, projectImport.SourceFiles, 0, ref id);
				}
			}

			SetupParentsAndChildrenFromDepths(root, collection);

			return root;
		}

		private void BuildProject(List<TreeViewItem> collection, IExplorer explorer, int depth, ref int id)
		{
			var newItem = new ContentTreeViewItem
			{
				displayName = explorer.Definition.Properties.Name,
				id = id++,
				depth = depth,
				icon = ContentEditorResources.Instance.ProjectIcon,

				item = explorer
			};
			idToItemMapping[newItem.id] = newItem;
			collection.Add(newItem);

			collection.Add(new ContentTreeViewItem
			{
				displayName = "Dependancies",
				id = id++,
				depth = depth + 1,
				icon = ContentEditorResources.Instance.DependanciesIcon,

				item = null
			});

			collection.Add(new ContentTreeViewItem
			{
				displayName = "Manifests",
				id = id++,
				depth = depth + 2,
				icon = ContentEditorResources.Instance.ManifestDependancyIcon,

				item = null
			});

			var manifestItem = BehaviourManifest.CreateFromAppDomain(AppDomain.CurrentDomain);
			var manifestTreeViewItem = new ContentTreeViewItem
			{
				displayName = "RPGCore 1.0.0",
				id = id++,
				depth = depth + 3,
				icon = ContentEditorResources.Instance.ManifestDependancyIcon,

				item = manifestItem
			};
			idToItemMapping[manifestTreeViewItem.id] = manifestTreeViewItem;
			collection.Add(manifestTreeViewItem);

			collection.Add(new ContentTreeViewItem
			{
				displayName = "Projects",
				id = id++,
				depth = depth + 2,
				icon = ContentEditorResources.Instance.ProjectDependancyIcon,

				item = null
			});

			BuildDirectory(collection, explorer.RootDirectory, depth + 1, ref id);
		}

		private void BuildDirectory(List<TreeViewItem> collection, IDirectory directory, int depth, ref int id)
		{
			foreach (var childDirectory in directory.Directories)
			{
				var newItem = new ContentTreeViewItem
				{
					displayName = childDirectory.Name,
					id = id++,
					depth = depth,
					icon = ContentEditorResources.Instance.FolderIcon,

					item = childDirectory
				};
				idToItemMapping[newItem.id] = newItem;
				collection.Add(newItem);

				BuildDirectory(collection, childDirectory, depth + 1, ref id);
			}

			foreach (var resource in directory.Resources)
			{
				var newItem = new ContentTreeViewItem
				{
					displayName = resource.Name,
					id = id++,
					depth = depth,
					icon = ContentEditorResources.Instance.DocumentIcon,

					item = resource
				};
				idToItemMapping[newItem.id] = newItem;
				collection.Add(newItem);
			}
		}
	}
}
