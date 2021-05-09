using RPGCore.DataEditor.Manifest;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.DataEditor
{
	/// <summary>
	/// An editable data structure with indexed elements.
	/// </summary>
	public class EditorList : IEditorValue
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly List<string> comments;

		/// <inheritdoc/>
		public IList<string> Comments => comments;

		/// <inheritdoc/>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public EditorSession Session { get; }

		/// <summary>
		/// The type of all elements contained within this <see cref="EditorList"/>.
		/// </summary>
		public TypeName ElementType { get; }

		/// <summary>
		/// All elements contained within this <see cref="EditorList"/>.
		/// </summary>
		public List<IEditorValue> Elements { get; }

		internal EditorList(EditorSession session, TypeName elementType)
		{
			Session = session;
			ElementType = elementType;

			Elements = new List<IEditorValue>();
			comments = new List<string>();
		}

		/// <summary>
		/// Sets the size of the array; creates new array elements to match the new size of the array.
		/// </summary>
		/// <param name="size"></param>
		/// <param name="insertNulls"></param>
		public void SetArraySize(int size, bool insertNulls = true)
		{
			if (size < 0)
			{
				throw new IndexOutOfRangeException("Cannot set array size to a value that is smaller than 0.");
			}

			while (Elements.Count > size)
			{
				Elements.RemoveAt(Elements.Count - 1);
			}
			while (Elements.Count < size)
			{
				if (insertNulls)
				{
					Elements.Add(Session.CreateDefaultValue(ElementType));
				}
				else
				{
					Elements.Add(Session.CreateInstatedValue(ElementType));
				}
			}
		}

		/// <summary>
		/// Removes an element from the array.
		/// </summary>
		/// <param name="index"></param>
		public void RemoveAt(int index)
		{
			if (index < 0)
			{
				throw new IndexOutOfRangeException("Cannot remove an array element at index less than 0.");
			}

			Elements.RemoveAt(index);
		}

		/// <summary>
		/// Adds a default element to the end of the array.
		/// </summary>
		/// <param name="insertNulls"></param>
		public void Add(bool insertNulls = true)
		{
			if (insertNulls)
			{
				Elements.Add(Session.CreateDefaultValue(ElementType));
			}
			else
			{
				Elements.Add(Session.CreateInstatedValue(ElementType));
			}
		}

		/// <summary>
		/// Adds an element to the end of the array.
		/// </summary>
		/// <param name="value"></param>
		public void Add(IEditorValue value)
		{
			if (!Session.IsValueOfType(value, ElementType))
			{
				throw new InvalidOperationException($"Cannot add {value} to {nameof(EditorList)} because it cannot be assigned to the element type \"{ElementType}\".");
			}

			Elements.Add(value);
		}

		/// <summary>
		/// Inserts an element to the end of the array.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		public void Insert(int index, IEditorValue value)
		{
			if (index < 0)
			{
				throw new IndexOutOfRangeException("Cannot remove an array element at index less than 0.");
			}

			if (!Session.IsValueOfType(value, ElementType))
			{
				throw new InvalidOperationException($"Cannot insert {value} at index {index} to {nameof(EditorList)} because it cannot be assigned to the element type \"{ElementType}\".");
			}

			Elements.Insert(index, value);
		}
	}
}
