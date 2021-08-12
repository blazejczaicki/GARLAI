using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TopShooter;
using UnityEngine;

public class GA_GeneticAlgorithm : MonoBehaviour
{
    [SerializeField] private SaverCSV saverCSV;

    [SerializeField] private float crossoverRate = 0.4f;
    [SerializeField] private float mutationRate = 0.05f;

    private List<GA_Chromosome> chromosomes;
    private List<PlayerAI> players;
    private int chromosomeNumber = 100;
    private int genesNumber = 5;
    private float fitnessSum = 0;
    private DataChromosome bestResult;
    private int generation = 0;

	public List<GA_Chromosome> Chromosomes { get => chromosomes; set => chromosomes = value; }
	public int Generation { get => generation; }

	public void Init(List<PlayerAI> players)
    {
        this.players = players;
        chromosomes = new List<GA_Chromosome>();
        Debug.Log("init ga");
		foreach (var player in players)
		{
            player.chromosome.InitChromosome();
            chromosomes.Add(player.chromosome);
		}
    }

    public void InitChromosomes()//to po generacji plansz i graczy
    {
        chromosomes.ForEach(crom => crom.InitChromosome());
	}

	public void UpdateAlgorithm() //not referencje w kolejnych iteracjach chromosomy
    {
        ComputeFitness();
        CalculateFitnessSum();
        AppendBestResult();
        SaveToFile();
        Selection();
        Crossover();
        TryMutation();

        //tu set data
        for (int i = 0; i < Chromosomes.Count; i++)//update playera 
        {
            players[i].chromosome = Chromosomes[i];
            Chromosomes[i].SetData(players[i]);
        }

        generation++;
        fitnessSum = 0;
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
        Chromosomes=Chromosomes.OrderByDescending(c => c.Fitness).ToList();
        var cromBest = Chromosomes.First();
        bestResult = new DataChromosome(cromBest, cromBest.PlayerAI.name, cromBest.PlayerAI.GetAverageHealth(),
            cromBest.PlayerAI.GetLifeTime(), cromBest.Fitness);
    }

    public void SaveToFile()
    {
		foreach (var crom in chromosomes)
		{
            saverCSV.WriteToCSVGenerations(new DataChromosome(crom, crom.PlayerAI.name, crom.PlayerAI.GetAverageHealth(),
            crom.PlayerAI.GetLifeTime(), crom.Fitness), generation);
		}
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
				if (Chromosomes[i].PlayerAI.IsBayesian)// update tylko genow
				{
                    return new GA_BayesChromosome(Chromosomes[i] as GA_BayesChromosome);
                }
				else
				{
                    return new GA_DT_Chromosome(Chromosomes[i]);
				}
            }
        }
        return null;
    }

    public void Crossover() 
    {
        foreach (var crom in Chromosomes)
        {
            if (Random.Range(0, 1) < crossoverRate)
            {
                crom.Crossover(Chromosomes[(int)Random.Range(0, Chromosomes.Count)]);
            }
        }
    }

    public void TryMutation()
    {
        foreach (var chrom in Chromosomes)
        {
            if (Random.Range(0,1)<mutationRate)
            {
                chrom.Mutate();
            }
        }
    }

    public void OnEndSaveTheBest()
	{
        saverCSV.WriteToCSVFinal(bestResult, generation-1);
	}
}

public struct DataChromosome
{
    public GA_Chromosome chromosome;
    public string name;
    public float averageHealth;
    public float lifeTime;
    public float fitness;

	public DataChromosome(GA_Chromosome chromosome, string name, float averageHealth, float lifeTime, float fitness)
	{
		this.chromosome = chromosome;
		this.name = name;
		this.averageHealth = averageHealth;
		this.lifeTime = lifeTime;
		this.fitness = fitness;
	}
}
