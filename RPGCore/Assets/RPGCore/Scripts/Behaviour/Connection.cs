using UnityEngine;
using System;
using System.Collections.Generic;
using RPGCore.Behaviour.Editor;

#if UNITY_EDITOR
using UnityEditor;
using RPGCore.Utility.Editors;
#endif

namespace RPGCore.Behaviour
{
#if UNITY_EDITOR
	[CustomPropertyDrawer (typeof (InputSocket), true)]
	public class InputDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			SerializedProperty sourceProperty = property.FindPropertyRelative ("SourceNode");

			if (sourceProperty.objectReferenceValue != null)
			{
				return EditorGUIUtility.singleLineHeight;
			}
			else
			{
				SerializedProperty valueProperty = property.FindPropertyRelative ("defaultValue");

				if (valueProperty == null)
					return EditorGUIUtility.singleLineHeight;

				return EditorGUI.GetPropertyHeight (valueProperty);
			}
		}

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			SerializedProperty sourceProperty = property.FindPropertyRelative ("SourceNode");

			if (sourceProperty.objectReferenceValue != null)
			{
				Rect inputNoteRect = EditorGUI.PrefixLabel (position, label);
				if (label != GUIContent.none)
					EditorGUI.LabelField (inputNoteRect, "Input", EditorStyles.centeredGreyMiniLabel);
			}
			else
			{
				SerializedProperty valueProperty = property.FindPropertyRelative ("defaultValue");

				if (valueProperty != null)
				{
					EditorGUI.BeginChangeCheck ();
					EditorGUI.PropertyField (position, valueProperty, label);
					if (EditorGUI.EndChangeCheck ())
					{
						InputSocket inputSocket = (InputSocket)PropertyUtility.GetTargetObjectOfProperty (property);
						property.serializedObject.ApplyModifiedProperties ();
						inputSocket.AfterContentChanged ();
					}
				}
			}

			property.FindPropertyRelative ("drawRect").rectValue = position;
		}
	}
	[CustomPropertyDrawer (typeof (OutputSocket), true)]
	public class OutputDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight;
		}

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			property.FindPropertyRelative ("drawRect").rectValue = position;

			EditorGUI.LabelField (position, label);
		}
	}
