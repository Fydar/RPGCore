using System;
using System.Linq;
using System.Reflection;

namespace RPGCore.Data.Polymorphic.Naming;

public static class GenericNames
{
	public static string ToGenericName(MemberInfo member)
	{
		if (typeof(MethodInfo).IsAssignableFrom(member.GetType()))
		{
			var method = (MethodInfo)member;
			if (!method.IsGenericMethod)
			{
				return method.Name;
			}

			string typeName = method.GetGenericMethodDefinition().Name;

			string genericArgs = string.Join(", ", method.GetGenericArguments()
				.Select(ta => ToGenericName(ta)).ToArray());

			return $"{typeName}<{genericArgs}>";
		}
		return member.Name;
	}

	public static string ToGenericFullname(MemberInfo member)
	{
		string parent = "";

		if (member.DeclaringType != null)
		{
			parent = ToGenericFullname(member.DeclaringType) + ".";
		}

		if (typeof(MethodInfo).IsAssignableFrom(member.GetType()))
		{
			var method = (MethodInfo)member;

			return $"{ToGenericFullname(method.DeclaringType)}.{ToGenericName(method)}";
		}

		return parent + member.Name;
	}

	public static string ToGenericName(Type type)
	{
		if (type == null)
		{
			return "Unknown";
		}

		if (TryGetFrameworkName(type, out string frameworkName))
		{
			return frameworkName;
		}

		if (!type.IsGenericType)
		{
			return type.Name;
		}

		string typeName = type.GetGenericTypeDefinition().Name;

		typeName = typeName.Substring(0, typeName.IndexOf('`'));

		string genericArgs = string.Join(",", type.GetGenericArguments()
			.Select(ta => ToGenericName(ta)).ToArray());

		return $"{typeName}<{genericArgs}>";
	}

	public static string ToGenericFullname(Type type)
	{
		if (type == null)
		{
			return "Unknown";
		}

		if (TryGetFrameworkName(type, out string frameworkName))
		{
			return frameworkName;
		}

		if (!type.IsGenericType)
		{
			return type.FullName;
		}

		string typeName = type.GetGenericTypeDefinition().FullName;

		typeName = typeName.Substring(0, typeName.IndexOf('`'));

		string genericArgs = string.Join(",", type.GetGenericArguments()
			.Select(ToGenericName).ToArray());

		return $"{typeName}<{genericArgs}>";
	}

	public static string ToUnboundName(Type type)
	{
		if (type == null)
		{
			return "Unknown";
		}

		if (!type.IsGenericType)
		{
			if (string.IsNullOrEmpty(type.FullName))
			{
				return type.Name;
			}
			else
			{
				return type.FullName;
			}
		}

		string genericTypeName = type.GetGenericTypeDefinition().FullName;

		genericTypeName = genericTypeName.Substring(0, genericTypeName.IndexOf('`'));

		return genericTypeName + "<" + new string(',', type.GetGenericArguments().Length - 1) + ">";
	}

	public static bool TryGetFrameworkName(Type type, out string frameworkName)
	{
		if (type == typeof(object))
		{
			frameworkName = "object";
		}
		else if (type == typeof(string))
		{
			frameworkName = "string";
		}
		else if (type == typeof(bool))
		{
			frameworkName = "bool";
		}
		else if (type == typeof(void))
		{
			frameworkName = "void";
		}
		else if (type == typeof(float))
		{
			frameworkName = "float";
		}
		else if (type == typeof(double))
		{
			frameworkName = "double";
		}
		else if (type == typeof(decimal))
		{
			frameworkName = "decimal";
		}
		else if (type == typeof(char))
		{
			frameworkName = "char";
		}
		else if (type == typeof(short))
		{
			frameworkName = "short";
		}
		else if (type == typeof(int))
		{
			frameworkName = "int";
		}
		else if (type == typeof(long))
		{
			frameworkName = "long";
		}
		else if (type == typeof(byte))
		{
			frameworkName = "byte";
		}
		else if (type == typeof(ushort))
		{
			frameworkName = "ushort";
		}
		else if (type == typeof(uint))
		{
			frameworkName = "uint";
		}
		else if (type == typeof(ulong))
		{
			frameworkName = "ulong";
		}
		else if (type == typeof(sbyte))
		{
			frameworkName = "sbyte";
		}
		else
		{
			frameworkName = null;
			return false;
		}
		return true;
	}
}
