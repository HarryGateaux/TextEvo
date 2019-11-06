using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Population {

	public string name;
	public Phenotype[] phenotypes;
	public int[] fitnesses;
	public string targetString;
	public int generation = 0;
	public int maxFitness;
	private int size;
	public float mutateRate = 0.1f;

	//class that defines population
	public Population(int _size){

		targetString = "helloworld";
		size = _size;
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
		
	//returns an array with frequency of the top 3 fitnesses weighted by occurence i.e cdf
	public List<Phenotype> Selection(){

		Phenotype[] top3 = new Phenotype[3];
		Phenotype[] pool = new Phenotype[100];
		List<Phenotype> poolNew = new List<Phenotype>();

		Array.Sort (fitnesses, phenotypes);
		Array.Reverse (fitnesses);
		Array.Reverse (phenotypes);
		Array.Copy (phenotypes, top3, 3);

		//adds each phenotype to the list based on its fitness
		for (int i = 0; i < phenotypes.Length; i++) {
			for (int j = 0; j < fitnesses [i]; j++) {
				poolNew.Add (phenotypes [i]);
			}
		}

		Debug.Log ("top 3 fitnesses " + fitnesses [0].ToString() + " " +  fitnesses [1].ToString() + " " +  fitnesses [2].ToString());

		string output = "top 10 phenotypes " ;

		for (int i = 0; i < 10; i++) {
			output += " " + i + " : " + phenotypes [i].ToString ();
		}

		Debug.Log (output);
		return poolNew;

	}

	//generates the next generation
	public void NextGen(){

		List<Phenotype> poolNew = Selection ();

		phenotypes = new Phenotype[size];
		fitnesses = new int[size];

		for (int i = 0; i < size; i++) {

			//pick a random item from the pool which is populated weighted by fitness
			int rando = UnityEngine.Random.Range (0, poolNew.Count); 
			int randoPart = UnityEngine.Random.Range (0, poolNew.Count); 

			string parentString = poolNew[rando].ToString ();
			string partnerString = poolNew[randoPart].ToString ();

			phenotypes [i] = new Phenotype (parentString);
			phenotypes [i].Evolve (partnerString, mutateRate);

			fitnesses [i] = phenotypes [i].Fitness (targetString);
		}

		generation++;

	}

		
	public override string ToString ()
	{
		return (name + " : Generation  #" + generation.ToString () + "\n" + MostFit () + " has the highest fitness of " + maxFitness + "\n\n");
	}
}