using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stopwatch = System.Diagnostics.Stopwatch;

public class EventModelTest : MonoBehaviour
{

	public void SlapFish (int a)
	{

	}

	void Start ()
	{
		IntEventField left = new IntEventField (1);
		IntEventField right = new IntEventField (3);

		left += () => {
			// Debug.Log ("hi");
		};

		left += (int newValue) => {
			Debug.Log ("New Value: " + newValue);
		};

		left.Value += 5;

		Debug.Log ("Left + Right = " + (left + right).Calculate());

		//int result = left + right;
		//Debug.Log ("Implicit Int Casting: " + result.ToString ());

		/*Stopwatch sw = Stopwatch.StartNew ();

		for (int i = 0; i < 100000; i++)
		{
			result = left.Value + right.Value;
		}
		Debug.Log ("Traditional Maths: " + sw.ElapsedTicks);
		sw.Reset ();

		sw.Start ();

		for (int i = 0; i < 100000; i++)
		{
			result = left + right;
		}
		Debug.Log ("Overriden Maths: " + sw.ElapsedTicks);*/

		IntEventField one = new IntEventField (1);

		IntEventField test = new IntEventField(left + right + one);

		left.Value++;
		right.Value++;

		Debug.Log (test.Value);
	}
}