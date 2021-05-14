using RPGCore.DataEditor.Manifest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace RPGCore.DataEditor
{
	/// <summary>
	/// A serializer for reading and writing <see cref="IEditorValue"/>.
	/// </summary>
	public class JsonEditorSerializer : IEditorSerializer
	{
		private readonly DictionaryKeyEditorSerializer dictionaryKeySerializer;

		/// <summary>
		/// Initialises a new instance of the <see cref="JsonEditorSerializer"/> class.
		/// </summary>
		public JsonEditorSerializer()
		{
			dictionaryKeySerializer = new DictionaryKeyEditorSerializer();
		}

		/// <inheritdoc/>
		public IEditorValue DeserializeValue(EditorSession session, TypeName type, ReadOnlySpan<byte> data)
		{
			var reader = new Utf8JsonReader(data, true, new JsonReaderState(new JsonReaderOptions()
			{
				CommentHandling = JsonCommentHandling.Allow,
				AllowTrailingCommas = true,
			}));

			if (!reader.Read())
			{
				throw new InvalidDataException("Unable to read value as the input data is empty.");
			}

			return ReadValueWithComments(session, type, ref reader);
		}

		/// <inheritdoc/>
		public void SerializeValue(IEditorValue value, TypeName type, Stream output)
		{
			using var writer = new Utf8JsonWriter(output, new JsonWriterOptions()
			{
				Indented = true,
				SkipValidation = false
			});
			SerializeValue(value, type, writer);
		}

		private void SerializeValue(IEditorValue value, TypeName type, Utf8JsonWriter writer)
		{
			foreach (string comment in value.Comments)
			{
				writer.WriteCommentValue(comment);
			}

			switch (value)
			{
				case EditorScalarValue editorScalar:
				{
					switch (editorScalar.Value)
					{
						case byte[] byteArray:
						{
							writer.WriteBase64StringValue(byteArray);
							break;
						}
						case Memory<byte> byteArray:
						{
							writer.WriteBase64StringValue(byteArray.Span);
							break;
						}
						case ArraySegment<byte> byteArray:
						{
							writer.WriteBase64StringValue(byteArray.AsSpan());
							break;
						}
						case string stringValue:
						{
							writer.WriteStringValue(stringValue);
							break;
						}
						case char characterValue:
						{
							writer.WriteStringValue(characterValue.ToString());
							break;
						}
						case bool booleanValue:
						{
							writer.WriteBooleanValue(booleanValue);
							break;
						}
						case byte numberValue:
						{
							writer.WriteNumberValue(numberValue);
							break;
						}
						case sbyte numberValue:
						{
							writer.WriteNumberValue(numberValue);
							break;
						}
						case short numberValue:
						{
							writer.WriteNumberValue(numberValue);
							break;
						}
						case ushort numberValue:
						{
							writer.WriteNumberValue(numberValue);
							break;
						}
						case int numberValue:
						{
							writer.WriteNumberValue(numberValue);
							break;
						}
						case uint numberValue:
						{
							writer.WriteNumberValue(numberValue);
							break;
						}
						case long numberValue:
						{
							writer.WriteNumberValue(numberValue);
							break;
						}
						case ulong numberValue:
						{
							writer.WriteNumberValue(numberValue);
							break;
						}
						case decimal numberValue:
						{
							writer.WriteNumberValue(numberValue);
							break;
						}
						case double numberValue:
						{
							writer.WriteNumberValue(numberValue);
							break;
						}
						case float numberValue:
						{
							writer.WriteNumberValue(numberValue);
							break;
						}
						case null:
						{
							writer.WriteNullValue();
							break;
						}
						default:
						{
							throw new InvalidDataException($"Unable to load type {editorScalar?.Value?.GetType().Name ?? "null"}");
						}
					}
					break;
				}
				case EditorObject editorObject:
				{
					writer.WriteStartObject();

					if (editorObject.Type != type)
					{
						writer.WritePropertyName("$type");
						writer.WriteStringValue(editorObject.Type.ToString());
					}

					foreach (var field in editorObject.Fields)
					{
						foreach (string comment in field.Comments)
						{
							writer.WriteCommentValue(comment);
						}

						writer.WritePropertyName(field.Name);
						SerializeValue(field.Value, field.Schema.Type, writer);
					}

					writer.WriteEndObject();
					break;
				}
				case EditorDictionary editorDictionary:
				{
					writer.WriteStartObject();

					foreach (var kvp in editorDictionary.KeyValuePairs)
					{
						foreach (string comment in kvp.Comments)
						{
							writer.WriteCommentValue(comment);
						}

						string keyString;

						using (var memoryStream = new MemoryStream())
						{
							dictionaryKeySerializer.SerializeValue(kvp.Key, editorDictionary.KeyType, memoryStream);

							memoryStream.Seek(0, SeekOrigin.Begin);
							var streamReader = new StreamReader(memoryStream);
							keyString = streamReader.ReadToEnd();
						}

						writer.WritePropertyName(keyString);
						SerializeValue(kvp.Value, editorDictionary.ValueType, writer);
					}

					writer.WriteEndObject();
					break;
				}
				case EditorList editorList:
				{
					writer.WriteStartArray();

					foreach (var element in editorList.Elements)
					{
						foreach (string comment in element.Comments)
						{
							writer.WriteCommentValue(comment);
						}

						SerializeValue(element, editorList.ElementType, writer);
					}

					writer.WriteEndArray();
					break;
				}
				case EditorNull:
				{
					writer.WriteNullValue();
					break;
				}
				case null:
					throw new ArgumentNullException("Cannot serialize \"null\" editor value.");
				default:
					throw new ArgumentException($"Cannot serialize editor value of type \"{value.GetType().Name}\".");
			}
		}

		private IEditorValue ReadValueWithComments(EditorSession session, TypeName type, ref Utf8JsonReader reader)
		{
			var comments = ReadComments(ref reader);

			var value = ReadValue(session, type, ref reader);

			foreach (string comment in comments)
			{
				value.Comments.Add(comment);
			}

			return value;
		}

		private IEditorValue ReadValue(EditorSession session, TypeName type, ref Utf8JsonReader reader)
		{
			if (reader.TokenType == JsonTokenType.Null)
			{
				return type.IsNullable
					? new EditorNull(session)
					: throw new InvalidDataException($"Cannot assign null value to non-nullable type \"{type}\".");
			}
			else if (type.IsArray)
			{
				return ReadArray(session, type, ref reader);
			}
			else if (type.IsDictionary)
			{
				return ReadDictionary(session, type, ref reader);
			}
			else if (type.IsUnknown)
			{
				return ReadObject(session, TypeName.Unknown, ref reader);
			}
			else
			{
				var typeInfo = session.ResolveType(type);

				if (typeInfo == null)
				{
					throw new InvalidDataException($"Unable to resolve type information for \"{type}\".");
				}

				if (typeInfo.Fields != null && typeInfo.Fields.Count > 0)
				{
					return ReadObject(session, type, ref reader);
				}
				else
				{
					return ReadScalarValue(session, type, ref reader);
				}
			}
		}

		private IEditorValue ReadObject(EditorSession session, TypeName type, ref Utf8JsonReader reader)
		{
			if (reader.TokenType != JsonTokenType.StartObject)
			{
				throw new InvalidDataException($"Unexpected token \"{reader.TokenType}\" whilst trying to read an object (expected \"{JsonTokenType.StartObject}\").");
			}

			EditorObject? editorObject = null;

			while (reader.Read())
			{
				var fieldComments = ReadComments(ref reader);

				switch (reader.TokenType)
				{
					case JsonTokenType.PropertyName:
					{
						string? propertyName = reader.GetString();

						// Create an EditorObject the first time we need it.
						if (editorObject == null)
						{
							if (string.Equals(propertyName, "$type", StringComparison.OrdinalIgnoreCase))
							{
								reader.Read();
								string? typeNameValue = reader.GetString();

								var explicitType = TypeName.FromString(typeNameValue.AsSpan());

								// Check to make sure that an explicit type inherits from the expected type.
								if (!type.IsUnknown && !session.IsTypeSubtypeOf(explicitType, type))
								{
									throw new InvalidDataException($"Cannot assign \"{explicitType}\" to \"{type}\".");
								}

								editorObject = new EditorObject(session, explicitType);
								continue;
							}
							else
							{
								if (type.IsUnknown)
								{
									throw new InvalidDataException($"Unknown type requires the first property to be \"$type\" to determine the final type of the object.");
								}

								editorObject = new EditorObject(session, type);
							}
						}

						if (string.Equals(propertyName, "$type", StringComparison.OrdinalIgnoreCase))
						{
							throw new InvalidDataException($"Type \"$type\" property must be supplied as the first property.");
						}

						var field = editorObject.Fields.FirstOrDefault(f => f.Name == propertyName);

						if (!reader.Read())
						{
							throw new InvalidDataException("Unexpected end of file.");
						}

						var fieldValue = ReadValueWithComments(session, field.Schema.Type, ref reader);
						field.Value = fieldValue;

						foreach (string comment in fieldComments)
						{
							field.Comments.Add(comment);
						}
						break;
					}
					case JsonTokenType.EndObject:
					{
						if (editorObject == null)
						{
							editorObject = new EditorObject(session, type);
						}
						return editorObject;
					}
				}
			}

			return new EditorNull(session);
		}

		private IEditorValue ReadArray(EditorSession session, TypeName type, ref Utf8JsonReader reader)
		{
			if (reader.TokenType != JsonTokenType.StartArray)
			{
				throw new InvalidDataException($"Unexpected token \"{reader.TokenType}\" whilst trying to read an array (expected \"{JsonTokenType.StartArray}\").");
			}

			var elementType = type.TemplateTypes[0];
			var editorList = new EditorList(session, elementType);

			while (reader.Read())
			{
				var elementComments = ReadComments(ref reader);

				switch (reader.TokenType)
				{
					default:
					{
						var elementValue = ReadValueWithComments(session, elementType, ref reader);

						foreach (string comment in elementComments)
						{
							elementValue.Comments.Add(comment);
						}

						editorList.Elements.Add(elementValue);
						break;
					}
					case JsonTokenType.EndArray:
					{
						return editorList;
					}
					case JsonTokenType.EndObject:
					case JsonTokenType.PropertyName:
					{
						throw new InvalidDataException($"Unexpected token \"{reader.TokenType}\" whilst trying to read an array.");
					}
				}
			}

			throw new InvalidDataException($"Unexpected token \"{reader.TokenType}\" whilst trying to read an array.");
		}

		private IEditorValue ReadDictionary(EditorSession session, TypeName type, ref Utf8JsonReader reader)
		{
			if (reader.TokenType != JsonTokenType.StartObject)
			{
				throw new InvalidDataException($"Unexpected token \"{reader.TokenType}\" whilst trying to read a dictionary (expected \"{JsonTokenType.StartObject}\").");
			}

			var keyType = type.TemplateTypes[0];
			var valueType = type.TemplateTypes[1];
			var editorDictionary = new EditorDictionary(session, keyType, valueType);

			while (reader.Read())
			{
				var keyValuePairComments = ReadComments(ref reader);

				switch (reader.TokenType)
				{
					case JsonTokenType.PropertyName:
					{
						var keyValue = ReadDictionaryKey(session, keyType, ref reader);

						if (!reader.Read())
						{
							throw new InvalidDataException("Unexpected end of file.");
						}

						var elementValue = ReadValueWithComments(session, valueType, ref reader);

						var kvp = new EditorKeyValuePair(editorDictionary, keyValue, elementValue);

						foreach (string comment in keyValuePairComments)
						{
							kvp.Comments.Add(comment);
						}

						editorDictionary.KeyValuePairs.Add(kvp);
						break;
					}
					case JsonTokenType.EndObject:
					{
						return editorDictionary;
					}
					default:
					{
						throw new InvalidDataException($"Unexpected token \"{reader.TokenType}\" whilst trying to read an array.");
					}
				}
			}

			throw new InvalidDataException($"Unexpected token \"{reader.TokenType}\" whilst trying to read a dictionary.");
		}

		private IEditorValue ReadDictionaryKey(EditorSession session, TypeName type, ref Utf8JsonReader reader)
		{
			string dictionaryKeyString = reader.GetString() ?? "";
			byte[] dictionaryKeyData = Encoding.UTF8.GetBytes(dictionaryKeyString);

			var value = dictionaryKeySerializer.DeserializeValue(session, type, dictionaryKeyData);

			return value;
		}

		private IEditorValue ReadScalarValue(EditorSession session, TypeName type, ref Utf8JsonReader reader)
		{
			var editorScalarValue = type.Identifier switch
			{
				"string" => new EditorScalarValue(session, type, reader.GetString()),
				"char" => new EditorScalarValue(session, type, reader.GetString()![0]),
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
				_ => throw new InvalidDataException($"Type \"{type.Identifier}\" is not readable as a scalar value."),
			};

			return editorScalarValue;
		}

		private static IList<string> ReadComments(ref Utf8JsonReader reader)
		{
			if (reader.TokenType != JsonTokenType.Comment)
			{
				return Array.Empty<string>();
			}

			var comments = new List<string>();
			do
			{
				comments.Add(reader.GetComment());
				reader.Read();
			}
			while (reader.TokenType == JsonTokenType.Comment);

			return comments;
		}
	}
}
