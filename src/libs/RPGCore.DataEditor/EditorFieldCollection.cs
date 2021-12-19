using RPGCore.DataEditor.Manifest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.DataEditor
{
	/// <summary>
	/// Represents a collection of <see cref="EditorField"/>s belonging to an <see cref="EditorObject"/>.
	/// </summary>
	[DebuggerDisplay("Count = {Count,nq}")]
	public class EditorFieldCollection : IEnumerable<EditorField>
	{
		private class EditorFieldComparer : IComparer<EditorField>
		{
			private readonly SchemaType schemaType;

			public EditorFieldComparer(SchemaType schemaType)
			{
				this.schemaType = schemaType;
			}

			public int Compare(EditorField x, EditorField y)
			{
				if (schemaType.Fields == null)
				{
					return 0;
				}

				var xSchema = x.Schema;
				var ySchema = y.Schema;

				int xIndex = xSchema == null ? 0 : schemaType.Fields.IndexOf(xSchema);
				int yIndex = ySchema == null ? 0 : schemaType.Fields.IndexOf(ySchema);

				return xIndex.CompareTo(yIndex);
			}
		}

		private readonly EditorObject owner;
		private readonly List<EditorField> internalCollection;

		/// <summary>
		/// Gets the number of <see cref="EditorField"/>s contained in this <see cref="EditorFieldCollection"/>.
		/// </summary>
		/// <returns>
		/// The number of <see cref="EditorField"/>s contained in this <see cref="EditorFieldCollection"/>.
		/// </returns>
		public int Count => internalCollection.Count;

		/// <summary>
		/// Gets an <see cref="EditorField"/> at the specified <paramref name="index"/>.
		/// </summary>
		/// <param name="index">The zero-based index of the <see cref="EditorField"/> to get.</param>
		/// <returns>The <see cref="EditorField"/> at the specified <paramref name="index"/>.</returns>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than 0. -or- <paramref name="index"/> is equal to or greater than <see cref="Count"/>.</exception>
		public EditorField this[int index] => internalCollection[index];

		/// <summary>
		/// Gets an <see cref="EditorField"/> with the specified <paramref name="name"/>.
		/// </summary>
		/// <param name="name">The name of the <see cref="EditorField"/> to search for.</param>
		/// <returns>The <see cref="EditorField"/> with the specified <paramref name="name"/> If an <see cref="EditorField"/> cannot be found; returns <c>null</c>.</returns>
		public EditorField? this[string name]
		{
			get
			{
				foreach (var field in internalCollection)
				{
					if (field.Name == name)
					{
						return field;
					}
				}
				return null;
			}
		}

		internal EditorFieldCollection(EditorObject owner)
		{
			internalCollection = new List<EditorField>();
			this.owner = owner;

			SyncFieldsWithType();
		}

		/// <inheritdoc/>
		public IEnumerator<EditorField> GetEnumerator()
		{
			return internalCollection.GetEnumerator();
		}

		internal void SyncFieldsWithType()
		{
			var type = owner.Session.Manifest.GetTypeInformation(owner.Type.Identifier);
			if (type == null)
			{
				throw new InvalidOperationException($"Cannot resync fields with type {owner.Type} as it could not be resolved.");
			}

			AddMissingFields(type);

			internalCollection.Sort(new EditorFieldComparer(type));
		}

		private void AddMissingFields(SchemaType type)
		{
			if (type == null)
			{
				throw new InvalidOperationException($"Cannot determine any type information for the type \"{owner.Type}\".");
			}

			if (type.Fields == null)
			{
				throw new InvalidOperationException($"Cannot use \"{owner.Type}\" for an object as it doesn't have any fields.");
			}

			foreach (var field in type.Fields)
			{
				if (!ContainsField(field.Name))
				{
					var newField = new EditorField(owner, field.Name);

					newField.Value.Value = owner.Session.CreateDefaultValue(field.Type);
					internalCollection.Add(newField);
				}
			}
		}

		private bool ContainsField(string field)
		{
			foreach (var otherField in internalCollection)
			{
				if (otherField.Name == field)
				{
					return true;
				}
			}
			return false;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return internalCollection.GetEnumerator();
		}
	}
}
