#if UNITY_EDITOR && !NET_2_0 && !NET_2_0_SUBSET
#define OPEN_SCRIPT
#endif

using UnityEngine;
using System;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using System.Reflection;
using System.Runtime.CompilerServices;
#endif

namespace RPGCore.Utility.InspectorLog
{
	[Serializable]
	public class InspectorLog
	{
		public struct LogItem
		{
			public readonly string content;
			public readonly LogCategory category;

#if UNITY_EDITOR && OPEN_SCRIPT
			private readonly int filePathID;
			private readonly int fileLine;

			public LogItem (string message, LogCategory logCategory, int pathHash, int line)
			{
				content = message;
				filePathID = pathHash;
				fileLine = line;
				category = logCategory;
			}
#else
		public LogItem(string message, LogCategory logCategory)
		{
			content = message;
			category = logCategory;
		}
#endif

#if UNITY_EDITOR && OPEN_SCRIPT
			public void Execute ()
			{
				string filePath = paths[filePathID];

				if (pathRemoveIndex == -1)
					pathRemoveIndex = Application.dataPath.Length - 6;

				filePath = filePath.Substring (pathRemoveIndex);

				AssetDatabase.OpenAsset (AssetDatabase.LoadAssetAtPath (filePath, typeof (MonoScript)), fileLine);
			}
#endif
		}

		public enum LogCategory
		{
			Debug,
			Info,
			Warning,
			Error,
			Send,
			Receive
		}

		private static Dictionary<int, string> paths = new Dictionary<int, string> ();
		private static int pathRemoveIndex = -1;

		public event Action<LogItem> OnLogged;

		[NonSerialized]
		public List<LogItem> FullLog = new List<LogItem> ();

		[NonSerialized] private readonly bool showIndex;
		[NonSerialized] private readonly bool expandable;

		public bool ShowIndex { get { return showIndex; } }
		public bool Expandable { get { return expandable; } }

		public int Count
		{
			get
			{
				if (FullLog == null)
					return 0;

				return FullLog.Count;
			}
		}

		public InspectorLog (bool showIndex = true, bool expandable = false)
		{
			this.showIndex = showIndex;
			this.expandable = expandable;
		}

#if UNITY_EDITOR && OPEN_SCRIPT
		public void Log (string message,
			[CallerFilePath] string sourceFilePath = "",
			[CallerLineNumber] int sourceLineNumber = 0)
		{
			Log (message, LogCategory.Debug, sourceFilePath, sourceLineNumber);
		}

		public void LogInfo (string message,
			[CallerFilePath] string sourceFilePath = "",
			[CallerLineNumber] int sourceLineNumber = 0)
		{
			Log (message, LogCategory.Info, sourceFilePath, sourceLineNumber);
		}

		public void LogWarning (string message,
			[CallerFilePath] string sourceFilePath = "",
			[CallerLineNumber] int sourceLineNumber = 0)
		{
			Log (message, LogCategory.Warning, sourceFilePath, sourceLineNumber);
		}

		public void LogError (string message,
			[CallerFilePath] string sourceFilePath = "",
			[CallerLineNumber] int sourceLineNumber = 0)
		{
			Log (message, LogCategory.Error, sourceFilePath, sourceLineNumber);
		}

		public void Log (string message, LogCategory category,
			[CallerFilePath] string sourceFilePath = "",
			[CallerLineNumber] int sourceLineNumber = 0)
		{
			int pathHash = sourceFilePath.GetHashCode ();

			if (!paths.ContainsKey (pathHash))
				paths.Add (pathHash, sourceFilePath);

			LogItem item = new LogItem (message, category, pathHash, sourceLineNumber);
			if (FullLog == null)
				FullLog = new List<LogItem> ();
			FullLog.Add (item);

			if (OnLogged != null)
				OnLogged (item);
		}
#else
		public void Log(string message)
		{
			Log (message, LogCategory.Debug);
		}

		public void LogInfo(string message)
		{
			Log (message, LogCategory.Info);
		}
	
		public void LogWarning(string message)
		{
			Log (message, LogCategory.Warning);
		}
	
		public void LogError(string message)
		{
			Log (message, LogCategory.Error);
		}

		public void Log(string message, LogCategory category)
		{
			LogItem item = new LogItem (message, category);
			FullLog.Add(item);

			if (OnLogged != null)
				OnLogged(item);
		}
#endif

