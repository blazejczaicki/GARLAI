using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TopShooter;
using UnityEngine;

public class GA_GeneticAlgorithm : MonoBehaviour
{
    [SerializeField] private SaverCSV saverCSV;
    private List<GA_Chromosome> chromosomes;
    private int chromosomeNumber = 100;
    private int genesNumber = 5;
    private float fitnessSum = 0;
    private GA_Chromosome bestResult;

	public List<GA_Chromosome> Chromosomes { get => chromosomes; set => chromosomes = value; }

    //coœ do zapisu danych do excela np.    

    public void Init(List<PlayerAI> players)
    {
        Debug.Log("init ga");
		foreach (var player in players)
		{
            player.Chromosome.InitChromosome();
		}
    }

    public void InitChromosomes()//to po generacji plansz i graczy
    {
        chromosomes.ForEach(crom => crom.InitChromosome());
	}

	public void UpdateAlgorithm()
    {
        ComputeFitness();
        CalculateFitnessSum();
        AppendBestResult();
        SaveToFile();
        Selection();
        //Crossover();
        TryMutation();
    }

    public void ComputeFitness()
    {
        foreach (var chrom in Chromosomes)
        {
            chrom.CalculateFitness();
        }
    }

    public void CalculateFitnessSum()
    {
        foreach (var chrom in Chromosomes)
        {
            fitnessSum += chrom.Fitness;
        }
    }

    public void AppendBestResult()
    {
        Chromosomes=Chromosomes.OrderBy(c => c.Fitness).ToList();
        bestResult = Chromosomes.First();
    }

    public void SaveToFile()
    {
        saverCSV.WriteToCSV(this);
    }

    public void CalculateAverageResult() //todo
    {
        // i tu zapis
    }

    public void Selection()
    {
        List<GA_Chromosome> newChromosomes = new List<GA_Chromosome>();
        for (int i = 0; i < Chromosomes.Count; i++)
        {
            newChromosomes.Add(SelectParent());
        }
        Chromosomes = newChromosomes;
    }

    public GA_Chromosome SelectParent()
    {
        float rand = Random.Range(0, fitnessSum);
        rand = Random.Range(0, rand);
        float runningSum = 0;
        for (int i = 0; i < Chromosomes.Count; i++)
        {
            runningSum += Chromosomes[i].Fitness;
            if (runningSum>=rand)
            {
                return Chromosomes[i];
            }
        }
        return null;
    }

    public void Crossover() //todo
    {
        foreach (var crom in Chromosomes)
        {

        }
    }

    public void TryMutation()
    {
        foreach (var chrom in Chromosomes)
        {
            if (Random.Range(0,1)<chrom.MutationRate)
            {
                chrom.Mutate();
            }
        }
    }

    public void TerminateGA() //todo
    {


    }
    public void ResetGA() //todo
    {

    }
}
