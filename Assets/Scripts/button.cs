using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class button : MonoBehaviour {

	public TextControl txtControl;
	public Population pop;

	void Awake(){
		txtControl = gameObject.GetComponent<TextControl> ();
	}

	public void OnClick(){
		Debug.Log ("new generation..." + txtControl.population.generation);
		txtControl.population.newGeneration (txtControl.population.matingPool());
	}
		
}
