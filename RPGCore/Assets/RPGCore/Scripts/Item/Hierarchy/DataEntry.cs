using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace RPGCore
{
	[Serializable]
	public class DataEntry : ISerializationCallbackReceiver
	{
		private static BinaryFormatter Formatter = new BinaryFormatter ();

		[SerializeField] private byte[] data = null;
		[NonSerialized] private object TargetObject = null;

		public void SetValue<T> (T value)
		{
			TargetObject = value;
		}

		public object GetValue ()
		{
			return TargetObject;
		}

		public T GetValue<T> ()
		{
			if (TargetObject != null && typeof (T).IsAssignableFrom (TargetObject.GetType ()))
			{
				return (T)TargetObject;
			}
			else
			{
				TargetObject = Activator.CreateInstance<T> ();

				return (T)TargetObject;
			}
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize ()
		{
			using (var ms = new MemoryStream ())
			{
				Formatter.Serialize (ms, TargetObject);
				data = ms.ToArray ();
			}
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize ()
		{
			using (var ms = new MemoryStream ())
			{
				ms.Write (data, 0, data.Length);
				ms.Seek (0, SeekOrigin.Begin);
				TargetObject = Formatter.Deserialize (ms);
			}
		}
	}
}