		public void Clear ()
		{
			FullLog.Clear ();
		}
	}

#if UNITY_EDITOR
	[CustomPropertyDrawer (typeof (InspectorLog))]
	public class InspectorLogDrawer : PropertyDrawer
	{
		private readonly int logSize = 12;

		private int scroll = -1;
		private int selectedEntry = -1;
		private int lastLogSize = -1;

		private Rect lastPosition;
		private Rect rect_header;
		private Rect rect_scrollbar;
		private Rect rect_content;
		private Rect[] rect_logContents;

		private UnityEngine.Object logParent;
		private InspectorLog log;

		private GUIStyle logEntryStyle;
		private GUIStyle logTextStyle;
		private GUIStyle elipsisStyle;

		private Texture DebugIcon;
		private Texture InfoIcon;
		private Texture WarningIcon;
		private Texture ErrorIcon;
		private Texture ConsoleIcon;
		private Texture SendIcon;
		private Texture ReceiveIcon;

		private int scrollBuffer;

		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			if (log == null)
			{
				log = (InspectorLog)GetTargetObjectOfProperty (property);

				log.OnLogged += (InspectorLog.LogItem item) =>
				{
					if (scroll + scrollBuffer == log.FullLog.Count - logSize - 1)
					{
						scrollBuffer += 1;
					}

					EditorUtility.SetDirty (logParent);
				};
			}

			float height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			if (!log.Expandable || property.isExpanded)
			{
				int logItemsCount = log.Count;

				height += EditorGUIUtility.singleLineHeight * logSize;
			}
			return height;
		}

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			if (logEntryStyle == null)
			{
				logEntryStyle = new GUIStyle (EditorStyles.helpBox)
				{
					clipping = TextClipping.Clip,
					wordWrap = false
				};

				logTextStyle = new GUIStyle
				{
					wordWrap = false,
					clipping = TextClipping.Clip,
					font = logEntryStyle.font,
					fontSize = logEntryStyle.fontSize,
					fontStyle = logEntryStyle.fontStyle
				};
				logTextStyle.normal.textColor = logEntryStyle.normal.textColor;
				logTextStyle.contentOffset = logEntryStyle.contentOffset;
				logTextStyle.padding = logEntryStyle.padding;
				logTextStyle.richText = true;

				elipsisStyle = new GUIStyle (logTextStyle);
				elipsisStyle.padding.left = 0;
				elipsisStyle.contentOffset = new Vector2 (0, elipsisStyle.contentOffset.y);

				DebugIcon = EditorGUIUtility.IconContent ("IN-AddComponentRight", "").image;
				InfoIcon = EditorGUIUtility.IconContent ("console.infoicon.sml", "").image;
				WarningIcon = EditorGUIUtility.IconContent ("console.warnicon.sml", "").image;
				ErrorIcon = EditorGUIUtility.IconContent ("console.erroricon.sml", "").image;
				ConsoleIcon = EditorGUIUtility.IconContent ("UnityEditor.ConsoleWindow", "").image;
				SendIcon = EditorGUIUtility.IconContent ("CollabPush", "").image;
				ReceiveIcon = EditorGUIUtility.IconContent ("CollabPull", "").image;
			}
			if (position != lastPosition || logSize != lastLogSize)
				RecalculateRects (position, logSize);

			label = new GUIContent (label.text, ConsoleIcon, label.tooltip);

			Rect headerRect = new Rect (lastPosition.x, lastPosition.y, lastPosition.width, EditorGUIUtility.singleLineHeight);
			if (log.Expandable)
			{
				EditorGUI.BeginChangeCheck ();
				property.isExpanded = EditorGUI.Foldout (headerRect, property.isExpanded, label, true);
				if (EditorGUI.EndChangeCheck ())
				{
					EditorUtility.SetDirty (property.serializedObject.targetObject);
				}

				if (!property.isExpanded)
				{
					return;
				}
			}
			else
			{
				EditorGUI.LabelField (rect_header, label);
			}

			int logItemsCount = log.Count;
			int scrollMax = logItemsCount - logSize;

			Event currentEvent = Event.current;

