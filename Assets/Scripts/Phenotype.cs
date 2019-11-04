using System;
using UnityEngine;

public class Phenotype {

	public char[] letters;
	public string phenotype;

	//constructor for random phenotype
	public Phenotype(int length){

		letters = new char[length];
		float rando;

		//generate a new random phenotype from letters a-z
		for (int i = 0; i < length; i++) {
			rando = UnityEngine.Random.Range(97, 122);
			letters [i] = (char)rando;
		}

		phenotype = new string (letters);
	}

	//constructor for phenotype from parent phenotype
	public Phenotype(string parentPhenotype){
		
		phenotype = parentPhenotype;
		letters = parentPhenotype.ToCharArray ();

	}

	//mutates the current phenotype
	public void mutate(){

		float pctChance = 0.1f;
		for (int i = 0; i < letters.Length; i++) {

			float rando = UnityEngine.Random.Range (0f, 1f); 
			//
			//mutates based on the pctChance
			if( rando < pctChance) {
				
					int rando2 = UnityEngine.Random.Range(97, 122);
					letters [i] = (char)(rando2);

			}
		}

		phenotype = new string (letters);
	}

	//combines the phenotype with another and crosses over
	public void crossOver(string partner){

		int crossoverPt = UnityEngine.Random.Range (0, partner.Length);
		string crossedString = phenotype.Substring (0, crossoverPt) + partner.Substring (crossoverPt, partner.Length - crossoverPt);
		phenotype = crossedString;
		mutate ();
	}

	public int fitness(string targetString){

		int score = 0;
		for(int i = 0; i < phenotype.Length; i++) {

			if (phenotype [i] == targetString [i]) {

				score++;

			}
		}
		return score;
	}

	public override string ToString()
	{
		return phenotype;
	}

}
