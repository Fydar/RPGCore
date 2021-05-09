using RPGCore.DataEditor.Manifest;
using System;
using System.IO;
using System.Text;
using System.Text.Json;

namespace RPGCore.DataEditor
{
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
				return new EditorScalarValue(session, type, encoding.GetString(data.ToArray()));
			}
			else if (type.Identifier == "char")
			{
				return new EditorScalarValue(session, type, encoding.GetString(data.ToArray())[0]);
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
				var editorScalarValue = type.Identifier switch
				{
					"bool" => new EditorScalarValue(session, type, reader.GetBoolean()),
					"byte" => new EditorScalarValue(session, type, reader.GetByte()),
					"sbyte" => new EditorScalarValue(session, type, reader.GetSByte()),
					"short" => new EditorScalarValue(session, type, reader.GetInt16()),
					"ushort" => new EditorScalarValue(session, type, reader.GetUInt16()),
					"int" => new EditorScalarValue(session, type, reader.GetInt32()),
					"uint" => new EditorScalarValue(session, type, reader.GetUInt32()),
					"long" => new EditorScalarValue(session, type, reader.GetInt64()),
					"ulong" => new EditorScalarValue(session, type, reader.GetUInt64()),
					"float" => new EditorScalarValue(session, type, reader.GetSingle()),
					"double" => new EditorScalarValue(session, type, reader.GetDouble()),
					"decimal" => new EditorScalarValue(session, type, reader.GetDecimal()),
					_ => throw new InvalidOperationException($"unknown type {type.Identifier}"),
				};

				return editorScalarValue;
			}
		}

		/// <inheritdoc/>
		public void SerializeValue(IEditorValue value, Stream output)
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
							writer.Write(value.ToString());
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
				case EditorObject editorObject:
				{
					break;
				}
				case EditorDictionary editorDictionary:
				case EditorList editorList:
				{
					break;
				}
				case null:
					throw new ArgumentNullException("Cannot serialize \"null\" editor value.");
				default:
					throw new ArgumentException($"Cannot serialize editor value of type \"{value.GetType().Name}\".");
			}
		}
	}
}
