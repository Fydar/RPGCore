using RPGCore.Behaviour.Manifest;
using RPGCore.Packages;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace RPGCore.Unity.Editors
{
	public class ContentTreeViewItem : TreeViewItem
	{
		public object item;

		private string baseDisplayName;
		private string modifiedDisplayName;

		public List<FrameTab> Tabs { get; private set; }
		public List<FrameTab> VirtualChildren { get; private set; }

		public FrameTab CurrentTab;

		public bool HasUnsavedChanges
		{
			get
			{
				if (Tabs == null || Tabs.Count == 0)
				{
					return false;
				}
				foreach (var tab in Tabs)
				{
					if (tab.Frame.HasUnsavedChanges)
					{
						return true;
					}
				}
				return false;
			}
		}

		public override string displayName
		{
			get
			{
				return HasUnsavedChanges
					? modifiedDisplayName
					: baseDisplayName;
			}
			set
			{
				baseDisplayName = value;
				modifiedDisplayName = value + "*";
			}
		}

		public void Open(EditorWindow window)
		{
			if (Tabs == null)
			{
				Tabs = new List<FrameTab>();
				if (item is IResource resource)
				{
					if (resource.Extension == ".json")
					{
						try
						{
							var childFrame = new FrameTab()
							{
								Title = new GUIContent("Editor"),
								Frame = new EditorSessionFrame(resource)
								{
									EditorContext = this
								}
							};
							Tabs.Add(childFrame);
						}
						catch (Exception exception)
						{
							Debug.LogError(exception.ToString());
						}
					}

					var informationFrame = new FrameTab()
					{
						Title = new GUIContent("Information"),
						Frame = new ResourceInformationFrame(resource)
					};
					Tabs.Add(informationFrame);
				}
				else if (item is BehaviourManifest manifest)
				{
					var manifestFrame = new FrameTab()
					{
						Title = new GUIContent("Manifest"),
						Frame = new ManifestInspectFrame(manifest)
					};
					Tabs.Add(manifestFrame);
				}
				else if (item is ProjectExplorer projectExplorer)
				{
					var manifestFrame = new FrameTab()
					{
						Title = new GUIContent("Project"),
						Frame = new ProjectInspectFrame(projectExplorer)
					};
					Tabs.Add(manifestFrame);
				}

				VirtualChildren = new List<FrameTab>();
				foreach (var tab in Tabs)
				{
					foreach (var childTab in tab.Frame.SpawnChildren())
					{
						childTab.Parent = tab;
						childTab.Frame.Window = window;

						VirtualChildren.Add(childTab);
					}
				}
				
				if (Tabs.Count != 0)
				{
					CurrentTab = Tabs[0];
				}
			}
		}

		public void Unfocus()
		{

		}
	}
}
