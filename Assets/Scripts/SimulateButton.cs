using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimulateButton : MonoBehaviour {
	
	public Text textButton; //this is assigned manually in GUI
	public MainApp mainApp;

	void Awake(){
		mainApp = gameObject.GetComponent<MainApp> ();
	}

	public void OnClick(){
		
		mainApp.enable = !mainApp.enable;

		if (mainApp.enable == true) {
			textButton.text = "Pause";
		} else {
			textButton.text = "Simulate";
		}
	}
		
}


//to do

//implement selection algorithms
//implement phenotype > genotype mapping
