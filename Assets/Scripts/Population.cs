using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Population {

	public Phenotype[] phenotypes;
	public int[] fitnesses;
	private string targetString;
	public int generation = 0;
	public int maxFitness;
	private int pop;

	//class that defines population
	public Population(int popCount){

		targetString = "helloworld";
		pop = popCount;
		generate (popCount);

	}

	//creates first generation
	public void generate(int popCount){
		
		phenotypes = new Phenotype[popCount];
		fitnesses = new int[popCount];

		for (int i = 0; i < popCount; i++) {
			phenotypes [i] = new Phenotype (targetString.Length);
			fitnesses [i] = phenotypes [i].fitness (targetString);
		}

		generation++;

	}

	public void newGeneration(Phenotype[] pool){

		phenotypes = new Phenotype[pop];
		fitnesses = new int[pop];

		for (int i = 0; i < pop; i++) {

			//pick a random item from the first five of the top 5
			int rando = UnityEngine.Random.Range (0, 5); 
			string parentString = pool[rando].ToString ();


			phenotypes [i] = new Phenotype (parentString);
			fitnesses [i] = phenotypes [i].fitness (targetString);
		}

		generation++;

	}


	//returns the phenotype with the highest fitness in the current generation
	public string highestFitness(){

		maxFitness = fitnesses.Max ();
		int maxIdx = fitnesses.ToList ().IndexOf (maxFitness);
		return phenotypes[maxIdx].phenotype;

	}

	//returns an array with frequency of the top 3 fitnesses weighted by occurence
	public Phenotype[] matingPool(){

		Phenotype[] top3 = new Phenotype[3];
		Phenotype[] pool = new Phenotype[100];

		Array.Sort (fitnesses, phenotypes);
		Array.Reverse (fitnesses);
		Array.Reverse (phenotypes);
		Array.Copy (phenotypes, top3, 3);

		int sumFitnesses = fitnesses [0] + fitnesses [1] + fitnesses [2];

		for (int i = 0; i < pool.Length; i++) {
			
			int rando = UnityEngine.Random.Range (0, sumFitnesses);
			if (rando < fitnesses [0]) { 
				pool [i] = top3 [0];
			} else if (rando < fitnesses [0] + fitnesses [1]) {
				pool [i] = top3 [0];
			} else if (rando < fitnesses [0] + fitnesses [1] + fitnesses [2]) {
				pool [i] = top3 [0];
			} else if (sumFitnesses == 0) {
				pool [i] = top3 [0];
			};

		}

		Debug.Log ("top 3 fitnesses " + fitnesses [0].ToString() + " " +  fitnesses [1].ToString() + " " +  fitnesses [2].ToString());
		return pool;

	}
}