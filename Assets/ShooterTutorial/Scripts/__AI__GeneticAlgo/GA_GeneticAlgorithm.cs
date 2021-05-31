using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GA_GeneticAlgorithm : MonoBehaviour
{
    List<GA_Chromosome> chromosomes;
    private int chromosomeNumber = 100;
    private int genesNumber = 5;
    private float fitnessSum = 0;
    //coœ do zapisu danych do excela np.

    public void UpdateAlgorithm()
    {
        Fitness();
        CalculateFitnessSum();
        AppendBestResult();// klasa stats?
        Selection();
        TryMutation();
    }

    public void Fitness()
    {
        foreach (var chrom in chromosomes)
        {
            chrom.CalculateFitness();
        }
    }

    public void CalculateFitnessSum()
    {
        foreach (var chrom in chromosomes)
        {
            fitnessSum += chrom.Fitness;
        }
    }

    public void AppendBestResult()
    {
        chromosomes=chromosomes.OrderBy(c => c.Fitness).ToList();
        var best = chromosomes.First();
        // i tu zapis
    }
     
    public void CalculateAverageResult()
    {
        // i tu zapis
    }

    public void Selection()
    {
        List<GA_Chromosome> newChromosomes = new List<GA_Chromosome>();
        for (int i = 0; i < chromosomes.Count; i++)
        {
            newChromosomes.Add(SelectParent());
        }
        chromosomes = newChromosomes;
    }

    public GA_Chromosome SelectParent()
    {
        float rand = Random.Range(0, fitnessSum);
        rand = Random.Range(0, rand);
        float runningSum = 0;
        for (int i = 0; i < chromosomes.Count; i++)
        {
            runningSum += chromosomes[i].Fitness;
            if (runningSum>=rand)
            {
                return chromosomes[i];
            }
        }
        return null;
    }

    public void Crossover()
    {
        foreach (var crom in chromosomes)
        {

        }
    }

    public void TryMutation()
    {
        foreach (var chrom in chromosomes)
        {
            if (Random.Range(0,1)<chrom.MutationRate)
            {
                chrom.Mutate();
            }
        }
    }

    public void TerminateGA()
    {

    }
}
