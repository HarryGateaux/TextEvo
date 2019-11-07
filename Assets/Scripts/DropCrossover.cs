using UnityEngine;
using UnityEngine.UI;

public class DropCrossover: MonoBehaviour {

	public Dropdown dd;
	public string choice;

	void Awake () {
		dd = GetComponent<Dropdown>();
		choice = dd.options [dd.value].text;
	}

	void Update () {

		choice = dd.options [dd.value].text;

	}
}
