using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class MainApp : MonoBehaviour {

	private Text textInstance;
	public Population[] pops;
	public List<Population> populations;
	public int popSize;
	public bool enable;

	// Use this for initialization

	void Awake() {

		popSize = 100;
		enable = false;

		populations = new List<Population> ();
		enable = false;

	}


	void Start () {
		textInstance = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {

		string finalOutput = "";
		for (int i = 0; i < populations.Count; i++) {
			finalOutput += populations [i].ToString ();

			if (enable && populations[i].maxFitness < 10 && Time.frameCount < 10000) {
				populations[i].NextGen ();
			}
		}

		textInstance.text = finalOutput;

	}
}
