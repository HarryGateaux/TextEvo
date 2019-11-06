using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//contains functions to apply mutation/crossover to individuals
public static class Evolution {

	public static void Evolve(Phenotype p1, Phenotype p2, float mutationRate, string crossoverType, string mutationType) {
			
		//cross the two parents
		CrossOver (p1, p2, "OnePt");
		//mutate both of the children
		Mutate (p1, mutationRate, "randomChoice");
		Mutate (p2, mutationRate, "randomChoice");


	}

	//takes two phenotypes as input and mutates them
	public static void CrossOver(Phenotype p1, Phenotype p2, string type) {

		int xPt1 = UnityEngine.Random.Range (0, p1.phenotype.Length);
		string crossedString1, crossedString2;

		switch (type)
		{
		//cuts the genotype at one point and swaps genes
		case "OnePt":

			crossedString1 = p1.phenotype.Substring (0, xPt1) + p2.phenotype.Substring (xPt1);
			p1 = new Phenotype (crossedString1);

			crossedString2 = p2.phenotype.Substring (0, xPt1) + p1.phenotype.Substring (xPt1);
			p2 = new Phenotype (crossedString2);

			break;

			//cuts the genotype at two points and swaps genes
		case "TwoPt":

			int xPt2 = UnityEngine.Random.Range (0, p1.phenotype.Length);

			if (xPt1 <= xPt2) {
				crossedString1 = p1.phenotype.Substring (0, xPt1) + p2.phenotype.Substring (xPt1, xPt2 - xPt1) + p1.phenotype.Substring (xPt2);
				crossedString2 = p2.phenotype.Substring (0, xPt1) + p1.phenotype.Substring (xPt1, xPt2 - xPt1) + p2.phenotype.Substring (xPt2);
			} 
			else 
			{
				crossedString1 = p2.phenotype.Substring (0, xPt2) + p1.phenotype.Substring (xPt2, xPt1 - xPt2) + p2.phenotype.Substring (xPt1);
				crossedString2 = p1.phenotype.Substring (0, xPt2) + p2.phenotype.Substring (xPt2, xPt1 - xPt2) + p1.phenotype.Substring (xPt1);
			}
			p1 = new Phenotype (crossedString1);
			p2 = new Phenotype (crossedString2);
			break;

			//swaps genes by index depending on the swap rate
		case "Uniform":

			float swapRate = 0.1f;
			char[] chars1 = p1.phenotype.ToCharArray ();
			char[] chars2 = p2.phenotype.ToCharArray ();

			for (int i = 0; i < p1.phenotype.Length; i++) {

				//if under swapRate swap genes at index
				if (UnityEngine.Random.Range (0f, 1f) < swapRate) {
					char temp = chars1 [i];
					chars1 [i] = chars2 [i];
					chars2 [i] = temp;
				}
			}

			p1 = new Phenotype (new string (chars1));
			p2 = new Phenotype (new string (chars2));

			break;
		}
	}

	//takes phenotype as input and mutates it
	public static void Mutate(Phenotype p1, float mutationRate, string type) {

	switch (type) {

	//if chosen, gene is mutated to random choice in range
	case "randomChoice":

		for (int i = 0; i < p1.letters.Length; i++) {
			//mutates based on the mutationRate
			if( UnityEngine.Random.Range (0f, 1f) < mutationRate) {

				int randomChoice = UnityEngine.Random.Range(97, 122);
				p1.letters [i] = (char)(randomChoice);

			}
		}

		break;

		//if chosen, gene is tweaked up or down one index in the range
	case "stepUp":

		for (int i = 0; i < p1.letters.Length; i++) {
			//mutates based on the mutationRate
			if( UnityEngine.Random.Range (0f, 1f) < mutationRate) {

				int intLetter = Convert.ToInt32 (p1.letters [i]);
				intLetter = ((intLetter + 1) - 97) % (122 - 97);
				p1.letters [i] = (char)(intLetter + 97);
			}
		}

		break;

		//if chosen, gene is tweaked by an amount determined by gaussian mean = 0 variance = sigma
	case "gaussianConvolution":


		break;

	}

	p1.phenotype = new string (p1.letters);



	}

}