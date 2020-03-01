using RPGCore.Packages;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace RPGCore.Unity.Editors
{
	public class RawTextWindowFrame : WindowFrame
	{
		private readonly string[] lines;
		private Vector2 offset;

		private GUIStyle textStyle;

		public RawTextWindowFrame(IResource resource)
		{
			try
			{
				var linesList = new List<string>();
				using (var stream = resource.LoadStream())
				using (var reader = new StreamReader(stream))
				{
					while (!reader.EndOfStream)
					{
						linesList.Add(reader.ReadLine());
					}
				}
				lines = linesList.ToArray();
			}
			catch
			{
			}
		}

		public override void OnEnable()
		{
			
		}

		public override void OnGUI()
		{
			if (textStyle == null)
			{
				var courierNewFont = Font.CreateDynamicFontFromOSFont("Courier New", 24);
				textStyle = new GUIStyle(EditorStyles.label)
				{
					richText = true,
					font = courierNewFont,
					padding = new RectOffset()
					{
						top = 4,
						bottom = 4,
						left = 8
					}
				};
			}

			var rect = GUILayoutUtility.GetRect(0, 480, GUILayout.ExpandWidth(true));
			EditorGUI.DrawRect(rect, new Color(0.95f, 0.95f, 0.95f));
			foreach (var element in new UniformScrollController(rect, EditorGUIUtility.singleLineHeight, ref offset, lines.Length))
			{
				var lineNumberRect = new Rect(
					element.Position.x,
					element.Position.y,
					25,
					element.Position.height
				);
				var lineRect = new Rect(
					element.Position.x + 25,
					element.Position.y,
					element.Position.width - 25,
					element.Position.height
				);

				if (element.Index < lines.Length)
				{
					EditorGUI.LabelField(lineNumberRect, (element.Index + 1).ToString(), textStyle);
					EditorGUI.LabelField(lineRect, lines[element.Index], textStyle);
				}
			}
		}
	}
}
