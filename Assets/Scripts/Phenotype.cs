using System;
using UnityEngine;

public class Phenotype {

	public char[] letters;
	public string phenotype;
	public float mutateRate;

	//constructor for first random phenotype
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

	//carries out evolution
	public void Evolve(string partner, float mutateRate){

		CrossOver (partner, "Uniform");
		Mutate (mutateRate, "randomChoice");

	}

	//mutates the current phenotype
	public void Mutate(float mutateRate, string type){

		switch (type) {

		//if chosen, gene is mutated to random choice in range
		case "randomChoice":

			for (int i = 0; i < letters.Length; i++) {
				//mutates based on the mutateRate
				if( UnityEngine.Random.Range (0f, 1f) < mutateRate) {

					int randomChoice = UnityEngine.Random.Range(97, 122);
					letters [i] = (char)(randomChoice);

				}
			}

			break;

		//if chosen, gene is tweaked up or down one index in the range
		case "stepUp":

			for (int i = 0; i < letters.Length; i++) {
				//mutates based on the mutateRate
				if( UnityEngine.Random.Range (0f, 1f) < mutateRate) {

					int intLetter = Convert.ToInt32 (letters [i]);
					intLetter = ((intLetter + 1) - 97) % (122 - 97);
					letters [i] = (char)(intLetter + 97);
				}
			}
				
			break;

		//if chosen, gene is tweaked by an amount determined by gaussian mean = 0 variance = sigma
		case "gaussianConvolution":


			break;

		}








		phenotype = new string (letters);
	}

	//combines the phenotype with another and crosses over, have choices of crossover type
	public void CrossOver(string partner, string type){

		int xPt1 = UnityEngine.Random.Range (0, partner.Length);
		string crossedString;

		switch (type)
		{
		//cuts the genotype at one point and swaps genes
		case "OnePt":

			crossedString = phenotype.Substring (0, xPt1) + partner.Substring (xPt1);
			phenotype = crossedString;
			break;
				
		//cuts the genotype at two points and swaps genes
		case "TwoPt":
			
			int xPt2 = UnityEngine.Random.Range (0, partner.Length);

			if (xPt1 <= xPt2) {
				crossedString = phenotype.Substring (0, xPt1) + partner.Substring (xPt1, xPt2 - xPt1) + phenotype.Substring (xPt2);
			} 
			else 
			{
				crossedString = partner.Substring (0, xPt2) + phenotype.Substring (xPt2, xPt1 - xPt2) + partner.Substring (xPt1);

			}
			phenotype = crossedString;
			break;

		//swaps genes by index depending on the swap rate
		case "Uniform":

			float swapRate = 0.1f;
			char[] chars = phenotype.ToCharArray ();
			char[] charsPart = partner.ToCharArray ();

			for (int i = 0; i < phenotype.Length; i++) {

				//if under swapRate swap genes at index
				if (UnityEngine.Random.Range (0f, 1f) < swapRate) {
					chars [i] = charsPart [i];
				}

			}

			phenotype = new string (chars);
			break;
		}

		letters = phenotype.ToCharArray ();

	}

	public int Fitness(string targetString){

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

	public double randDist(){

		System.Random rand = new System.Random(); //reuse this if you are generating many
		double u1 = 1.0-rand.NextDouble(); //uniform(0,1] random doubles
		double u2 = 1.0-rand.NextDouble();
		double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
			Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
		double randNormal =	0 + 1 * randStdNormal; //random normal(mean,stdDev^2)
		
		return randNormal;
	}

}
