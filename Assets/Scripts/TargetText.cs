using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetText : MonoBehaviour {
	private Text textInstance;
	public string targetString;
	// Use this for initialization

	void Awake () {
		textInstance = GetComponent<Text>();
		targetString = "helloworld";
	}
	void Start () {
		textInstance.text = "Target String is \"" + targetString + "\"";	
	}

}