			if (logItemsCount > logSize)
			{
				float handleSize = Mathf.Max (3 / Mathf.Sqrt (logItemsCount - logSize), 0.075f);

				if (scroll == -1)
					scroll = scrollMax;

				scroll += scrollBuffer;
				scrollBuffer = 0;

				if (currentEvent.rawType == EventType.ScrollWheel && rect_content.Contains (currentEvent.mousePosition))
				{
					if ((Math.Abs (scroll - 1.0f) < 0.01f && currentEvent.delta.y > 0) ||
						(Math.Abs (scroll) < 0.01f && currentEvent.delta.y < 0))
					{
						// Let us scroll normally, optionally we can interupt the scrolling
						currentEvent.Use ();
					}
					else
					{
						scroll += Mathf.RoundToInt (currentEvent.delta.y);
						currentEvent.Use ();
					}
				}

				float smallScroll = GUI.VerticalScrollbar (rect_scrollbar, (float)scroll / (float)scrollMax, handleSize, 0.0f, 1.0f + handleSize);
				scroll = Mathf.RoundToInt (smallScroll * scrollMax);
			}
			else
			{
				EditorGUI.BeginDisabledGroup (true);
				GUI.VerticalScrollbar (rect_scrollbar, scroll, 1.0f, 0.0f, 1.0f);
				EditorGUI.EndDisabledGroup ();

				scroll = logItemsCount;
			}

			if (logItemsCount != 0)
			{
				int scrollOffset = scroll;
				if (logItemsCount <= logSize)
					scrollOffset = 0;

				int offset = Mathf.RoundToInt (scrollOffset);

				if (currentEvent.type == EventType.MouseDown)
				{
					if (rect_content.Contains (currentEvent.mousePosition))
					{
						float posInContent = Mathf.InverseLerp (rect_content.yMin, rect_content.yMax, currentEvent.mousePosition.y);

						int clickedIndex = Mathf.FloorToInt (posInContent * logSize);
						int logIndex = clickedIndex + offset;

						if (currentEvent.clickCount == 2)
						{
							InspectorLog.LogItem item = log.FullLog[logIndex];
#if OPEN_SCRIPT
							ExecuteAction (item);
#endif

							selectedEntry = logIndex;
						}
						else
						{
							selectedEntry = logIndex;
						}

						EditorUtility.SetDirty (property.serializedObject.targetObject);
						currentEvent.Use ();
					}
					else
					{
					}
				}
				if (currentEvent.type == EventType.KeyDown)
				{
					if (currentEvent.keyCode == KeyCode.DownArrow)
					{
						if (selectedEntry != logItemsCount - 1)
							selectedEntry++;

						if (selectedEntry >= scroll + logSize)
							scroll = selectedEntry - logSize + 1;
						else if (selectedEntry < scroll)
							scroll = selectedEntry;

						EditorUtility.SetDirty (property.serializedObject.targetObject);
						currentEvent.Use ();
					}
					else if (currentEvent.keyCode == KeyCode.UpArrow)
					{
						if (selectedEntry != 0)
							selectedEntry--;

						if (selectedEntry >= scroll + logSize)
							scroll = selectedEntry - logSize + 1;
						else if (selectedEntry < scroll)
							scroll = selectedEntry;

						EditorUtility.SetDirty (property.serializedObject.targetObject);
						currentEvent.Use ();
					}
				}
				else if (currentEvent.type == EventType.Repaint)
				{
					Color originalColour = GUI.color;
					GUI.Box (rect_content, "", EditorStyles.helpBox);

					for (int i = 0; i < rect_logContents.Length; i++)
					{
						int index = i + offset;

						if (index < 0)
							continue;

						if (index >= logItemsCount)
							break;

						Rect logRect = rect_logContents[i];
						InspectorLog.LogItem item = log.FullLog[index];

						if (selectedEntry == index)
							GUI.color = new Color (0.25f, 0.45f, 1.0f, 1.0f);
						else if (index % 2 == 0)
							GUI.color = new Color (0.8f, 0.8f, 0.8f, 1.0f);
						else
							GUI.color = new Color (0.7f, 0.7f, 0.7f, 1.0f);

						string content;
						if (log.ShowIndex)
							content = index.ToString () + ": " + item.content;
						else
							content = item.content;

						logEntryStyle.Draw (logRect, false, false, false, false);

						Texture icon = null;
						switch (item.category)
						{
							case InspectorLog.LogCategory.Debug:
								icon = DebugIcon;
								break;
							case InspectorLog.LogCategory.Info:
								icon = InfoIcon;
								break;
							case InspectorLog.LogCategory.Warning:
								icon = WarningIcon;
								break;
							case InspectorLog.LogCategory.Error:
								icon = ErrorIcon;
								break;
							case InspectorLog.LogCategory.Send:
								icon = SendIcon;
								break;
							case InspectorLog.LogCategory.Receive:
								icon = ReceiveIcon;
								break;
						}

						TextCroppingField (logRect, new GUIContent (content, icon));
					}

					GUI.color = originalColour;
				}
			}
			else
			{
				GUI.Box (rect_content, "Empty", EditorStyles.helpBox);
			}
		}

		private void TextCroppingField (Rect rect, GUIContent content)
		{
			GUIContent textContent = new GUIContent (content.text);
			float width = logTextStyle.CalcSize (textContent).x;

			Rect iconRect = new Rect (rect.x, rect.y, rect.height, rect.height);
			rect = new Rect (iconRect.xMax, iconRect.y, rect.width - iconRect.width, rect.height);

			iconRect.x += logEntryStyle.padding.left * 0.5f;

			if (width > rect.width)
			{
				float elipsisWidth = elipsisStyle.CalcSize (new GUIContent ("...")).x;

				rect = new Rect (rect.x, rect.y, rect.width - elipsisWidth, rect.height);
				Rect elipsisRect = new Rect (rect.xMax, rect.y, elipsisWidth, rect.height);

				GUI.color = Color.red;
				logTextStyle.Draw (rect, textContent, false, false, false, false);
				elipsisStyle.Draw (elipsisRect, "...", false, false, false, false);
			}
			else
			{
				logTextStyle.Draw (rect, textContent, false, false, false, false);
			}

			Color originalColor = GUI.color;
			GUI.color = Color.white;
			GUI.Box (iconRect, content.image, EditorStyles.label);
			GUI.color = originalColor;
		}

		private void RecalculateRects (Rect frame, int logEntryCount)
		{
			rect_header = new Rect (frame)
			{
				height = EditorGUIUtility.singleLineHeight
			};

			rect_content = new Rect (frame);
			rect_content.y += EditorGUIUtility.singleLineHeight;
			rect_content.height -= EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			rect_scrollbar = new Rect (rect_content)
			{
				xMin = rect_content.xMax - EditorGUIUtility.singleLineHeight
			};

			rect_content.xMax -= EditorGUIUtility.singleLineHeight;

			rect_logContents = new Rect[logEntryCount];

			Rect currentLogContent = new Rect (rect_content)
			{
				height = EditorGUIUtility.singleLineHeight
			};

			for (int i = 0; i < logEntryCount; i++)
			{
				rect_logContents[i] = currentLogContent;
				currentLogContent.y += EditorGUIUtility.singleLineHeight;
			}

			lastPosition = frame;
			lastLogSize = logEntryCount;
		}

