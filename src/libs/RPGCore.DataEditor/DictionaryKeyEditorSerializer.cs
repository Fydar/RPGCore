using RPGCore.DataEditor.Manifest;
using System;
using System.IO;
using System.Text;
using System.Text.Json;

namespace RPGCore.DataEditor;

/// <summary>
/// A serializer for reading and writing <see cref="IEditorValue"/>.
/// </summary>
public class DictionaryKeyEditorSerializer : IEditorSerializer
{
	/// <inheritdoc/>
	public IEditorValue DeserializeValue(EditorSession session, TypeName type, ReadOnlySpan<byte> data)
	{
		var encoding = new UTF8Encoding();
		if (type.IsArray || type.IsDictionary || type.IsUnknown)
		{
			throw new InvalidOperationException($"Unable to read value of type \"{type}\".");
		}
		else if (type.Identifier == "string")
		{
			return new EditorScalarValue<string>(session, type, encoding.GetString(data.ToArray()));
		}
		else if (type.Identifier == "char")
		{
			return new EditorScalarValue<char>(session, type, encoding.GetString(data.ToArray())[0]);
		}

		var reader = new Utf8JsonReader(data, true, new JsonReaderState(new JsonReaderOptions()
		{
			CommentHandling = JsonCommentHandling.Allow,
			AllowTrailingCommas = true,
		}));

		if (!reader.Read())
		{
			throw new InvalidOperationException("Unable to read value as the data is empty.");
		}

		var typeInfo = session.ResolveType(type);

		if (typeInfo == null)
		{
			throw new InvalidOperationException($"Unable to resolve type information for \"{type}\".");
		}

		if (typeInfo.Fields != null && typeInfo.Fields.Count > 0)
		{
			throw new InvalidOperationException($"Read object type \"{type}\" as dictionary key.");
		}
		else
		{
			EditorScalarValue editorScalarValue = type.Identifier switch
			{
				"string" => new EditorScalarValue<string>(session, type, reader.GetString()),
				"char" => new EditorScalarValue<char>(session, type, reader.GetString()![0]),
				"bool" => new EditorScalarValue<bool>(session, type, reader.GetBoolean()),
				"byte" => new EditorScalarValue<byte>(session, type, reader.GetByte()),
				"sbyte" => new EditorScalarValue<sbyte>(session, type, reader.GetSByte()),
				"short" => new EditorScalarValue<short>(session, type, reader.GetInt16()),
				"ushort" => new EditorScalarValue<ushort>(session, type, reader.GetUInt16()),
				"int" => new EditorScalarValue<int>(session, type, reader.GetInt32()),
				"uint" => new EditorScalarValue<uint>(session, type, reader.GetUInt32()),
				"long" => new EditorScalarValue<long>(session, type, reader.GetInt64()),
				"ulong" => new EditorScalarValue<ulong>(session, type, reader.GetUInt64()),
				"float" => new EditorScalarValue<float>(session, type, reader.GetSingle()),
				"double" => new EditorScalarValue<double>(session, type, reader.GetDouble()),
				"decimal" => new EditorScalarValue<decimal>(session, type, reader.GetDecimal()),
				_ => throw new InvalidDataException($"Type \"{type.Identifier}\" is not readable as a scalar value."),
			};

			return editorScalarValue;
		}
	}

	/// <inheritdoc/>
	public void SerializeValue(IEditorValue value, TypeName type, Stream output)
	{
		var writer = new StreamWriter(output);

		switch (value)
		{
			case EditorScalarValue editorScalar:
			{
				switch (editorScalar.Value)
				{
					case byte[] byteArray:
					{
						writer.Write(Convert.ToBase64String(byteArray));
						break;
					}
					case Memory<byte> byteArray:
					{
						writer.Write(Convert.ToBase64String(byteArray.ToArray()));
						break;
					}
					case ArraySegment<byte> byteArray:
					{
						writer.Write(Convert.ToBase64String(byteArray.Array, byteArray.Offset, byteArray.Count));
						break;
					}
					case bool booleanValue:
					{
						writer.Write(booleanValue ? "true" : "false");
						break;
					}
					case string:
					case char:
					case byte:
					case sbyte:
					case short:
					case ushort:
					case int:
					case uint:
					case long:
					case ulong:
					case decimal:
					case double:
					case float:
					{
						writer.Write(editorScalar.Value.ToString());
						break;
					}
					case null:
					{
						writer.Write("null");
						break;
					}
					default:
					{
						throw new InvalidOperationException($"unknown type {editorScalar?.Value?.GetType().Name ?? "null"}");
					}
				}
				break;
			}
			case EditorNull:
			{
				writer.Write("null");
				break;
			}
			case EditorObject:
			{
				break;
			}
			case EditorDictionary:
			case EditorList:
			{
				break;
			}
			case null:
				throw new ArgumentNullException("Cannot serialize \"null\" editor value.");
			default:
				throw new ArgumentException($"Cannot serialize editor value of type \"{value.GetType().Name}\".");
		}
		writer.Flush();
	}
}
