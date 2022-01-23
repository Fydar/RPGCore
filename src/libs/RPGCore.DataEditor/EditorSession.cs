using RPGCore.DataEditor.Files;
using RPGCore.DataEditor.Manifest;
using System;
using System.Text;

namespace RPGCore.DataEditor;

/// <summary>
/// Provides configuration for data editing.
/// </summary>
public class EditorSession
{
	/// <summary>
	/// The manifest used for configuring the data editor.
	/// </summary>
	public ProjectManifest Manifest { get; }

	/// <summary>
	/// The serializer used to load and save values.
	/// </summary>
	public IEditorSerializer Serializer { get; }

	/// <summary>
	/// Creates a new instance of the <see cref="EditorSession"/> object.
	/// </summary>
	/// <param name="manifest">A manifest used to drive data behaviour.</param>
	/// <param name="serializer">The serializer used to serialize values.</param>
	public EditorSession(ProjectManifest manifest, IEditorSerializer serializer)
	{
		Manifest = manifest;
		Serializer = serializer;
	}

	/// <summary>
	/// Starts editing a file using configuration provided by this <see cref="EditorSession"/>.
	/// </summary>
	/// <returns>A new <see cref="EditorFile"/> used for editing.</returns>
	public EditorFileBuilder EditFile()
	{
		return new EditorFileBuilder(this);
	}

	/// <summary>
	/// Gets the underlying type information from a qualified type.
	/// </summary>
	/// <param name="schemaQualifiedType"></param>
	/// <returns></returns>
	public SchemaType? ResolveType(TypeName schemaQualifiedType)
	{
		var type = Manifest.GetTypeInformation(schemaQualifiedType.Identifier);

		return type;
	}

	internal IEditorValue CreateDefaultValue(TypeName type)
	{
		return type.IsNullable
			? new EditorNull(this)
			: CreateInstatedValue(type);
	}

	internal IEditorValue CreateInstatedValue(TypeName type)
	{
		if (type.IsDictionary)
		{
			return new EditorDictionary(this, type);
		}
		else if (type.IsArray)
		{
			return new EditorList(this, type);
		}
		else
		{
			var typeInfo = Manifest.GetTypeInformation(type.Identifier);
			if (typeInfo == null)
			{
				throw new InvalidOperationException($"Cannot create an instance of an object of type \"{type.Identifier}\".");
			}

			if (string.IsNullOrEmpty(typeInfo.InstatedValue))
			{
				return new EditorNull(this);
			}
			else
			{
				byte[]? data = Encoding.UTF8.GetBytes(typeInfo.InstatedValue);
				var scalarInnerValue = Serializer.DeserializeValue(this, type, new ArraySegment<byte>(data));
				return scalarInnerValue;
			}
		}
	}

	internal bool IsTypeSubtypeOf(TypeName subType, TypeName baseType)
	{
		return true;
	}

	internal bool IsValueOfType(IEditorValue value, TypeName type)
	{
		if (type.IsDictionary)
		{
			if (value is EditorDictionary editorDictionary)
			{
				bool isKeyMatch = editorDictionary.KeyType == type.TemplateTypes[0];
				bool isValueMatch = editorDictionary.ValueType == type.TemplateTypes[1];

				return isKeyMatch && isValueMatch;
			}
			else
			{
				return false;
			}
		}
		else if (type.IsArray)
		{
			if (value is EditorList editorList)
			{
				return editorList.ElementType == type.TemplateTypes[0];
			}
			else
			{
				return false;
			}
		}
		else
		{
			var typeInfo = Manifest.GetTypeInformation(type.Identifier);
			if (typeInfo == null)
			{
				throw new InvalidOperationException($"Cannot create an instance of an object of type \"{typeInfo}\".");
			}

			if (value is EditorNull)
			{
				return type.IsNullable;
			}
			else if (value is EditorScalarValue scalarValue)
			{
				return scalarValue.Type == type;
			}
			else if (value is EditorObject editorObject)
			{
				return editorObject.Type == type;
			}
			else
			{
				return false;
			}
		}
	}
}