#if OPEN_SCRIPT
		private void ExecuteAction (InspectorLog.LogItem item)
		{
			item.Execute ();
		}
#endif

		private object GetTargetObjectOfProperty (SerializedProperty prop)
		{
			var path = prop.propertyPath.Replace (".Array.data[", "[");
			logParent = prop.serializedObject.targetObject;
			object obj = logParent;

			var elements = path.Split ('.');
			foreach (var element in elements)
			{
				if (element.Contains ("["))
				{
					var elementName = element.Substring (0, element.IndexOf ("[", StringComparison.Ordinal));
					var index = System.Convert.ToInt32 (element.Substring (element.IndexOf ("[", StringComparison.Ordinal)).Replace ("[", "").Replace ("]", ""));
					obj = GetValue_Imp (obj, elementName, index);
				}
				else
				{
					obj = GetValue_Imp (obj, element);
				}
			}
			return obj;
		}

		private static object GetValue_Imp (object source, string name)
		{
			if (source == null)
				return null;
			var type = source.GetType ();

			while (type != null)
			{
				var f = type.GetField (name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
				if (f != null)
					return f.GetValue (source);

				var p = type.GetProperty (name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
				if (p != null)
					return p.GetValue (source, null);

				type = type.BaseType;
			}
			return null;
		}

		private static object GetValue_Imp (object source, string name, int index)
		{
			var enumerable = GetValue_Imp (source, name) as System.Collections.IEnumerable;
			if (enumerable == null) return null;
			var enm = enumerable.GetEnumerator ();

			for (int i = 0; i <= index; i++)
			{
				if (!enm.MoveNext ()) return null;
			}
			return enm.Current;
		}
	}
#endif
}