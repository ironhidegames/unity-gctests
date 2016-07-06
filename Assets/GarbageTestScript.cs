using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public delegate int MegaCallbackDelegate(object o);

public static class SuperHandler
{
	public static int CallbackCode (object o)
	{
		GarbageTestScript t = o as GarbageTestScript;
		return t.superValue++;
	}
}

public class GarbageTestScript : MonoBehaviour {

	public int actions = 100;
	public Button[] modeButtons;

	public Text currentModeText;

	[NonSerialized]
	bool[] modes = new bool[7];

	List<Action<object>> list = new  List<Action<object>>(); 
	List<MegaCallbackDelegate> listWithDelegates = new  List<MegaCallbackDelegate>(); 

	public int superValue = 0;

	static MegaCallbackDelegate myDelegate = delegate(object obj) {
		GarbageTestScript garbageTest = obj as GarbageTestScript;
		return garbageTest.superValue++;
	};

	public static int StaticCallbackCode (object o)
	{
		GarbageTestScript t = o as GarbageTestScript;
		return t.superValue++;
	}

	public int MyCallbackCode (object o)
	{
		return this.superValue++;
	}

	public int MyCallbackCodeWithCast (object o)
	{
		GarbageTestScript t = o as GarbageTestScript;
		return t.superValue++;
	}

	void Start () {
		// modes = new bool[modeButtons.Length];
		for (int i = 0; i < modeButtons.Length; i++) {
			int index = i;
			modeButtons [i].onClick.AddListener (() => ActivateMode (index));

			modes [i] = true;
		}
	}

	void ActivateMode(int modeIndex)
	{
		for (int i = 0; i < modes.Length; i++) {
			modes [i] = false;
		}
		modes [modeIndex] = true;

		currentModeText.text = modeButtons [modeIndex].GetComponentInChildren<Text> ().text;
	}

	// Update is called once per frame
	void Update () {
	
		list.Clear ();
		listWithDelegates.Clear ();

		if (modes [0]) {

			Profiler.BeginSample ("Modo1");
			//forma 1
			for (int i = 0; i < actions; i++) {
				list.Add (delegate(object obj) {
					this.superValue++;
				});
			}
			Profiler.EndSample ();
		}

		if (modes [1]) {

			//forma 3
			Profiler.BeginSample ("Modo2");
			for (int i = 0; i < actions; i++) {
				listWithDelegates.Add (MyCallbackCode);
			}
			Profiler.EndSample ();
		}

		if (modes [2]) {

			//forma 2
			Profiler.BeginSample ("Modo3");
			for (int i = 0; i < actions; i++) {
				listWithDelegates.Add (StaticCallbackCode);
			}
			Profiler.EndSample ();
		}

		if (modes [3]) {

			//forma 1
			Profiler.BeginSample ("Modo4");
			for (int i = 0; i < actions; i++) {
				list.Add (delegate(object obj) {
					GarbageTestScript garbageTest = obj as GarbageTestScript;
					garbageTest.superValue++;
				});
			}
			Profiler.EndSample ();
		}

		if (modes [4]) {
			Profiler.BeginSample ("Modo5");
			for (int i = 0; i < actions; i++) {
				listWithDelegates.Add (MyCallbackCodeWithCast);
			}
			Profiler.EndSample ();
		}

		if (modes [5]) {
			Profiler.BeginSample ("Modo6");
			for (int i = 0; i < actions; i++) {
				listWithDelegates.Add (SuperHandler.CallbackCode);
			}
			Profiler.EndSample ();
		}

		if (modes [6]) {
			Profiler.BeginSample ("Modo7");
			for (int i = 0; i < actions; i++) {
				listWithDelegates.Add (GarbageTestScript.myDelegate);
			}
			Profiler.EndSample ();
		}
	}
}
