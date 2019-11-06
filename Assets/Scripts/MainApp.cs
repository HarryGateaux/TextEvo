using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainApp : MonoBehaviour {

	private Text textInstance;
	public Population[] pops;
	private int popSize;
	public bool enable;

	// Use this for initialization

	void Awake() {

		popSize = 100;
		enable = false;

		pops = new Population[5];
		string[] nameStrings = new string[5] { "first", "second", "third", "fourth", "fifth" };

		for (int i = 0; i < 5; i++) {

			pops [i] = new Population (popSize);
			pops [i].name = nameStrings [i];

		}
		Debug.Log ("Generation is : " + pops[0].generation);
	}


	void Start () {
		textInstance = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {

		string finalOutput = "";
		for (int i = 0; i < 5; i++) {
			finalOutput += pops [i].ToString ();

			if (enable && pops[i].maxFitness < 8 && Time.frameCount < 10000) {
				pops[i].NextGen ();
			}
		}

		textInstance.text = finalOutput;


	}
}
