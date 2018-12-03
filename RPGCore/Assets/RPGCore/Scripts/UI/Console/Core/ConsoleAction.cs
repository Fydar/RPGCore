using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public abstract class ConsoleAction : ConsoleCommand
{
	public List<Delegate> Usages;

	public ConsoleAction ()
	{
		Type thisType = GetType ();
		Usages = new List<Delegate> ();

		var methods = thisType.GetMethods (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);

		for (int i = 0; i < methods.Length; i++)
		{
			MethodInfo method = methods[i];

			object[] commandUsageAttibutes = method.GetCustomAttributes (typeof (CommandUsageAttribute), true);

			if (commandUsageAttibutes.Length > 0)
			{
				//CommandUsageAttribute attribute = (CommandUsageAttribute)commandUsageAttibutes [0];

				if (method.IsStatic)
				{
					Usages.Add (Delegate.CreateDelegate (thisType, method));
				}
				else
				{
					Usages.Add (GetDelegateFromMethod (method, this));
				}
			}
		}
	}

	public static Delegate GetDelegateFromMethod (MethodInfo method, object target)
	{
		var args = method.GetParameters ();
		Type[] types = new Type[args.Length];

		for (int i = 0; i < args.Length; i++)
		{
			types[i] = args[i].ParameterType;
		}

		if (types.Length == 0)
		{
			return Delegate.CreateDelegate (typeof (Action).MakeGenericType (types), target, method);
		}
		else if (types.Length == 1)
		{
			return Delegate.CreateDelegate (typeof (Action<>).MakeGenericType (types), target, method);
		}
		else if (types.Length == 2)
		{
			return Delegate.CreateDelegate (typeof (Action<,>).MakeGenericType (types), target, method);
		}
		else if (types.Length == 3)
		{
			return Delegate.CreateDelegate (typeof (Action<,,>).MakeGenericType (types), target, method);
		}
		else if (types.Length == 4)
		{
			return Delegate.CreateDelegate (typeof (Action<,,,>).MakeGenericType (types), target, method);
		}

		return null;
	}

	public override void Run (ref string[] Parameters, int offset = 0)
	{
		for (int i = 0; i < Usages.Count; i++)
		{
			var usage = Usages[i];
			var useParemeters = usage.Method.GetParameters ();

			if (Parameters.Length - offset != useParemeters.Length)
				continue;

			object[] compiledParams = CompileParams (Parameters, offset, useParemeters);

			if (compiledParams == null)
			{
				continue;
			}

			usage.DynamicInvoke (compiledParams);
			return;
		}

		if (Parameters.Length == offset)
		{
			Debug.Log (Help ());
			return;
		}
	}

	public override string Help ()
	{
		string helpString = "Help for command";

		foreach (Delegate usage in Usages)
		{
			helpString += "\n\t" + usage.Method.Name;
		}

		return helpString;
	}

	object[] CompileParams (string[] Parameters, int offset, ParameterInfo[] useParameters)
	{
		object[] compiledParams = new object[useParameters.Length];

		for (int j = 0; j < Parameters.Length - offset; j++)
		{
			var useParam = useParameters[j];
			var stringParam = Parameters[j + offset];

			bool success;
			object compiledParam = ConsoleCommand.PhraseParameter (stringParam, useParam.ParameterType, out success);

			if (!success || compiledParam == null)
				return null;

			compiledParams[j] = compiledParam;
		}
		return compiledParams;
	}
}
