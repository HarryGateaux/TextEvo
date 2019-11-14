using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Population
{
    public string _name;

    public int _generation = 0;
    public int _bestFitness;
    public int _size;
    
    public string _bestGenome;
    public Genome[] _genomes;
    public int[] _fitnesses;

    public Population(int size, string targetString)
    {
        _size = size;
        Seed(targetString);
    }

    //creates first generation
    public void Seed(string targetString)
    {
        _genomes = new Genome[_size];

        for (int i = 0; i < _size; i++)
        {
            _genomes[i] = new Genome(targetString.Length);
        }
        _generation++;
    }
}


public class Fitness
{ 
    public string _targetString;

    public Fitness (string targetString)
    {
        _targetString = targetString;
    }

    public Population Apply(Population p)
    {
        GenerateFitnesses(p);
        SortByFitness(p);
        return p;
    }

    //calculates the fitnesses for a population
    private void GenerateFitnesses(Population p)
    {
        p._fitnesses = new int[p._size];

        for (int i = 0; i < p._size; i++)
        {
            p._fitnesses[i] = FitnessFunction(p._genomes[i]);
        }
    }

    //calculates an individual genomes fitness
    private int FitnessFunction(Genome g)
    {
        int fitness = 0;
        //calculates the difference in characters between the candidate genome and target
        for (int i = 0; i < g.genes.Length; i++)
        {

            if (g.genes[i] == _targetString[i])
            {
                fitness++;
            }
        }
        return fitness;
    }

    //sorts the genomes and fitness by fitness descending order, and records best
    public Population SortByFitness(Population p)
    {
        Array.Sort(p._fitnesses, p._genomes);
        Array.Reverse(p._fitnesses);
        Array.Reverse(p._genomes);

        p._bestGenome = p._genomes[0].genome;
        p._bestFitness = p._fitnesses[0];

        string top3fit = String.Format("{0} Population : Generation {1} : Top 3 Fitnesses {2}, {3}, {4}", p._name, p._generation, p._fitnesses[0].ToString(), p._fitnesses[1].ToString(), p._fitnesses[2].ToString());
        string top5pheno = String.Format("{0} Population : Generation  {1} : Top 5 genomes ", p._name, p._generation);

        for (int i = 0; i < 5; i++)
        {
            top5pheno += " " + i + " - " + p._genomes[i].ToString();
        }

        //print out top candidates
        Debug.Log(top3fit);
        Debug.Log(top5pheno);

        return p;
    }
}

public class Selection
{
    public string _selectionType;

    public Selection(string selectionType)
    {
        _selectionType = selectionType;
    }
    //problem here if all have zero fitnesses as the CDF is blank!
    public List<Genome> Cdf(Genome[] genomes, int[] fitnesses)
    {
        List<Genome> cdf = new List<Genome>();

        for (int i = 0; i < genomes.Length; i++)
        {
            for (int j = 0; j < fitnesses[i]; j++)
            {
                cdf.Add(genomes[i]);
            }
        }

        //if they all have zero fitness, just put one of each into the pool
        if (cdf.Count == 0)
        {
            for (int i = 0; i < genomes.Length; i++)
            {
                cdf.Add(genomes[i]);
            }
        }
        return cdf;
    }

