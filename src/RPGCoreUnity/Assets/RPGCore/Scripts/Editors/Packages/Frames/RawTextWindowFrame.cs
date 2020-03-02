using RPGCore.Packages;
using System;
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

		private GUIStyle lineNumberStyle;
		private GUIStyle textStyle;

		public RawTextWindowFrame(IResource resource)
		{
			try
			{
				lines = BuildLines(resource);
			}
			catch (Exception exception)
			{
				Debug.LogError(exception);
			}
		}

		protected virtual string[] BuildLines(IResource resource)
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
			return linesList.ToArray();
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
					},
					alignment = TextAnchor.UpperLeft
				};
				lineNumberStyle = new GUIStyle(textStyle)
				{
					padding = new RectOffset()
					{
						top = 4,
						bottom = 4,
						left = 8,
						right = 6
					},
					alignment = TextAnchor.UpperRight
				};
			}

			if (lines == null)
			{
				return;
			}

			string guideString = new string('0', lines.Length.ToString().Length);

			lineNumberStyle.CalcMinMaxWidth(
				new GUIContent(guideString),
				out _, out float maxWidth);

			var rect = GUILayoutUtility.GetRect(0, 480, GUILayout.ExpandWidth(true));
			var lineNumberBackgroundRect = new Rect(
				rect.x,
				rect.y,
				maxWidth,
				rect.height
			);

			EditorGUI.DrawRect(rect, new Color(0.95f, 0.95f, 0.95f));
			EditorGUI.DrawRect(lineNumberBackgroundRect, new Color(0.90f, 0.90f, 0.90f));

			foreach (var element in new UniformScrollController(rect, EditorGUIUtility.singleLineHeight, ref offset, lines.Length))
			{
				var lineNumberRect = new Rect(
					element.Position.x,
					element.Position.y,
					maxWidth,
					element.Position.height
				);
				var lineRect = new Rect(
					element.Position.x + maxWidth,
					element.Position.y,
					element.Position.width - maxWidth,
					element.Position.height
				);

				if (element.Index < lines.Length)
				{
					EditorGUI.LabelField(lineNumberRect, (element.Index + 1).ToString(), lineNumberStyle);
					EditorGUI.LabelField(lineRect, lines[element.Index], textStyle);
				}
			}
		}
	}
}
