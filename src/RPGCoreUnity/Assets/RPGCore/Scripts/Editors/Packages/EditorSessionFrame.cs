using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGCore.Behaviour;
using RPGCore.Behaviour.Editor;
using RPGCore.Behaviour.Manifest;
using RPGCore.Packages;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace RPGCore.Unity.Editors
{
	public class FramedEditorSessionFeature
	{
		public EditorSessionFrame Frame;
	}

	public class EditorSessionFrame : WindowFrame
	{
		public IResource Resource { get; private set; }
		public EditorSession EditorSession { get; private set; }
		public ContentTreeViewItem EditorContext { get; internal set; }

		private readonly JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings()
		{
			Converters = new List<JsonConverter>()
			{
				new LocalPropertyIdJsonConverter()
			}
		});

		public override IEnumerable<FrameTab> SpawnChildren()
		{
			foreach (var field in AllFields(EditorSession.Root))
			{
				if (field.Field.Type == "SerializedGraph")
				{
					var frame = new BehaviourGraphFrame
					{
						View = new BehaviourEditorView()
					};
					frame.View.BeginSession(field.Session, field);

					yield return new FrameTab()
					{
						Title = new GUIContent(field.Name),
						Frame = frame,
					};
				}
			}
		}

		private IEnumerable<EditorField> AllFields(EditorField root)
		{
			yield return root;
			foreach (var field in root)
			{
				foreach (var childField in AllFields(field))
				{
					yield return childField;
				}
			}
		}

		public EditorSessionFrame(EditorSession editorSession)
		{
			EditorSession = editorSession;

			EditorSession.OnChanged += () =>
			{
				HasUnsavedChanges = true;
			};

			var feature = EditorSession.Root.GetOrCreateFeature<FramedEditorSessionFeature>();
			feature.Frame = this;
		}

		public EditorSessionFrame(IResource resource)
		{
			Resource = resource;

			JObject editorTarget;
			using (var editorTargetData = Resource.LoadStream())
			using (var sr = new StreamReader(editorTargetData))
			using (var reader = new JsonTextReader(sr))
			{
				editorTarget = JObject.Load(reader);
			}

			var manifest = BehaviourManifest.CreateFromAppDomain(AppDomain.CurrentDomain);

			string typeName = null;
			if (Resource.Tags.Contains("type-building"))
			{
				typeName = "BuildingTemplate";
			}
			else if (Resource.Tags.Contains("type-resource"))
			{
				typeName = "ResourceTemplate";
			}
			else if (Resource.Tags.Contains("type-buildingpack"))
			{
				typeName = "BuildingPackTemplate";
			}
			else if (Resource.Tags.Contains("gamerules"))
			{
				typeName = "GameRulesTemplate";
			}
			else
			{
				typeName = "ProceduralItemTemplate";
			}

			EditorSession = new EditorSession(manifest, editorTarget, typeName, serializer);

			EditorSession.OnChanged += () =>
			{
				HasUnsavedChanges = true;
			};

			var feature = EditorSession.Root.GetOrCreateFeature<FramedEditorSessionFeature>();
			feature.Frame = this;
		}

		public override void OnEnable()
		{
		}

		public override void OnGUI()
		{
			GUILayout.BeginArea(Position);

			if (Resource is ProjectResource projectResource)
			{
				EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
				if (GUILayout.Button("Save", EditorStyles.toolbarButton, GUILayout.Width(100)))
				{
					using (var file = projectResource.WriteStream())
					{
						serializer.Serialize(
							new JsonTextWriter(file)
							{
								Formatting = Formatting.Indented
							},
							EditorSession.Instance
						);
					}

					HasUnsavedChanges = false;
				}
				EditorGUILayout.EndHorizontal();
			}

			RPGCoreEditor.DrawEditor(EditorSession);

			GUILayout.EndArea();
		}
	}
}
