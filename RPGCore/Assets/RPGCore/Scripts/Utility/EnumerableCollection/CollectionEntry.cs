using RPGCore.Inventories;
using System;
using UnityEngine;

namespace RPGCore
{
	[Serializable]
	public class StatEntry : CollectionEntry
	{
		public override Type IndexType
		{
			get
			{
				return typeof (StatCollection<>);
			}
		}
	}

	[Serializable]
	public class WeaponStatEntry : CollectionEntry
	{
		public override Type IndexType
		{
			get
			{
				return typeof (WeaponStatCollection<>);
			}
		}
	}

	[Serializable]
	public class ArmourStatEntry : CollectionEntry
	{
		public override Type IndexType
		{
			get
			{
				return typeof (ArmourStatCollection<>);
			}
		}
	}

	[Serializable]
	public class EquipmentEntry : CollectionEntry
	{
		public override Type IndexType
		{
			get
			{
				return typeof (EquipmentCollection<>);
			}
		}
	}

	[Serializable]
	public class StateEntry : CollectionEntry
	{
		public override Type IndexType
		{
			get
			{
				return typeof (StateCollection<>);
			}
		}
	}

	public abstract class CollectionEntry
	{
		[SerializeField]
		protected string field = "";
		[NonSerialized]
		public int entryIndex = -1;

		public abstract Type IndexType { get; }

		public string Field
		{
			get
			{
				return field;
			}
			set
			{
				field = value;
				entryIndex = -1;
			}
		}

		public int Index
		{
			get
			{
				if (entryIndex == -1)
				{
					var collection = EnumerableCollection.GetReflectionInformation (IndexType);

					entryIndex = collection.IndexOf (field);
				}

				return entryIndex;
			}
		}

		public override string ToString ()
		{
			return Field;
		}
	}
}
