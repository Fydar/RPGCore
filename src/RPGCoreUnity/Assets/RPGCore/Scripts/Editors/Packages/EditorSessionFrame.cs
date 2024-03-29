﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGCore.Behaviour;
using RPGCore.DataEditor;
using RPGCore.DataEditor.Manifest;
using RPGCore.Packages;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace RPGCoreUnity.Editors
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
					frame.View.BeginSession(field.Session, field.Value as EditorObject);

					yield return new FrameTab()
					{
						Title = new GUIContent(field.Field.Name),
						Frame = frame,
					};
				}
			}
		}

		private IEnumerable<EditorField> AllFields(IEditorValue root)
		{
			if (root is EditorObject editorObject)
			{
				foreach (var field in editorObject.Fields)
				{
					yield return field.Value;

					foreach (var childField in AllFields(field.Value.Value))
					{
						yield return childField;
					}
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

			var feature = EditorSession.GetOrCreateFeature<FramedEditorSessionFeature>();
			feature.Frame = this;
		}

		public EditorSessionFrame(IResource resource)
		{
			Resource = resource;

			JObject editorTarget;
			using (var editorTargetData = Resource.Content.LoadStream())
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
			else if (Resource.Tags.Contains("type-project"))
			{
				typeName = "ProjectModel";
			}
			else if (Resource.Tags.Contains("type-skill"))
			{
				typeName = "SkillModel";
			}
			else if (Resource.Tags.Contains("type-category"))
			{
				typeName = "ProjectCategoryModel";
			}
			else if (Resource.Tags.Contains("type-education"))
			{
				typeName = "EducationalInstitutionModel";
			}
			else if (Resource.Tags.Contains("type-company"))
			{
				typeName = "CompanyModel";
			}
			else if (Resource.Tags.Contains("type-category"))
			{
				typeName = "ProjectCategoryModel";
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

			var feature = EditorSession.GetOrCreateFeature<FramedEditorSessionFeature>();
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
					using (var file = projectResource.Content.OpenWrite())
					using (var sr = new StreamWriter(file))
					using (var jsonWriter = new JsonTextWriter(sr)
					{
						Formatting = Formatting.Indented
					})
					{
						serializer.Serialize(jsonWriter, EditorSession.Instance);
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
