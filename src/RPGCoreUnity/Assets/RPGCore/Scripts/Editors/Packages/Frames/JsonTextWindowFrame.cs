using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGCore.Packages;
using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace RPGCore.Unity.Editors
{
	public class JsonTextWindowFrame : WindowFrame
	{
		private const string KeyColor = "#0451a5";
		private const string StringValueColor = "#a31515";
		private const string BoolValueColor = "#0000ff";
		private const string NumericValueColor = "#098658";

		private readonly string[] lines;
		private Vector2 offset;

		private GUIStyle textStyle;

		public JsonTextWindowFrame(IResource resource)
		{
			try
			{
				var serializer = new JsonSerializer();
				JObject jobject;

				using (var stream = resource.LoadStream())
				using (var reader = new StreamReader(stream))
				using (var jsonReader = new JsonTextReader(reader))
				{
					jobject = serializer.Deserialize<JObject>(jsonReader);
				}

				string allLines = jobject.ToString(Formatting.Indented);

				lines = allLines.Split(new char[] { '\n' }, System.StringSplitOptions.None);
			}
			catch (Exception exception)
			{
				Debug.LogError(exception);
			}

			if (lines == null)
			{
				return;
			}

			var sb = new StringBuilder();

			for (int i = 0; i < lines.Length; i++)
			{
				string oldLine = lines[i];

				int quoteIndex = 0;
				bool isValue = false;
				bool isToken = false;
				sb.Clear();

				foreach (char c in oldLine)
				{
					if (c == '"')
					{
						if (quoteIndex == 0)
						{
							sb.Append("<color=" + KeyColor + ">");
							sb.Append(c);
						}
						else if (quoteIndex == 1)
						{
							sb.Append(c);
							sb.Append("</color>");
						}
						else if (quoteIndex == 2)
						{
							sb.Append("<color=" + StringValueColor + ">");
							sb.Append(c);
						}
						else if (quoteIndex == 3)
						{
							sb.Append(c);
							sb.Append("</color>");
						}
						quoteIndex++;
						continue;
					}
					else if (c == ':')
					{
						isValue = true;
					}
					else if (isToken && c == ',')
					{
						sb.Append("</color>");
						isToken = false;
					}
					else if (quoteIndex == 2 && isValue && !isToken && c != '{' && c != '[' && !char.IsWhiteSpace(c))
					{
						isToken = true;

						sb.Append("<color=" + NumericValueColor + ">");
					}

					sb.Append(c);
				}

				if (isToken)
				{
					sb.Append("</color>");
				}

				lines[i] = sb.ToString();
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
