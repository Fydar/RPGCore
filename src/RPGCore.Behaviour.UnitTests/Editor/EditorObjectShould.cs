using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RPGCore.Behaviour.Manifest;
using System;
using System.Collections.Generic;

namespace RPGCore.Behaviour.Editor.UnitTests
{
	public class EditorObjectShould
	{
		public class RootModel
		{

			public string FirstValue;
			public ChildModel AChild;
			public ChildModel BChild;
			public ChildModel[] Children;
			public Dictionary<string, ChildModel> MoreChildren;
		}

		public class ChildModel
		{
			public int ChildFirstValue;
			public bool ChildSecondValue;
		}

		[Test]
		public void Work()
		{
			var types = TypeManifest.Construct (
				new Type[]
				{
					typeof(int),
					typeof(bool),
					typeof(string),
				},
				new Type[]
				{
					typeof(RootModel),
					typeof(ChildModel)
				}
			);

			var manifest = new BehaviourManifest ()
			{
				Nodes = null,
				Types = types
			};

			var sourceObject = new RootModel ()
			{
				FirstValue = "Lol",
				AChild = new ChildModel ()
				{

				},
				BChild = null,
				Children = new ChildModel[]
				{
					new ChildModel(),
					null
				},
				MoreChildren = new Dictionary<string, ChildModel> ()
				{
					["alpha"] = new ChildModel ()
					{

					},
					["beta"] = null
				}
			};

			var editorSession = new EditorSession (manifest, sourceObject, new JsonSerializer ());

			DrawTree (editorSession.Root);

			editorSession.Root["BChild"].SetValue (new ChildModel ());
			editorSession.Root["BChild"].ApplyModifiedProperties ();

			DrawTree (editorSession.Root);
		}

		private static void DrawTree(EditorField root, int indent = 0)
		{
			TestContext.Error.WriteLine (new string (' ', indent * 3) + root.Name);
			foreach (var child in root)
			{
				DrawTree (child, indent + 1);
			}
		}
	}
}