    //returns an array with frequency of the top 3 fitnesses weighted by occurence i.e cdf
    public List<Genome> Select(Population p)
    {
        //local variables
        var selectionPool = new List<Genome>();
        var size = p._size;
        var genomes = p._genomes;
        var fitnesses = p._fitnesses;

        List<Genome> cdf;

        switch (_selectionType)
        {

            //choose sample of size n from the GA, and places the highest fitness one in the selection pool
            case "tournament":

                for (int i = 0; i < size; i++)
                {

                    //choose two values (n = 2)
                    int r = UnityEngine.Random.Range(0, size);
                    int r2 = UnityEngine.Random.Range(0, size);
                    Genome best = fitnesses[r] >= fitnesses[r2] ? genomes[r] : genomes[r2];
                    selectionPool.Add(best);
                }

                break;

            //random selection of breeding genomes from the cdf
            case "fitnessProportional":

                cdf = Cdf(genomes, fitnesses);
                if(cdf.Count == 0) { Debug.Log("null cdf"); }

                for (int i = 0; i < size; i++)
                {
                    int r = UnityEngine.Random.Range(0, cdf.Count);
                    selectionPool.Add(cdf[r]);
                }
                break;

            //step through the cdf in steps of cdf.count / size, and picks random item in each step range
            case "stochasticUniversalSampling":


                cdf = Cdf(genomes, fitnesses);
                if (cdf.Count == 0) { Debug.Log("null cdf"); }

                for (int i = 0; i < size; i++)
                {
                    int r = UnityEngine.Random.Range((cdf.Count / size) * i, (cdf.Count / size) * (i + 1));
                    selectionPool.Add(cdf[r]);
                }
                break;

            //takes the top 5 by fitness and then fills the selection pool with size/5 of those
            case "truncated":

                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < (size / 5); j++)
                    {
                        //genomes is already ordered so it's fairly simple
                        selectionPool.Add(genomes[i]);
                    }
                }
                break;

        }

        //shuffle the selection pool
        System.Random rand = new System.Random();
        selectionPool = new List<Genome>(selectionPool.OrderBy(x => rand.Next()));

        return selectionPool;
    }
}


public class CrossOver
{
    public string _crossoverType;

    public CrossOver(string crossoverType )
    {
        _crossoverType = crossoverType;
    }

    public Population Apply(Population p, List<Genome> selectionPool)
    {
        p._genomes = new Genome[p._size];
        p._fitnesses = new int[p._size];
        Stack<Genome> selectionStack = new Stack<Genome>(selectionPool);

        //loop over pairs, as need two parents per two children
        for (int i = 0; i < p._size; i += 2)
        { 
            //add the two new children to the new genomes for this generation (have been shuffled previously)

            p._genomes[i] = new Genome(selectionStack.Pop().genome);
            p._genomes[i + 1] = new Genome(selectionStack.Pop().genome);
            //evolve them
            Cross(p._genomes[i], p._genomes[i + 1]);

        }
        return p;
    }


    //takes two genomes as input and mutates them
    public void Cross(Genome p1, Genome p2)
    {

        int xPt1 = UnityEngine.Random.Range(0, p1.genome.Length);
        string crossedString1, crossedString2;

        switch (_crossoverType)
        {
            //cuts the genotype at one point and swaps genes
            case "OnePt":

                crossedString1 = p1.genome.Substring(0, xPt1) + p2.genome.Substring(xPt1);
                p1 = new Genome(crossedString1);

                crossedString2 = p2.genome.Substring(0, xPt1) + p1.genome.Substring(xPt1);
                p2 = new Genome(crossedString2);

                break;

            //cuts the genotype at two points and swaps genes
            case "TwoPt":

                int xPt2 = UnityEngine.Random.Range(0, p1.genome.Length);

                if (xPt1 <= xPt2)
                {
                    crossedString1 = p1.genome.Substring(0, xPt1) + p2.genome.Substring(xPt1, xPt2 - xPt1) + p1.genome.Substring(xPt2);
                    crossedString2 = p2.genome.Substring(0, xPt1) + p1.genome.Substring(xPt1, xPt2 - xPt1) + p2.genome.Substring(xPt2);
                }
                else
                {
                    crossedString1 = p2.genome.Substring(0, xPt2) + p1.genome.Substring(xPt2, xPt1 - xPt2) + p2.genome.Substring(xPt1);
                    crossedString2 = p1.genome.Substring(0, xPt2) + p2.genome.Substring(xPt2, xPt1 - xPt2) + p1.genome.Substring(xPt1);
                }
                p1 = new Genome(crossedString1);
                p2 = new Genome(crossedString2);
                break;

            //swaps genes by index depending on the swap rate
            case "Uniform":

                float swapRate = 0.1f;
                char[] chars1 = p1.genome.ToCharArray();
                char[] chars2 = p2.genome.ToCharArray();

                for (int i = 0; i < p1.genome.Length; i++)
                {

                    //if under swapRate swap genes at index
                    if (UnityEngine.Random.Range(0f, 1f) < swapRate)
                    {
                        char temp = chars1[i];
                        chars1[i] = chars2[i];
                        chars2[i] = temp;
                    }
                }

                p1 = new Genome(new string(chars1));
                p2 = new Genome(new string(chars2));

                break;
        }
    }
}


