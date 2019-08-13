using System;
using System.Collections.Generic;

public class ParameterConverter
{
	public delegate object Converter (string parameter, System.Type targetType);

	public static Dictionary<Type, Converter> Converters;

	static ParameterConverter ()
	{
		Converters = new Dictionary<Type, Converter>
		{
			{ typeof (string), StringConverter }
		};
	}

	private static object StringConverter (string parameter, Type targetType)
	{
		return parameter;
	}

	private static object BoolConverter (string parameter, Type targetType)
	{
		return parameter;
	}
}
