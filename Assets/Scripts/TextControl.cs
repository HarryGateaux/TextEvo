using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TextControl : MonoBehaviour {

	public string message;
	private Text textInstance;
	public Population population;
	public string finalOutput;

	// Use this for initialization

	void Awake() {

		population = new Population(100);
		for(int i = 0; i < 100; i++) {
			string output = population.phenotypes [i].phenotype;
			int fitness = population.fitnesses [i];
			finalOutput += output + " " + fitness + " -- ";
		}

		Debug.Log ("Generation is : " + population.generation);

	}


	void Start () {
		textInstance = GetComponent<Text>();

	}
	
	// Update is called once per frame
	void Update () {
		textInstance.text = "Generation is : " + population.generation.ToString() + "\n" + "Highest Fitness of " + population.maxFitness  + " is " + population.highestFitness();
	}
}
