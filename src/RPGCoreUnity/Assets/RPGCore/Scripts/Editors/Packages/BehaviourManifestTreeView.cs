using RPGCore.Behaviour.Editor;
using RPGCore.DataEditor.Manifest;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;

namespace RPGCore.Unity.Editors
{
	internal class BehaviourManifestTreeView : TreeView
	{
		public BehaviourManifest Manifest { get; private set; }

		public BehaviourManifestTreeView(TreeViewState treeViewState)
			: base(treeViewState)
		{
		}
		public void SetTarget(BehaviourManifest manifest)
		{
			if (Manifest == manifest)
			{
				return;
			}

			Manifest = manifest;
			Reload();
		}

		protected override TreeViewItem BuildRoot()
		{
			var root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };
			var collection = new List<TreeViewItem>();

			if (Manifest != null)
			{
				int id = 1;

				collection.Add(new TreeViewItem
				{
					displayName = "Nodes",
					id = id++,
					depth = 0,
					icon = ContentEditorResources.Instance.FolderIcon
				});

				foreach (var nodeType in Manifest.Types.NodeTypes)
				{
					collection.Add(new TreeViewItem
					{
						displayName = nodeType.Key,
						id = id++,
						depth = 1,
						icon = ContentEditorResources.Instance.ManifestDependancyIcon
					});

					if (nodeType.Value.Inputs != null)
					{
						foreach (var input in nodeType.Value.Inputs)
						{
							collection.Add(new TreeViewItem
							{
								displayName = input.Key,
								id = id++,
								depth = 2,
								icon = ContentEditorResources.Instance.InputIcon
							});
						}
					}

					if (nodeType.Value.Outputs != null)
					{
						foreach (var input in nodeType.Value.Outputs)
						{
							collection.Add(new TreeViewItem
							{
								displayName = input.Key,
								id = id++,
								depth = 2,
								icon = ContentEditorResources.Instance.OutputIcon
							});
						}
					}

					BuildFields(collection, nodeType.Value, 2, ref id);
				}

				collection.Add(new TreeViewItem
				{
					displayName = "Objects",
					id = id++,
					depth = 0,
					icon = ContentEditorResources.Instance.FolderIcon
				});

				foreach (var objectType in Manifest.Types.ObjectTypes)
				{
					collection.Add(new TreeViewItem
					{
						displayName = objectType.Key,
						id = id++,
						depth = 1,
						icon = ContentEditorResources.Instance.ManifestDependancyIcon
					});

					BuildFields(collection, objectType.Value, 2, ref id);
				}
			}
			else
			{
				collection.Add(new TreeViewItem
				{
					displayName = "Unable to load manifest",
					id = 1,
					depth = 0,
					icon = ContentEditorResources.Instance.DocumentIcon
				});
			}

			SetupParentsAndChildrenFromDepths(root, collection);

			return root;
		}

		private static void BuildFields(List<TreeViewItem> collection, TypeInformation type, int depth, ref int id)
		{
			if (type.Fields == null)
			{
				return;
			}
			foreach (var field in type.Fields)
			{
				collection.Add(new TreeViewItem
				{
					displayName = field.Key,
					id = id++,
					depth = depth,
					icon = ContentEditorResources.Instance.DocumentIcon
				});
			}
		}
	}
}