public class Mutation
{
    public string _mutationType;

    public Mutation(string mutationType)
    {
        _mutationType = mutationType;
    }

    public Population Apply(Population p)
    {
        for (int i = 0; i < p._size; i += 1)
        {

            //evolve them
            Mutate(p._genomes[i], 0.1f);
            p._genomes[i] = p._genomes[i];

        }

        return p;

    }

    //takes genome as input and mutates it
    public void Mutate(Genome p1, float mutationRate)
    {

        switch (_mutationType)
        {

            //if chosen, gene is mutated to random choice in range
            case "randomChoice":

                for (int i = 0; i < p1.genes.Length; i++)
                {
                    //mutates based on the mutationRate
                    if (UnityEngine.Random.Range(0f, 1f) < mutationRate)
                    {

                        int randomChoice = UnityEngine.Random.Range(97, 122);
                        p1.genes[i] = (char)(randomChoice);

                    }
                }
       
                break;

            //if chosen, gene is tweaked up or down one index in the range
            case "stepUp":

                for (int i = 0; i < p1.genes.Length; i++)
                {
                    //mutates based on the mutationRate
                    if (UnityEngine.Random.Range(0f, 1f) < mutationRate)
                    {

                        int intLetter = Convert.ToInt32(p1.genes[i]);
                        intLetter = ((intLetter + 1) - 97) % (122 - 97);
                        p1.genes[i] = (char)(intLetter + 97);
                    }
                }

                break;

            //if chosen, gene is tweaked by an amount determined by gaussian mean = 0 variance = sigma
            case "gaussianConvolution":


                break;

        }

        p1.genome = new string(p1.genes);



    }


}

public class GeneticAlgo
{
    private Population _population;
    private Fitness _fitness;
    private Selection _selection;
    private CrossOver _crossover;
    private Mutation _mutation;
    public string _name;

    public Population Population
    {
        get { return _population; }
        set { _population = value; }
    }

    public GeneticAlgo(Fitness fitness, Population population, Selection selection, CrossOver crossover, Mutation mutation)
    {
        Population = population;
        _fitness = fitness;
        _selection = selection;
        _crossover = crossover;
        _mutation = mutation;
    }

    public void NextGeneration()
    {

        var popOrderedByFitness = _fitness.Apply(Population);
        var parentSelection = _selection.Select(Population);
        var childrenCrossed = _crossover.Apply(popOrderedByFitness, parentSelection);
        var childrenMutated = _mutation.Apply(childrenCrossed);

        Population = childrenMutated;

        Population._generation++;

    }

    public override string ToString()
    {
        string output = String.Format("Population {0} : Selection : {1} Crossover : {2} Mutation : {3} \n " +
                        "<Size=18>Generation {4} <color=#000000>{5}</color> : Fitness = <color=#000000>{6} </color> </size>\n\n", Population._name,
                        _selection._selectionType, _crossover._crossoverType, _mutation._mutationType,
                        Population._generation.ToString(), Population._bestGenome, Population._bestFitness);

        return output;

    }

    public string CompleteString()
    {
        string output = String.Format("Population {0} : Selection : {1} Crossover : {2} Mutation : {3} \n " +
                 "<Size=18>Generation  {4} <color=#000000>{5} EVOLVED! </color></size>\n\n", Population._name,
                 _selection._selectionType, _crossover._crossoverType, _mutation._mutationType,
                 Population._generation.ToString(), Population._bestGenome, Population._bestFitness);

        return output;



    }


}

