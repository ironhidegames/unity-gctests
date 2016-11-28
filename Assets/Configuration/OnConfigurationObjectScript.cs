using UnityEngine;
using System.Collections;

public class OnConfigurationObjectScript : MonoBehaviour {

	public int i = 0;

	void OnConfiguration(ConfigurationData data) {
		i = 10;
	}
}
