using UnityEngine;

namespace RPGCore.Utility
{
	public class ErrorIfNullAttribute : PropertyAttribute
	{
		public string ErrorMessage = "This field cannot be null.";

		public ErrorIfNullAttribute ()
		{

		}

		public ErrorIfNullAttribute (string error)
		{
			ErrorMessage = error;
		}
	}
}

