using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGCore.Behaviour;
using RPGCore.Behaviour.Editor;
using RPGCore.Behaviour.Manifest;
using RPGCore.Demo.Inventory.Nodes;
using RPGCore.Packages;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace RPGCore.Unity.Editors
{
	public class BehaviourEditor : EditorWindow
	{
		private BehaviourGraphFrame GraphFrame;

		private ProjectImport CurrentPackage;
		private ProjectResource CurrentResource;
		private readonly JsonSerializer Serializer = JsonSerializer.Create(new JsonSerializerSettings()
		{
			Converters = new List<JsonConverter>()
			{
				new LocalPropertyIdJsonConverter()
			}
		});

		[MenuItem("Window/Behaviour")]
		public static void Open()
		{
			var window = GetWindow<BehaviourEditor>();

			window.Show();
		}

		public static void Open(EditorSession session, EditorField graphField)
		{
			var window = GetWindow<BehaviourEditor>();

			window.Show();

			window.GraphFrame = new BehaviourGraphFrame();
			window.GraphFrame.View = new BehaviourEditorView();
			window.GraphFrame.View.BeginSession(session, graphField);

			window.GraphFrame.Window = window;
			window.GraphFrame.OnEnable();
		}

		private void OnEnable()
		{
			if (EditorGUIUtility.isProSkin)
			{
				titleContent = new GUIContent("Behaviour", BehaviourGraphResources.Instance.DarkThemeIcon);
			}
			else
			{
				titleContent = new GUIContent("Behaviour", BehaviourGraphResources.Instance.LightThemeIcon);
			}

			if (GraphFrame == null)
			{
				GraphFrame = new BehaviourGraphFrame();
			}
			GraphFrame.Window = this;
			GraphFrame.OnEnable();
		}

		private void OnGUI()
		{
			DrawTopBar();

			GraphFrame.Position = new Rect(0, EditorGUIUtility.singleLineHeight + 1,
				position.width, position.height - (EditorGUIUtility.singleLineHeight + 1));

			GraphFrame.OnGUI();
		}

		private void DrawTopBar()
		{
			EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.ExpandWidth(true));

			DrawAssetSelection();

			GUILayout.Space(6);

			if (GUILayout.Button("Save", EditorStyles.toolbarButton, GUILayout.Width(100)))
			{
				using (var file = CurrentResource.WriteStream())
				{
					Serializer.Serialize(
						new JsonTextWriter(file)
						{
							Formatting = Formatting.Indented
						},
						GraphFrame.View.Session.Instance
					);
				}
			}

			if (GUILayout.Button(GraphFrame.View?.DescribeCurrentAction, EditorStyles.toolbarButton, GUILayout.Width(120)))
			{
			}

			EditorGUILayout.EndHorizontal();
		}

		private void DrawAssetSelection()
		{
			CurrentPackage = (ProjectImport)EditorGUILayout.ObjectField(CurrentPackage, typeof(ProjectImport), true,
				GUILayout.Width(180));
			if (CurrentPackage == null)
			{
				return;
			}

			var explorer = CurrentPackage.Explorer;

			foreach (var resource in explorer.Resources)
			{
				if (!resource.Name.EndsWith(".bhvr"))
				{
					continue;
				}

				if (GUILayout.Button(resource.ToString()))
				{
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
							typeof(ExtraData)
						}
					);

					var manifest = new BehaviourManifest()
					{
						Nodes = nodes,
						Types = types,
					};
					Debug.Log(editorTarget);
					var graphEditor = new EditorSession(manifest, editorTarget, "SerializedGraph", Serializer);

					GraphFrame.View.BeginSession(graphEditor, graphEditor.Root);
				}
			}
		}
	}
}
