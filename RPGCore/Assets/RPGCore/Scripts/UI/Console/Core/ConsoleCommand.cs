using System;

public abstract class ConsoleCommand
{
	public abstract void Run (ref string[] Parameters, int offset = 0);
	public abstract string Help ();

	public static object PhraseParameter (string param, Type target, out bool success)
	{
		success = true;
		try
		{
			if (target.IsEnum)
			{
				string formattedParam = param.ToLower ();
				string[] possibleNames = Enum.GetNames (target);

				for (int i = 0; i < possibleNames.Length; i++)
				{
					string formattedName = possibleNames[i].ToLower ();

					if (formattedName == formattedParam)
					{
						return Enum.ToObject (target, i);
					}
				}

				success = false;
				return null;
			}
			else if (target == typeof (String))
			{
				return param;
			}
			else if (target == typeof (Boolean))
			{
				return Convert.ToBoolean (param);
			}
			else if (target == typeof (Int16))
			{
				return Convert.ToInt16 (param);
			}
			else if (target == typeof (Int32))
			{
				return Convert.ToInt32 (param);
			}
			else if (target == typeof (Int64))
			{
				return Convert.ToInt64 (param);
			}
			else if (target == typeof (UInt16))
			{
				return Convert.ToUInt16 (param);
			}
			else if (target == typeof (UInt32))
			{
				return Convert.ToUInt32 (param);
			}
			else if (target == typeof (UInt64))
			{
				return Convert.ToUInt64 (param);
			}
			else if (target == typeof (Byte))
			{
				return Convert.ToByte (param);
			}
			else if (target == typeof (SByte))
			{
				return Convert.ToSByte (param);
			}
			else if (target == typeof (Double))
			{
				return Convert.ToInt16 (param);
			}
			else if (target == typeof (Single))
			{
				return Convert.ToSingle (param);
			}
			else if (target == typeof (Decimal))
			{
				return Convert.ToDecimal (param);
			}
		}
		catch
		{
			success = false;
			return null;
		}

		success = false;
		return null;
	}
}