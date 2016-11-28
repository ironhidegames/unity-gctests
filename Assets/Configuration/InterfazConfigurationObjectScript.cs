using UnityEngine;


public class InterfazConfigurationObjectScript : MonoBehaviour, InterfazConfiguration {

	int i = 0;

	public void Configure(ConfigurationData data) {
		i = 10;
	}

	public GameObject GetGameObject() {
		return this.gameObject;
	}
}
