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
		public Color LineNumberColor
		{
			get
			{
				return IsDarkTheme
					? new Color(0.15f, 0.15f, 0.15f)
					: new Color(0.90f, 0.90f, 0.90f);
			}
		}

		public Color BackgroundColor
		{
			get
			{
				return IsDarkTheme
					? new Color(0.1f, 0.1f, 0.1f)
					: new Color(0.95f, 0.95f, 0.95f);
			}
		}

		public Color TextDefaultColor
		{
			get
			{
				return IsDarkTheme
					? new Color(0.95f, 0.95f, 0.95f)
					: new Color(0.1f, 0.1f, 0.1f);
			}
		}

		public bool IsDarkTheme
		{
			get
			{
				return isDarkTheme;
			}
			set
			{
				isDarkTheme = value;

				lineNumberStyle = null;
				textStyle = null;

				OnThemeChanged();
			}
		}

		protected string[] lines;
		private Vector2 offset;

		private GUIStyle lineNumberStyle;
		private GUIStyle textStyle;
		private bool isDarkTheme;

		public RawTextWindowFrame()
		{

		}

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

		public RawTextWindowFrame(string[] lines)
		{
			this.lines = lines;
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

		protected virtual void OnThemeChanged()
		{

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
					alignment = TextAnchor.UpperLeft,
					normal = new GUIStyleState()
					{
						textColor = TextDefaultColor
					}
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
					alignment = TextAnchor.UpperRight,
					normal = new GUIStyleState()
					{
						textColor = TextDefaultColor
					}
				};
			}

			if (lines == null)
			{
				return;
			}

			string guideString = new string('0', Math.Max(2, lines.Length.ToString().Length));

			lineNumberStyle.CalcMinMaxWidth(
				new GUIContent(guideString),
				out _, out float maxWidth);

			var lineNumberBackgroundRect = new Rect(
				Position.x,
				Position.y,
				maxWidth,
				Position.height
			);

			var changeThemeButtonRect = new Rect(
				Position.xMax - (EditorGUIUtility.singleLineHeight * 2),
				Position.y,
				EditorGUIUtility.singleLineHeight,
				EditorGUIUtility.singleLineHeight
			);

			EditorGUI.DrawRect(Position, BackgroundColor);
			EditorGUI.DrawRect(lineNumberBackgroundRect, LineNumberColor);

			foreach (var element in new UniformScrollController(Position, EditorGUIUtility.singleLineHeight, ref offset, lines.Length))
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

			if (GUI.Button(changeThemeButtonRect, new GUIContent(">")))
			{
				IsDarkTheme = !IsDarkTheme;
			}
		}
	}
}
