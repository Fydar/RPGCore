using RPGCore.DataEditor.Manifest;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace RPGCore.DataEditor;

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
	public TypeName Type { get; }

	/// <inheritdoc/>
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public EditorSession Session { get; }

	/// <summary>
	/// All elements contained within this <see cref="EditorList"/>.
	/// </summary>
	public List<EditorReplaceable> Elements { get; }

	/// <summary>
	/// A collection of <see cref="IEditorFeature"/>s associated with this <see cref="EditorList"/>.
	/// </summary>
	public FeatureCollection<EditorList> Features { get; }

	/// <summary>
	/// The length of this <see cref="EditorList"/>.
	/// </summary>
	public int Length
	{
		get => Elements.Count;
		set => SetArraySize(value);
	}

	/// <summary>
	/// The type of all elements contained within this <see cref="EditorList"/>.
	/// </summary>
	public TypeName ElementType => Type.TemplateTypes[0];

	/// <inheritdoc/>
	public event PropertyChangedEventHandler PropertyChanged;

	/// <inheritdoc/>
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	FeatureCollection IEditorToken.Features => Features;

	internal EditorList(EditorSession session, TypeName type)
	{
		Session = session;
		Type = type;
		Features = new FeatureCollection<EditorList>(this);

		Elements = new List<EditorReplaceable>();
		comments = new List<string>();

		PropertyChanged = delegate { };
	}

	/// <summary>
	/// Sets the size of the array; creates new array elements to match the new size of the array.
	/// </summary>
	/// <param name="size">The new size of this <see cref="EditorList"/>.</param>
	/// <param name="insertNulls">If the <see cref="ElementType"/> of this <see cref="EditorList"/> allows <c>null</c> values, insert nulls in new elements.</param>
	public void SetArraySize(int size, bool insertNulls = true)
	{
		if (size < 0)
		{
			throw new ArgumentOutOfRangeException("Cannot set array size to a value that is smaller than 0.", nameof(size));
		}

		bool modified = false;
		while (Elements.Count > size)
		{
			Elements.RemoveAt(Elements.Count - 1);
			modified = true;
		}
		while (Elements.Count < size)
		{
			if (insertNulls)
			{
				Elements.Add(new EditorReplaceable(Session, ElementType, Session.CreateDefaultValue(ElementType)));
			}
			else
			{
				Elements.Add(new EditorReplaceable(Session, ElementType, Session.CreateInstatedValue(ElementType)));
			}

			modified = true;
		}
		if (modified)
		{
			InvokeOnSizeChanged();
		}
	}

	/// <summary>
	/// Removes an element from the array.
	/// </summary>
	/// <param name="index">The index of the element to remove.</param>
	public void RemoveAt(int index)
	{
		if (index < 0)
		{
			throw new ArgumentOutOfRangeException("Cannot remove an array element at index less than 0.", nameof(index));
		}

		Elements.RemoveAt(index);

		InvokeOnSizeChanged();
	}

	/// <summary>
	/// Adds a default element to the end of the array.
	/// </summary>
	/// <param name="insertNulls">If the <see cref="ElementType"/> of this <see cref="EditorList"/> allows <c>null</c> values, insert nulls in new elements.</param>
	public void Add(bool insertNulls = true)
	{
		if (insertNulls)
		{
			Elements.Add(new EditorReplaceable(Session, ElementType, Session.CreateDefaultValue(ElementType)));
		}
		else
		{
			Elements.Add(new EditorReplaceable(Session, ElementType, Session.CreateInstatedValue(ElementType)));
		}

		InvokeOnSizeChanged();
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

		Elements.Add(new EditorReplaceable(Session, ElementType, value));

		InvokeOnSizeChanged();
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
			throw new ArgumentOutOfRangeException("Cannot remove an array element at index less than 0.", nameof(index));
		}

		if (!Session.IsValueOfType(value, ElementType))
		{
			throw new InvalidOperationException($"Cannot insert {value} at index {index} to {nameof(EditorList)} because it cannot be assigned to the element type \"{ElementType}\".");
		}

		Elements.Insert(index, new EditorReplaceable(Session, ElementType, value));

		InvokeOnSizeChanged();
	}

	/// <inheritdoc/>
	public IEditorValue Duplicate()
	{
		var editorList = new EditorList(Session, Type);

		foreach (var element in Elements)
		{
			editorList.Add(element.Value.Duplicate());
		}

		return editorList;
	}

	private void InvokeOnSizeChanged()
	{
		PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Elements)));
		PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Length)));
	}
}
