/*using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RPGCore.DataEditor;
using RPGCore.DataEditor.Manifest;
using System;
using System.Collections.Generic;

namespace RPGCore.Behaviour.Editor.UnitTests
{
	[TestFixture(TestOf = typeof(EditorFile))]
	public class EditorSessionShould
	{
		[EditorType]
		public class RootModel
		{
			public string FirstValue;
			public int SecondValue;
			public ChildModel AChild;
			public ChildModel BChild;
			public ChildModel[] Children;
			public Dictionary<string, ChildModel> MoreChildren;
		}

		[EditorType]
		public class ChildModel
		{
			public int ChildFirstValue;
			public bool ChildSecondValue;
			public int IntegerValue;
		}
		[EditorType]
		public class GenericModel
		{
			public string Type;
			public JObject Data;
		}

		[Test, Parallelizable]
		public void WorkWithGenerics()
		{
			var manifest = ProjectManifest.CreateFromAppDomain(AppDomain.CurrentDomain);

			var generic = new GenericModel()
			{
				Data = JObject.FromObject(new ChildModel()),
				Type = "ChildModel"
			};

			var editorSession = new EditorSession(manifest, generic, new JsonSerializer());

			DrawTree(editorSession.Root);
		}

		[Test, Parallelizable]
		public void Work()
		{
			var manifest = ProjectManifest.CreateFromAppDomain(AppDomain.CurrentDomain);

			Console.Write(manifest.ToString());

			var sourceObject = new RootModel()
			{
				FirstValue = "Lol",
				AChild = new ChildModel()
				{
				},
				BChild = null,
				Children = new ChildModel[]
				{
					new ChildModel(),
					null
				},
				MoreChildren = new Dictionary<string, ChildModel>()
				{
					["alpha"] = new ChildModel()
					{
					},
					["beta"] = null
				}
			};

			var editorSession = new EditorSession(manifest, sourceObject, new JsonSerializer());

			DrawTree(editorSession.Root);

			var newChild = new ChildModel()
			{
				ChildFirstValue = -10,
				ChildSecondValue = true
			};

			(editorSession.Root.Fields["AChild"].Value as EditorObject).PopulateObject(newChild);
			// (editorSession.Root.Fields["AChild"].Value as EditorEditableValue).ApplyModifiedProperties();

			TestContext.Error.WriteLine("===========");
			DrawTree(editorSession.Root);
		}

		private static void DrawTree(EditorObject root, int indent = 1)
		{
			TestContext.Error.WriteLine($"{{");
			foreach (var child in root.Fields)
			{
				TestContext.Error.Write($"{new string(' ', indent * 3)}{child.Key}: ");

				DrawValue(child.Value.Value, indent + 1);
			}
			TestContext.Error.WriteLine($"{new string(' ', (indent - 1) * 3)}}}");
		}

		private static void DrawValue(IEditorValue editorValue, int indent = 1)
		{
			if (editorValue is EditorValue editableValue)
			{
				string rendered = editableValue.ToString();
				if (string.IsNullOrEmpty(rendered))
				{
					TestContext.Error.WriteLine("null");
				}
				else
				{
					TestContext.Error.WriteLine($"{editableValue}");
				}
			}
			else if (editorValue is EditorObject editorObject)
			{
				DrawTree(editorObject, indent + 1);
			}
			else if (editorValue is EditorDictionary editorDictionary)
			{
				foreach (var element in editorDictionary.KeyValuePairs)
				{
					TestContext.Error.Write($"[\"{element.Key}\"] = ");
					DrawValue(element.Value.Value, indent);
				}
			}
			else if (editorValue is EditorList editorList)
			{
				for (int i = 0; i < editorList.Elements.Count; i++)
				{
					var element = editorList.Elements[i];
					TestContext.Error.Write($"[{i}] = ");

					DrawValue(element, indent);
				}
			}
		}
	}
}
*/
