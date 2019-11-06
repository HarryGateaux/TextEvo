using System;
using UnityEngine;

public class Phenotype {

	public char[] letters;
	public string phenotype;
	public float mutateRate;

	//constructor for first random genotype
	public Phenotype(int length){

		letters = new char[length];
		int randInt;

		//generate a new random genotype from letters a-z
		for (int i = 0; i < length; i++) {
			randInt = UnityEngine.Random.Range(97, 122);
			letters [i] = (char)randInt;
		}

		phenotype = new string (letters);
	}

	//constructor for genotype from parent genotype
	public Phenotype(string parentPhenotype){
		
		phenotype = parentPhenotype;
		letters = parentPhenotype.ToCharArray ();

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
