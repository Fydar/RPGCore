using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGCore.Behaviour;
using RPGCore.Behaviour.Editor;
using RPGCore.Behaviour.Manifest;
using RPGCore.Demo.BoardGame.Models;
using RPGCore.Demo.Inventory.Nodes;
using RPGCore.Packages;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace RPGCore.Unity.Editors
{
	public class ContentEditorFrame : WindowFrame
	{
		private ProjectImport CurrentPackage;

		[SerializeField] private TreeViewState resourceTreeViewState;
		private ResourceTreeView resourceTreeView;

		private ProjectResource CurrentResource;
		private EditorSession EditorSession;

		private readonly JsonSerializer Serializer = JsonSerializer.Create(new JsonSerializerSettings()
		{
			Converters = new List<JsonConverter>()
			{
				new LocalPropertyIdJsonConverter()
			}
		});

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
						if (CurrentResource != resource)
						{
							CurrentResource = resource;

							CurrentResource = resource;
							JObject editorTarget;
							using (var editorTargetData = CurrentResource.LoadStream())
							using (var sr = new StreamReader(editorTargetData))
							using (var reader = new JsonTextReader(sr))
							{
								editorTarget = JObject.Load(reader);
							}

							var nodes = NodeManifest.Construct(new Type[] {
									typeof (AddNode),
									typeof (RollNode),
									typeof (OutputValueNode),
									typeof (ItemInputNode),
									typeof (ActivatableItemNode),
									typeof (IterateNode),
									typeof (GetStatNode),
								});
							var types = TypeManifest.Construct(
								new Type[]
								{
									typeof(bool),
									typeof(string),
									typeof(int),
									typeof(byte),
									typeof(long),
									typeof(short),
									typeof(uint),
									typeof(ulong),
									typeof(ushort),
									typeof(sbyte),
									typeof(char),
									typeof(float),
									typeof(double),
									typeof(decimal),
									typeof(InputSocket),
									typeof(LocalId),
								},
								new Type[]
								{
									typeof(SerializedGraph),
									typeof(SerializedNode),
									typeof(PackageNodeEditor),
									typeof(PackageNodePosition),
									typeof(ExtraData),

									typeof(ResourceTemplate),
									typeof(BuildingTemplate),
								}
							);

							var manifest = new BehaviourManifest()
							{
								Nodes = nodes,
								Types = types,
							};
							Debug.Log(editorTarget);
							EditorSession = new EditorSession(manifest, editorTarget, "SerializedGraph", Serializer);
						}

						DrawResourceDialogue();
					}
				}

				GUILayout.EndArea();
			}
		}

		private void DrawResourceDialogue()
		{
			EditorGUILayout.LabelField(CurrentResource.FullName, string.Join(", ", CurrentResource.Tags));

			RPGCoreEditor.DrawEditor(EditorSession);
		}
	}
}
