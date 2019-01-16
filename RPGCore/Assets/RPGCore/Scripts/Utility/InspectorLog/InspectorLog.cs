#if UNITY_EDITOR && !NET_2_0 && !NET_2_0_SUBSET
#define OPEN_SCRIPT
#endif

using UnityEngine;
using System;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#if OPEN_SCRIPT
using System.Runtime.CompilerServices;
using RPGCore.Utility.Editors;
using RPGCore.Editors.Utility;
#endif
#endif

namespace RPGCore.Utility.InspectorLog
{
	[Serializable]
	public class InspectorLog
	{
		public enum LogCategory
		{
			Debug,
			Info,
			Warning,
			Error,
			Send,
			Receive
		}

		public struct LogItem
		{
			public readonly string content;
			public readonly LogCategory category;

#if OPEN_SCRIPT
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

#if OPEN_SCRIPT
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

#if OPEN_SCRIPT
		private static Dictionary<int, string> paths = new Dictionary<int, string> ();
		private static int pathRemoveIndex = -1;
#endif

		public event Action<LogItem> OnLogged;

		[NonSerialized]
		private List<LogItem> logHistory = new List<LogItem> ();

		public bool ShowIndex { get; }
		public bool Expandable { get; }

		public int Count
		{
			get
			{
				if (logHistory == null)
					return 0;

				return logHistory.Count;
			}
		}

		public LogItem this[int index]
		{
			get
			{
				return logHistory[index];
			}
		}

		public InspectorLog (bool showIndex = true, bool expandable = false)
		{
			ShowIndex = showIndex;
			Expandable = expandable;
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
			if (logHistory == null)
				logHistory = new List<LogItem> ();
			logHistory.Add (item);

			if (OnLogged != null)
				OnLogged (item);
		}
#else
        public void Log(string message)
        {
            Log(message, LogCategory.Debug);
        }

        public void LogInfo(string message)
        {
            Log(message, LogCategory.Info);
        }

        public void LogWarning(string message)
        {
            Log(message, LogCategory.Warning);
        }

        public void LogError(string message)
        {
            Log(message, LogCategory.Error);
        }

        public void Log(string message, LogCategory category)
        {
            LogItem item = new LogItem(message, category);
			if (logHistory == null)
				logHistory = new List<LogItem> ();
            logHistory.Add(item);

            if (OnLogged != null)
                OnLogged(item);
        }
#endif

		public void Clear ()
		{
			logHistory.Clear ();
		}
	}

#if UNITY_EDITOR
	[CustomPropertyDrawer (typeof (InspectorLog))]
	class InspectorLogDrawer : PropertyDrawer
	{
		private static GUIStyle logEntryStyle;
		private static GUIStyle logTextStyle;
		private static GUIStyle elipsisStyle;

		private static Texture DebugIcon;
		private static Texture InfoIcon;
		private static Texture WarningIcon;
		private static Texture ErrorIcon;
		private static Texture ConsoleIcon;
		private static Texture SendIcon;
		private static Texture ReceiveIcon;

		private readonly int logSize = 12;

		private Vector2 Offset = new Vector2 (float.MinValue, 0);
		private int SelectedIndex = -1;

		private UnityEngine.Object logParent;
		private InspectorLog log;

		public override bool CanCacheInspectorGUI (SerializedProperty property)
		{
			return false;
		}

		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			if (log == null)
			{
				log = (InspectorLog)AdvancedGUI.GetTargetObjectOfProperty (property);
				logParent = property.serializedObject.targetObject;

				log.OnLogged += (InspectorLog.LogItem item) =>
				{
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

			label = new GUIContent (label.text, ConsoleIcon, label.tooltip);

			Rect headerRect = new Rect (position)
			{
				height = EditorGUIUtility.singleLineHeight
			};
			Rect viewRect = new Rect (position)
			{
				yMin = headerRect.yMax
			};

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
				EditorGUI.LabelField (headerRect, label);
			}

			int elements = 0;
			if (log != null)
			{
				elements = log.Count;
			}
			foreach (var element in new UniformScrollController (viewRect, EditorGUIUtility.singleLineHeight, ref Offset, elements))
			{
				if (element.Index >= log.Count)
					continue;
				DrawElement (element.Index, log[element.Index], element.Position);
			}
		}

		private void DrawElement (int index, InspectorLog.LogItem item, Rect logRect)
		{
			if (Event.current.type == EventType.Repaint)
			{
				Color originalColor = GUI.color;
				if (SelectedIndex == index)
					GUI.color = new Color (0.25f, 0.45f, 1.0f, 1.0f);
				else
				if (index % 2 == 0)
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
				GUI.color = originalColor;
			}
			else if (Event.current.type == EventType.MouseDown)
			{
				if (logRect.Contains (Event.current.mousePosition))
				{
					SelectedIndex = index;
					if (Event.current.clickCount == 2)
					{
#if OPEN_SCRIPT
						ExecuteAction (item);
#endif
					}
					Event.current.Use ();
				}
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

#if OPEN_SCRIPT
		private static void ExecuteAction (InspectorLog.LogItem item)
		{
			item.Execute ();
		}
#endif
	}
#endif
}
