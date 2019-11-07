using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddButton : MonoBehaviour {

	public MainApp mainApp;
	private int maxPops = 5;

	void Awake(){
		mainApp = gameObject.GetComponent<MainApp> ();
	}


	public void OnClick(){
		//create a new population on click

		string[] nameStrings = new string[5] { "First", "Second", "Third", "Fourth", "Fifth" };

		//less than not less than or equal to because it's not been added yet
		if (mainApp.populations.Count < maxPops) {

			mainApp.populations.Add (new Population (mainApp.popSize, "stochasticUniversalSampling") { name = nameStrings [mainApp.populations.Count] });

		} else {

			Debug.Log ("Population Limit Reached : 5 is the Maximum Number of Populations");
		}

	}
}
