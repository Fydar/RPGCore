using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
using RPGCore.Utility.Editors;
#endif

namespace RPGCore.Behaviour
{
	public abstract class BehaviourNode : ScriptableObject, ISerializationCallbackReceiver
	{
		private static FieldInfo[] collectionFields;

		[Header ("Node")]
		public Vector2 Position;

		[NonSerialized]
		public Rect lastDrawRect;

		[NonSerialized]
		private InputSocket[] inputSockets;
		[NonSerialized]
		private OutputSocket[] outputSockets;

		public InputSocket[] InputSockets
		{
			get
			{
				if (inputSockets == null)
					inputSockets = FindAllSockets<InputSocket> ();

				return inputSockets;
			}
		}

		public OutputSocket[] OutputSockets
		{
			get
			{
				if (outputSockets == null)
					outputSockets = FindAllSockets<OutputSocket> ();

				return outputSockets;
			}
		}

		public FieldInfo[] CollectionFields
		{
			get
			{
				//if (collectionFields == null)
				{
					collectionFields = GetType ().GetFields (
						BindingFlags.Instance | BindingFlags.Public |
						BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
				}

				return collectionFields;
			}
		}

		public void SetupContext (IBehaviourContext context)
		{
			OnSetup (context);
		}

		public void RemoveContext (IBehaviourContext context)
		{
			OnRemove (context);
		}

		protected abstract void OnSetup (IBehaviourContext context);
		protected abstract void OnRemove (IBehaviourContext context);

		public OutputSocket GetOutput (string id)
		{
			string[] paths = id.Split ('.');
			object currentTarget = this;

			for (int i = 0; i < paths.Length; i++)
			{
				string path = paths[i];

				FieldInfo field = currentTarget.GetType ().GetField (path);

				if (field == null)
					return null;

				currentTarget = field.GetValue (currentTarget);

				if (currentTarget == null)
					return null;
			}

			if (!typeof (OutputSocket).IsAssignableFrom (currentTarget.GetType ()))
				return null;

			return (OutputSocket)currentTarget;
		}

		private T[] FindAllSockets<T> ()
			where T : Socket
		{
			List<T> foundObjects = new List<T> ();
			Stack<string> socketPath = new Stack<string> ();

			for (int i = 0; i < CollectionFields.Length; i++)
			{
				FieldInfo targetField = CollectionFields[i];
				socketPath.Push (targetField.Name);

				if (typeof (T).IsAssignableFrom (targetField.FieldType))
				{
					T childSocket = (T)targetField.GetValue (this);
					if (childSocket == null)
					{
						childSocket = (T)Activator.CreateInstance (targetField.FieldType);
						targetField.SetValue (this, childSocket);
					}
					childSocket.SocketPath = string.Join (".", socketPath.Reverse ());
					childSocket.ParentNode = this;
					foundObjects.Add (childSocket);
				}
				else if (typeof (EnumerableCollection).IsAssignableFrom (targetField.FieldType))
				{
					if (!typeof (T).IsAssignableFrom (targetField.FieldType.BaseType.GetGenericArguments ()[0]))
						continue;

					EnumerableCollection enumer = (EnumerableCollection)targetField.GetValue (this);

					if (enumer == null)
						continue;

					foreach (var child in enumer.FindAllRoutes ())
					{
						object childObject = child.Member.GetValue (child.Target);

						if (childObject == null)
						{
							childObject = (T)Activator.CreateInstance (targetField.FieldType);
							targetField.SetValue (this, childObject);
						}

						if (typeof (T).IsAssignableFrom (childObject.GetType ()))
						{
							T childSocket = (T)childObject;

							socketPath.Push (child.Member.Name);
							childSocket.SocketPath = string.Join (".", socketPath.Reverse ());
							socketPath.Pop ();

							childSocket.ParentNode = this;
							foundObjects.Add (childSocket);
						}
					}
				}
				else if (typeof (IEnumerable<T>).IsAssignableFrom (targetField.FieldType))
				{
					IEnumerable<T> childCollection = (IEnumerable<T>)targetField.GetValue (this);
					if (childCollection == null)
						continue;

					foreach (var childSocket in childCollection)
					{
						if (childSocket == null)
							continue;

						childSocket.SocketPath = string.Join (".", socketPath.Reverse ());
						childSocket.ParentNode = this;
						foundObjects.Add (childSocket);
					}
				}
				socketPath.Pop ();
			}

			return foundObjects.ToArray ();
		}

		public void OnBeforeSerialize () { }

		public void OnAfterDeserialize ()
		{
			var tempA = InputSockets;
			var tempB = OutputSockets;
		}

#if UNITY_EDITOR
		public virtual Vector2 GetDiamentions ()
		{
			SerializedObject serializedObject = SerializedObjectPool.Grab (this);

			float height = 0.0f;
			SerializedProperty iterator = serializedObject.GetIterator ();
			iterator.Next (true);
			iterator.NextVisible (false);
			iterator.NextVisible (false);

			while (iterator.NextVisible (false))
			{
				height += EditorGUI.GetPropertyHeight (iterator, true) + EditorGUIUtility.standardVerticalSpacing;
			}

			return new Vector2 (200, height);
		}

		public virtual void DrawGUI (SerializedObject serializedObject, Rect position)
		{
			Rect marchingRect = new Rect (position);

			SerializedProperty iterator = serializedObject.GetIterator ();
			iterator.Next (true);
			iterator.NextVisible (false);
			iterator.NextVisible (false);

			while (iterator.NextVisible (false))
			{
				marchingRect.height = EditorGUI.GetPropertyHeight (iterator);
				EditorGUI.PropertyField (marchingRect, iterator, true);
				marchingRect.y += marchingRect.height + EditorGUIUtility.standardVerticalSpacing;
			}
		}
#endif
	}
}
