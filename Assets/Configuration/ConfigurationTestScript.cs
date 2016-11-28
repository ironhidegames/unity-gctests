using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConfigurationTestScript : MonoBehaviour {

	public int count = 250;
	List<GameObject>  onConfigurationObjects = new List<GameObject>();
	List<InterfazConfiguration>  interfazObjects = new List<InterfazConfiguration>();

	ConfigurationData data = new ConfigurationData();

	public void CreateObjects() {
		DestroyObjects ();

		for (int i = 0; i < count; i++) {
			var obj = new GameObject ("SendMessage");
			obj.AddComponent<OnConfigurationObjectScript> ();
			onConfigurationObjects.Add (obj);
			break;
		}

		for (int i = 0; i < count; i++) {
			var obj = new GameObject ("Interfaz");
			var interfaz = obj.AddComponent<InterfazConfigurationObjectScript> ();
			interfazObjects.Add (interfaz);
			break;
		}
	}

	void DestroyObjects() {
		for (int j = 0, interfazObjectsCount = interfazObjects.Count; j < interfazObjectsCount; j++) {
			var i = interfazObjects [j];
			GameObject.Destroy (i.GetGameObject());
		}

		for (int i = 0, onConfigurationObjectsCount = onConfigurationObjects.Count; i < onConfigurationObjectsCount; i++) {
			var o = onConfigurationObjects [i];
			GameObject.Destroy (o.gameObject);
		}

		interfazObjects.Clear ();
		onConfigurationObjects.Clear ();
	}

	public void ConfigureInterfazes() {
		for (int j = 0, interfazObjectsCount = count; j < interfazObjectsCount; j++) {
			var i = interfazObjects [0];
			i.Configure (data);
		}
	}

	public void ConfigureSendMessage() {
		for (int i = 0, onConfigurationObjectsCount = count; i < onConfigurationObjectsCount; i++) {
			var o = onConfigurationObjects [0];
			o.SendMessage ("OnConfiguration", data);
		}
	}

	void Awake() {
		CreateObjects ();
	}

//	void Update() {
//		if (Input.GetKeyDown (KeyCode.Alpha1)) {
//			ConfigureInterfazes ();
//		}
//		if (Input.GetKeyDown (KeyCode.Alpha2)) {
//			ConfigureSendMessage ();
//		}
//		if (Input.GetKeyDown (KeyCode.Alpha3)) {
//			CreateObjects ();
//		}
//	}
}