#endif

	public abstract class Connection<T, B, C>
		where B : Connection<T, B, C>
		where C : ConnectionEntry, ISocketConvertable<T>, ISocketType<T>, new()
	{
#if UNITY_EDITOR
		private static B instance;

		public static B Instance
		{
			get
			{
				if (instance == null)
					instance = Activator.CreateInstance<B> (); ;

				return instance;
			}
		}

		public virtual void DrawConnection (Vector3 start, Vector3 end, Vector3 startDir, Vector3 endDir)
		{
			float distance = Vector3.Distance (start, end);
			Vector3 startTan = start + (startDir * distance * 0.5f);
			Vector3 endTan = end + (endDir * distance * 0.5f);

			Color connectionColour = new Color (1.0f, 1.0f, 1.0f) * Color.Lerp (GUI.color, Color.white, 0.5f);
			Handles.DrawBezier (start, end, startTan, endTan, connectionColour,
				BehaviourGraphResources.Instance.SmallConnection, 10);
		}
#endif

		public abstract class Input : InputSocket
		{
			[SerializeField, HideInInspector]
			private T defaultValue;

			private C defaultEntry = null;
			private Dictionary<IBehaviourContext, C> contextCahce;

			public C this[IBehaviourContext context]
			{
				get
				{
					return GetEntry (context);
				}
			}

			public C GetEntry (IBehaviourContext context)
			{
				if (SourceSocket == null)
				{
					if (defaultEntry == null)
					{
						defaultEntry = new C
						{
							Value = defaultValue
						};
					}

					return defaultEntry;
				}

				if (contextCahce == null)
					contextCahce = new Dictionary<IBehaviourContext, C> ();

				C foundEntry;

				bool result = contextCahce.TryGetValue (context, out foundEntry);

				if (!result || ContextUtility.currentIndex != -1)
				{
					foundEntry = new C ();

					if (ContextUtility.currentIndex == -1)
					{
						contextCahce.Add (context, foundEntry);
					}

					if (typeof (ListOutput).IsAssignableFrom (SourceSocket.GetType ()))
					{
						ListOutput list = (ListOutput)SourceSocket;

						EntryCollection entries = list.GetEntry(context);

						// If this is a root thing
						if (ContextUtility.currentIndex == -1)
						{
							for (int i = 0; i < entries.Count; i++)
							{
								C entry = entries[i];
								int index = i;
								ContextUtility.currentIndex = index;

								ParentNode.SetupContext (context);
							}
							ContextUtility.currentIndex = -1;

							entries.OnAddEntry += (C entry) =>
							{
								ContextUtility.currentIndex = entries.Count - 1;
								ParentNode.SetupContext (context);
								ContextUtility.currentIndex = -1;
							};

							entries.OnRemoveEntry += (C entry) =>
							{
								entry.Value = default (T);

								//ContextUtility.currentIndex = entries.Count - 1;
								//ParentNode.Remove (context);
								//ContextUtility.currentIndex = -1;
							};
						}
						else
						{
							ConnectionEntry connectionEntry = entries[ContextUtility.currentIndex];

							if (!typeof (ISocketConvertable<T>).IsAssignableFrom (connectionEntry.GetType ()))
								Debug.Log (SourceSocket.GetType ().Name + " is not convertable to "
									+ GetType ().Name);

							ISocketConvertable<T> socket = (ISocketConvertable<T>)connectionEntry;

							connectionEntry.OnAfterChanged += () =>
							{
								foundEntry.Value = socket.Convert;
							};

							foundEntry.Value = socket.Convert;
						}
					}
					else
					{
						Socket sourceOutput = (Socket)SourceSocket;

						ConnectionEntry connectionEntry = sourceOutput.GetBaseEntry (context);

						if (!typeof (ISocketConvertable<T>).IsAssignableFrom (connectionEntry.GetType ()))
							Debug.Log (SourceSocket.GetType ().Name + " is not convertable to "
								+ GetType ().Name);

						ISocketConvertable<T> socket = (ISocketConvertable<T>)connectionEntry;

						connectionEntry.OnAfterChanged += () =>
						{
							foundEntry.Value = socket.Convert;
						};

						foundEntry.Value = socket.Convert;
					}
				}

				return foundEntry;
			}

			public override ConnectionEntry GetBaseEntry (IBehaviourContext context)
			{
				return GetEntry (context);
			}

			public override object GetConnectionObject (IBehaviourContext context)
			{
				Debug.Log ("Getting connection object for " + context.ToString ());
				return GetEntry (context);
			}

			public override void AfterContentChanged ()
			{
				if (defaultEntry == null)
					return;

				defaultEntry.Value = defaultValue;
			}

			public override void RemoveContext (IBehaviourContext context)
			{
				if (contextCahce == null)
					return;

				contextCahce.Remove (context);
			}

#if UNITY_EDITOR
			public override void DrawConnection (Vector3 start, Vector3 end, Vector3 startDir, Vector3 endDir)
			{
				Instance.DrawConnection (start, end, startDir, endDir);
			}
#endif
		}

		public abstract class Output : OutputSocket
		{
			private Dictionary<IBehaviourContext, C> ContextCahce;

			public C this[IBehaviourContext context]
			{
				get
				{
					return GetEntry (context);
				}
			}

			public override ConnectionEntry GetBaseEntry (IBehaviourContext context)
			{
				return GetEntry (context);
			}

			public C GetEntry (IBehaviourContext context)
			{
				if (ContextCahce == null)
					ContextCahce = new Dictionary<IBehaviourContext, C> ();

				C foundEntry;

				bool result = ContextCahce.TryGetValue (context, out foundEntry);

				if (!result)
				{
					foundEntry = new C ();

					ContextCahce.Add (context, foundEntry);
				}

				return foundEntry;
			}

			public override void RemoveContext (IBehaviourContext context)
			{
				if (ContextCahce == null)
					return;

				ContextCahce.Remove (context);
			}

#if UNITY_EDITOR
			public override void DrawConnection (Vector3 start, Vector3 end, Vector3 startDir, Vector3 endDir)
			{
				Instance.DrawConnection (start, end, startDir, endDir);
			}
#endif
		}

		public abstract class ListInput : InputSocket
		{
			[SerializeField, HideInInspector]
			private T defaultValue;

			private C defaultEntry = null;
			private Dictionary<IBehaviourContext, C> contextCahce;

			public C GetEntry (IBehaviourContext context)
			{
				C foundEntry;

				if (SourceNode == null)
				{
					if (defaultEntry == null)
					{
						defaultEntry = new C
						{
							Value = defaultValue
						};
					}

					foundEntry = defaultEntry;

					return foundEntry;
				}

				if (contextCahce == null)
					contextCahce = new Dictionary<IBehaviourContext, C> ();

				bool result = contextCahce.TryGetValue (context, out foundEntry);

				if (!result)
				{
					foundEntry = new C ();

					contextCahce.Add (context, foundEntry);

					Socket sourceOutput = (Socket)SourceSocket;

					ConnectionEntry connectionEntry = sourceOutput.GetBaseEntry (context);

					if (!typeof (ISocketConvertable<T>).IsAssignableFrom (connectionEntry.GetType ()))
						Debug.Log (SourceSocket.GetType ().Name + " is not convertable to "
							+ GetType ().Name);

					ISocketConvertable<T> socket = (ISocketConvertable<T>)connectionEntry;

					connectionEntry.OnAfterChanged += () =>
					{
						foundEntry.Value = socket.Convert;
					};
				}

				return foundEntry;
			}

			public override ConnectionEntry GetBaseEntry (IBehaviourContext context)
			{
				return GetEntry (context);
			}

			public override void AfterContentChanged ()
			{
				if (defaultEntry == null)
					return;

				defaultEntry.Value = defaultValue;
			}

			public override void RemoveContext (IBehaviourContext context)
			{
				if (contextCahce == null)
					return;

				contextCahce.Remove (context);
			}

#if UNITY_EDITOR
			public override void DrawConnection (Vector3 start, Vector3 end, Vector3 startDir, Vector3 endDir)
			{
				Instance.DrawConnection (start, end, startDir, endDir);
			}
#endif
		}

		public class EntryCollection
		{
			public Action<C> OnAddEntry;
			public Action<C> OnRemoveEntry;

			private List<C> Entries;

			public EntryCollection ()
			{
				Entries = new List<C> ();
			}

			public C this[int i]
			{
				get { return Entries[i]; }
			}

			public int Count
			{
				get
				{
					return Entries.Count;
				}
			}

			public void Add (T value)
			{
				if (Contains (value))
					Debug.LogError ("Trying to add to " + GetType ().Name + " but it already contains "
						+ value.ToString ());

				C entry = new C
				{
					Value = value
				};

				Entries.Add (entry);

				if (OnAddEntry != null)
					OnAddEntry (entry);
			}

			public void Remove (T value)
			{
				for (int i = 0; i < Entries.Count; i++)
				{
					C entry = Entries[i];

					if (entry.Convert.Equals (value))
					{
						Entries.RemoveAt (i);

						if (OnRemoveEntry != null)
							OnRemoveEntry (entry);

						return;
					}
				}
			}

			public bool Contains (T value)
			{
				for (int i = 0; i < Entries.Count; i++)
				{
					C entry = Entries[i];

					if (entry.Convert.Equals (value))
					{
						return true;
					}
				}
				return false;
			}
		}

		public abstract class ListOutput : OutputSocket
		{
			public Dictionary<IBehaviourContext, EntryCollection> ContextCahce;

			public IBehaviourContext currentContext = null;

			public EntryCollection GetEntry (IBehaviourContext context)
			{
				if (ContextCahce == null)
					ContextCahce = new Dictionary<IBehaviourContext, EntryCollection> ();

				EntryCollection foundEntry;

				bool result = ContextCahce.TryGetValue (context, out foundEntry);

				if (!result)
				{
					foundEntry = new EntryCollection ();

					ContextCahce.Add (context, foundEntry);
				}

				return foundEntry;
			}

			public override void RemoveContext (IBehaviourContext context)
			{
				if (ContextCahce == null)
					return;

				ContextCahce.Remove (context);

				EntryCollection entry = GetEntry (context);

				for (int i = 0; i < entry.Count; i++)
				{
					entry[i].Value = default (T);
					//entry.Remove (entry [i]);
				}
			}

#if UNITY_EDITOR
			public override void DrawConnection (Vector3 start, Vector3 end, Vector3 startDir, Vector3 endDir)
			{
				Instance.DrawConnection (start, end, startDir, endDir);
			}
#endif
		}
	}
}
