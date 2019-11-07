﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Population {

	public string selectionType;
	public string mutationType = "randomChoice";
	public string crossoverType = "OnePt";
	public string targetString;
	public string name;

	public int generation = 0;
	public int maxFitness;
	public int size;

	public float mutationRate = 0.1f;

	public Phenotype[] phenotypes;
	public int[] fitnesses;


	//class that defines population
	public Population(int _size, string _selectionType){

		targetString = "helloworld";
		size = _size;
		selectionType = _selectionType;
		Seed ();
	}

	//creates first generation
	public void Seed(){

		phenotypes = new Phenotype[size];
		fitnesses = new int[size];

		for (int i = 0; i < size; i++) {
			phenotypes [i] = new Phenotype (targetString.Length);
			fitnesses [i] = phenotypes [i].Fitness (targetString);
		}

		generation++;
	}

	//returns the string with the highest fitness in the current generation
	public string MostFit(){

		maxFitness = fitnesses.Max ();
		int maxIdx = fitnesses.ToList ().IndexOf (maxFitness);
		return phenotypes[maxIdx].phenotype;
	}

	//sorts the phenotypes and fitness by fitness descending order
	public void SortByFitness(){

		Array.Sort (fitnesses, phenotypes);
		Array.Reverse (fitnesses);
		Array.Reverse (phenotypes);

		string top3fit = String.Format ("{0} Population : Generation {1} : Top 3 Fitnesses {2}, {3}, {4}", name, generation, fitnesses [0].ToString (), fitnesses [1].ToString (), fitnesses [2].ToString ());
		
		string top5pheno = String.Format("{0} Population : Generation  {1} : Top 5 phenotypes ", name, generation);

		for (int i = 0; i < 5; i++) {
			top5pheno += " " + i + " - " + phenotypes [i].ToString ();
		}

		//print out top candidates
		Debug.Log (top3fit);
		Debug.Log (top5pheno);
			
	}

	//create fitness proportionate pool of potential breeding partners (similar to unscaled CDF)
	public List<Phenotype> Cdf(){

		List<Phenotype> cdf = new List<Phenotype>();

		for (int i = 0; i < phenotypes.Length; i++) {
			for (int j = 0; j < fitnesses [i]; j++) {
				cdf.Add (phenotypes [i]);
			}
		}
		return cdf;
	}
		
	//returns an array with frequency of the top 3 fitnesses weighted by occurence i.e cdf
	public Stack<Phenotype> Selection(string type){

		SortByFitness ();

		//List<Phenotype> selectionPool = new List<Phenotype>();
		Stack<Phenotype> selectionPool = new Stack<Phenotype>();

		switch (type) 
		{

		//choose sample of size n from the population, and places the highest fitness one in the selection pool
		case "tournament":
			
			for (int i = 0; i < size; i++) {

				//choose two values (n = 2)
				int r = UnityEngine.Random.Range (0, size); 
				int r2 = UnityEngine.Random.Range (0, size); 
				Phenotype best = fitnesses [r] >= fitnesses [r2] ? phenotypes [r] : phenotypes [r2];
				selectionPool.Push(best);
			}

			break;

		//random selection of breeding phenotypes from the cdf
		case "fitnessProportional":
			
			List<Phenotype> cdf = Cdf ();

			for (int i = 0; i < size; i++) {
				int r = UnityEngine.Random.Range (0, cdf.Count); 
				selectionPool.Push(cdf[r]);
			}
			break;

		//step through the cdf in steps of cdf.count / size, and picks random item in each step range
		case "stochasticUniversalSampling":

			cdf = Cdf ();


			for (int i = 0; i < size; i++) {
				int r = UnityEngine.Random.Range ((cdf.Count / size) * i, (cdf.Count / size) * (i + 1)); 
				selectionPool.Push( cdf [r]);
			}
			break;
		
		//takes the top 5 by fitness and then fills the selection pool with size/5 of those
		case "truncated":
			
			for (int i = 0; i < 5; i++) {
				for (int j = 0; j < (size / 5); j++) {
					//phenotypes is already ordered so it's fairly simple
					selectionPool.Push( phenotypes[i]);
				}
			}
			break;

		}

		//shuffle the selection pool
		System.Random rand = new System.Random();
		selectionPool = new Stack<Phenotype>(selectionPool.OrderBy (x => rand.Next ()));

		return selectionPool;

	}

	//generates the next generation pool and evolves them
	public void NextGen(){

		Stack<Phenotype> selectionPool = Selection (selectionType);

		phenotypes = new Phenotype[size];
		fitnesses = new int[size];

		//loop over pairs, as need two parents per two children
		for (int i = 0; i < size; i += 2) {

			//add the two new children to the new phenotypes for this generation (have been shuffled previously)

			phenotypes [i] = new Phenotype (selectionPool.Pop().phenotype);
			phenotypes [i + 1] = new Phenotype (selectionPool.Pop().phenotype);

			//evolve them
			Evolution.Evolve (phenotypes [i], phenotypes [i + 1], mutationRate, crossoverType, mutationType);

			//add their fitnesses to the new fitnesses for this generation
			fitnesses [i] = phenotypes [i].Fitness (targetString);
			fitnesses [i + 1] = phenotypes [i + 1].Fitness (targetString);
		}
			
		generation++;

	}
		
	public override string ToString ()
	{
		return (name + " : Generation  #" + generation.ToString () + "\n" + MostFit () + " has the highest fitness of " + maxFitness + "\n\n");
	}
}