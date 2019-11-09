using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuiSetup : MonoBehaviour {

	private Text textSimulate, textTarget, textOutput;
	private string targetString;
	private Dropdown dSelect, dMutate, dCross;
	private int maxPops = 5, popSize = 100;
	private bool enable = false;
	private List<GeneticAlgo> geneticAlgos;

	void Awake(){

		targetString = "helloworld";

		textOutput = GetComponent<Text>();
		textSimulate = GameObject.Find("TextSimulate").GetComponent<Text> ();
		textTarget = GameObject.Find("TextTarget").GetComponent<Text> ();

		textTarget.text = "Target String is \"" + targetString + "\"";	

		dSelect = GameObject.Find("DropSelect").GetComponent<Dropdown>();
		dMutate = GameObject.Find ("DropMutate").GetComponent<Dropdown>();
		dCross = GameObject.Find ("DropCross").GetComponent<Dropdown>();

		geneticAlgos = new List<GeneticAlgo> ();

    }

    public void OnClickAdd(){

		string selectType = dSelect.options [dSelect.value].text;
		string mutateType = dMutate.options [dMutate.value].text;
		string crossType = dCross.options [dCross.value].text;

		if (geneticAlgos.Count < maxPops) {

            Fitness fitness = new Fitness("helloworld");
            Population population = new Population(popSize, fitness._targetString);
            Selection selection = new Selection(selectType);
            CrossOver crossover = new CrossOver(crossType);
            Mutation mutation = new Mutation(mutateType);

            geneticAlgos.Add (new GeneticAlgo(fitness, population, selection, crossover, mutation) {_name = (geneticAlgos.Count + 1).ToString() });
		} else {

			Debug.Log ("Population Limit Reached : 5 is the Maximum Number of geneticAlgos");
		}
	}

	public void OnClickSimulate(){

		enable = !enable;

		if (enable == true) {
			textSimulate.text = "Pause";
		} else {
			textSimulate.text = "Simulate";
		}
	}
		
	// Update is called once per frame
	void Update () {

		string finalOutput = "";

		for (int i = 0; i < geneticAlgos.Count; i++) {
			finalOutput += geneticAlgos [i].ToString ();

			if (enable && geneticAlgos[i].Population._bestFitness < 10 && Time.frameCount < 10000) {
				geneticAlgos[i].NextGeneration ();
			}
		}
        
		textOutput.text = finalOutput;
	}
}
