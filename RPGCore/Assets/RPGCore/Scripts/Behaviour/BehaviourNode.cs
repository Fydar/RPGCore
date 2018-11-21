using System;
using System.Collections.Generic;
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
		private static FieldInfo[] collectionFields = null;

		[Header ("Node")]
		public Vector2 Position;

		[NonSerialized]
		public Rect LastRect;

		[NonSerialized]
		private InputSocket[] inputs = null;
		[NonSerialized]
		private OutputSocket[] outputs = null;

		public InputSocket[] Inputs
		{
			get
			{
				if (inputs == null)
					inputs = FindAllSockets<InputSocket> ();

				return inputs;
			}
		}

		public OutputSocket[] Outputs
		{
			get
			{
				if (outputs == null)
					outputs = FindAllSockets<OutputSocket> ();

				return outputs;
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


		public void SetupContext (IBehaviourContext character)
		{
			OnSetup (character);
		}

		public void RemoveContext (IBehaviourContext character)
		{
			OnRemove (character);
		}

		protected abstract void OnSetup (IBehaviourContext character);
		protected abstract void OnRemove (IBehaviourContext character);

		public OutputSocket GetOutput (string id)
		{
			FieldInfo field = GetType ().GetField (id);

			if (field == null)
				return null;

			object fieldData = field.GetValue (this);

			if (fieldData == null)
				return null;

			try
			{
				return (OutputSocket)fieldData;
			}
			catch
			{
				return null;
			}
		}

		private T[] FindAllSockets<T> ()
			where T : Socket
		{
			List<T> foundObjects = new List<T> ();

			for (int i = 0; i < CollectionFields.Length; i++)
			{
				FieldInfo targetField = CollectionFields[i];

				if (typeof (T).IsAssignableFrom (targetField.FieldType))
				{
					T childSocket = (T)targetField.GetValue (this);
					if (childSocket == null)
					{
						childSocket = (T)Activator.CreateInstance (targetField.FieldType);
						targetField.SetValue (this, childSocket);
					}
					childSocket.SocketName = targetField.Name;
					childSocket.ParentNode = this;
					foundObjects.Add (childSocket);
				}
				else if (typeof (IEnumerable<T>).IsAssignableFrom (targetField.FieldType))
				{
					IEnumerable<T> childCollection = (IEnumerable<T>)targetField.GetValue (this);
					if (childCollection == null)
					{
						if (childCollection == null)
							continue;
					}
					foreach (var childSocket in childCollection)
					{
						if (childSocket == null)
							continue;

						childSocket.SocketName = targetField.Name;
						childSocket.ParentNode = this;
						foundObjects.Add (childSocket);
					}
				}
				else if (typeof (EnumerableCollection<T>).IsAssignableFrom (targetField.FieldType))
				{
					if (!typeof (T).IsAssignableFrom (targetField.FieldType.BaseType.GetGenericArguments ()[0]))
						continue;

					EnumerableCollection<T> enumer = (EnumerableCollection<T>)targetField.GetValue (this);

					if (enumer == null)
						continue;

					foreach (object child in enumer)
					{
						if (child == null)
							continue;

						if (typeof (T).IsAssignableFrom (child.GetType ()))
						{
							T childSocket = (T)child;

							if (childSocket == null)
							{
								childSocket = (T)Activator.CreateInstance (targetField.FieldType);
								targetField.SetValue (this, childSocket);
							}

							childSocket.SocketName = targetField.Name;
							childSocket.ParentNode = this;
							foundObjects.Add (childSocket);
						}
					}
				}
			}

			return foundObjects.ToArray ();
		}


		void ISerializationCallbackReceiver.OnBeforeSerialize () { }

		void ISerializationCallbackReceiver.OnAfterDeserialize ()
		{
			var tempA = Inputs;
			var tempB = Outputs;
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