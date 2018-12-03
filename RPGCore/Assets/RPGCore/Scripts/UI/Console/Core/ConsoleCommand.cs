using System;

public abstract class ConsoleCommand
{
	public abstract void Run (ref string[] parameters, int offset = 0);
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

			if (target == typeof (string))
				return param;
			if (target == typeof (bool))
				return Convert.ToBoolean (param);
			if (target == typeof (short))
				return Convert.ToInt16 (param);
			if (target == typeof (int))
				return Convert.ToInt32 (param);
			if (target == typeof (long))
				return Convert.ToInt64 (param);
			if (target == typeof (ushort))
				return Convert.ToUInt16 (param);
			if (target == typeof (uint))
				return Convert.ToUInt32 (param);
			if (target == typeof (ulong))
				return Convert.ToUInt64 (param);
			if (target == typeof (byte))
				return Convert.ToByte (param);
			if (target == typeof (sbyte))
				return Convert.ToSByte (param);
			if (target == typeof (double))
				return Convert.ToInt16 (param);
			if (target == typeof (float))
				return Convert.ToSingle (param);
			if (target == typeof (decimal))
				return Convert.ToDecimal (param);
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
