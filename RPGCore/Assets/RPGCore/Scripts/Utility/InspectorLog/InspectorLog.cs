#if UNITY_EDITOR && !NET_2_0 && !NET_2_0_SUBSET
#define OPEN_SCRIPT
#endif

using UnityEngine;
using System;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using System.Reflection;
#endif

#if OPEN_SCRIPT
using System.Runtime.CompilerServices;
#endif

namespace RPGCore.Utility.InspectorLog
{
	[Serializable]
	public class InspectorLog
	{
		public struct LogItem
		{
			public string content;

#if UNITY_EDITOR && OPEN_SCRIPT
			private readonly int filePathID;
			private readonly int fileLine;

			public LogItem (ref string message, ref int pathHash, ref int line)
			{
				content = message;
				filePathID = pathHash;
				fileLine = line;
			}
#else
			public LogItem (ref string message)
		{
			content = message;
		}
#endif

			public void Execute ()
			{
#if UNITY_EDITOR && OPEN_SCRIPT
				string filePath = Paths[filePathID];

				if (pathRemoveIndex == -1)
					pathRemoveIndex = Application.dataPath.Length - 6;

				filePath = filePath.Substring (pathRemoveIndex);

				AssetDatabase.OpenAsset (AssetDatabase.LoadAssetAtPath (filePath, typeof (MonoScript)), fileLine);
#endif
			}
		}

		public event Action<LogItem> OnLogged;

#if OPEN_SCRIPT
		private static readonly Dictionary<int, string> Paths = new Dictionary<int, string> ();
		private static int pathRemoveIndex = -1;
#endif

		public bool ShowIndex { get; private set; }
		public bool Expandable { get; private set; }

		[NonSerialized]
		public List<LogItem> FullLog = new List<LogItem> ();

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
			ShowIndex = showIndex;
			Expandable = expandable;
		}

#if UNITY_EDITOR && OPEN_SCRIPT
		public void Log (string message,
			[CallerFilePath] string sourceFilePath = "",
			[CallerLineNumber] int sourceLineNumber = 0)
		{
			int pathHash = sourceFilePath.GetHashCode ();

			if (!Paths.ContainsKey (pathHash))
				Paths.Add (pathHash, sourceFilePath);

			LogItem item = new LogItem (ref message, ref pathHash, ref sourceLineNumber);

			if (FullLog == null)
				FullLog = new List<LogItem> ();

			FullLog.Add (item);

			if (OnLogged != null)
				OnLogged (item);
		}
#else
	public void Log(string message)
	{
        LogItem item = new LogItem (ref message);
		FullLog.Add(item);

		if (OnLogged != null)
			OnLogged(item);
	}
#endif
	}

#if UNITY_EDITOR
	[CustomPropertyDrawer (typeof (InspectorLog))]
	public class InspectorLogDrawer : PropertyDrawer
	{
		const int logSize = 12;

		private int scroll = -1;
		private int selectedEntry = -1;
		private int lastLogSize = -1;

		private Rect lastPosition;
		private Rect rect_header;
		private Rect rect_scrollbar;
		private Rect rect_content;
		private Rect[] rect_logContents;

		private UnityEngine.Object logParent = null;
		private InspectorLog log = null;

		private int scrollBuffer = 0;

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

			float height = EditorGUIUtility.singleLineHeight;
			if (!log.Expandable || (log.Expandable && property.isExpanded))
			{
				int logItemsCount = log.Count;

				height += EditorGUIUtility.singleLineHeight * logSize;
			}
			return height;
		}

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			if (position != lastPosition || logSize != lastLogSize)
				RecalculateRects (position, logSize);

			Rect headerRect = new Rect (lastPosition.x, lastPosition.y, lastPosition.width, EditorGUIUtility.singleLineHeight);
			if (log.Expandable)
			{
				property.isExpanded = EditorGUI.Foldout (headerRect, property.isExpanded, label, true);
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
				float handleSize = Mathf.Max (1 / Mathf.Sqrt (logItemsCount - logSize), 0.075f);

				if (scroll == -1)
					scroll = scrollMax;

				scroll += scrollBuffer;
				scrollBuffer = 0;

				if (currentEvent.rawType == EventType.ScrollWheel && rect_content.Contains (currentEvent.mousePosition))
				{
					if ((scroll == 1.0f && currentEvent.delta.y > 0) ||
						(scroll == 0.0f && currentEvent.delta.y < 0))
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
							ExecuteAction (item);

							selectedEntry = logIndex;
						}
						else
						{
							selectedEntry = logIndex;
						}

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

						if (log.ShowIndex)
							EditorStyles.helpBox.Draw (logRect, index.ToString () + ": " + item.content, false, false, false, false);
						else
							EditorStyles.helpBox.Draw (logRect, item.content, false, false, false, false);
					}

					GUI.color = originalColour;
				}
			}
			else
			{
				GUI.Box (rect_content, "Empty", EditorStyles.helpBox);
			}
		}

		private void RecalculateRects (Rect frame, int logSize)
		{
			rect_header = new Rect (frame)
			{
				height = EditorGUIUtility.singleLineHeight
			};

			rect_content = new Rect (frame);
			rect_content.y += EditorGUIUtility.singleLineHeight;
			rect_content.height -= EditorGUIUtility.singleLineHeight;

			rect_scrollbar = new Rect (rect_content)
			{
				xMin = rect_content.xMax - EditorGUIUtility.singleLineHeight
			};

			rect_content.xMax -= EditorGUIUtility.singleLineHeight;

			rect_logContents = new Rect[logSize];

			Rect currentLogContent = new Rect (rect_content)
			{
				height = EditorGUIUtility.singleLineHeight
			};

			for (int i = 0; i < logSize; i++)
			{
				rect_logContents[i] = currentLogContent;
				currentLogContent.y += EditorGUIUtility.singleLineHeight;
			}

			lastPosition = frame;
			lastLogSize = logSize;
		}

		private void ExecuteAction (InspectorLog.LogItem item)
		{
			item.Execute ();
		}

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
					var elementName = element.Substring (0, element.IndexOf ("["));
					var index = System.Convert.ToInt32 (element.Substring (element.IndexOf ("[")).Replace ("[", "").Replace ("]", ""));
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