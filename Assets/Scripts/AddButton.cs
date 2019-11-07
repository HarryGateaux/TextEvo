using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddButton : MonoBehaviour {

	public MainApp mainApp;
	public DropSelection dSelect;
	public DropMutation dMutate;
	public DropCrossover dCross;
	private int maxPops = 5;


	void Awake(){
		
		mainApp = gameObject.GetComponent<MainApp> ();
		dSelect = gameObject.GetComponent<DropSelection>();
		dMutate = gameObject.GetComponent<DropMutation>();
		dCross = gameObject.GetComponent<DropCrossover>();
	}


	public void OnClick(){
		
		//create a new population on click

		Debug.Log (dSelect);

		//less than not less than or equal to because it's not been added yet
		if (mainApp.populations.Count < maxPops) {

			mainApp.populations.Add (new Population (mainApp.popSize, "truncated" ) { name = (mainApp.populations.Count + 1).ToString() });


		} else {

			Debug.Log ("Population Limit Reached : 5 is the Maximum Number of Populations");
		}

	}
}
