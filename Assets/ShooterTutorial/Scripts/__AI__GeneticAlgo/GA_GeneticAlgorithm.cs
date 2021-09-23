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

    private GA_Chromosome cromBest;

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

	public void UpdateAlgorithm(bool isBayes) //not referencje w kolejnych iteracjach chromosomy
    {
        ComputeFitness();
        CalculateFitnessSum();
        AppendBestResult();
        SaveToFile();
		if (isBayes)
		{
            SaveBNdata();
		}
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

    private void SaveBNdata()
	{
        var avgGenerationData= CalculateAvgBN();
        var first = chromosomes.First() as GA_BayesChromosome;
        List<string> names = new List<string>();
		for (int i = 2; i < first.nodes.Count; i++)
		{
            names.Add(first.nodes[i].Name);
        }
        saverCSV.SaveBayesianGenes(avgGenerationData, names, true, generation);

		if (generation==0 || generation==2 || generation==5 || generation==7 || generation==9)
		{
            var best = cromBest as GA_BayesChromosome;
            var bestData = best.bayesianGenes;
            names = new List<string>();
            for (int i = 2; i < first.nodes.Count; i++)
            {
                names.Add(best.nodes[i].Name);
            }
            List<double[]> bestbestData = new List<double[]>();
            for (int i = 2; i < bestData.Count; i++)
            {
                bestbestData.Add(bestData[i]);
            }
            saverCSV.SaveBayesianGenes(bestbestData, names, false, generation);
        }
	}

    private List<double[]> CalculateAvgBN()
    {
        List<double[]> res = new List<double[]>();
        var first = chromosomes.First() as GA_BayesChromosome;

        for (int i = 2; i < first.bayesianGenes.Count; i++)
        {
            res.Add(new double[first.bayesianGenes[i].Length]);
        }
        for (int i = 0; i < chromosomes.Count; i++)
        {
            var cr = chromosomes[i] as GA_BayesChromosome;
            int kind = 0;
            for (int k = 2; k < cr.bayesianGenes.Count; k++)
            {
                for (int j = 0; j < cr.bayesianGenes[k].Length; j++)
                {
                    res[kind][j] += cr.bayesianGenes[k][j];
                }
                kind++;
            }
        }
        for (int k = 0; k < res.Count; k++)
        {
            for (int j = 0; j < res[k].Length; j++)
            {
                res[k][j] = res[k][j] / (double)chromosomes.Count;
            }
        }
        return res;
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
        cromBest = Chromosomes.First();
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
        SaveAverageData();
		if (generation==0)
		{
            saverCSV.WriteToCSVFinal(bestResult, generation, true);
		}
    }

    public void SaveAverageData()
	{
        string name = "Avg " + generation;
        float avgHealth = 0;
        float avLifeTime = 0;
        float avgFitness = 0;
        List<DataAI> data = new List<DataAI>();
		for (int i = 0; i < chromosomes.First().PlayerAI.DataAI.Count; i++)
		{
            data.Add(new DataAI(chromosomes.First().PlayerAI.DataAI[i]));
		}

		foreach (var crom in chromosomes)
		{
            avgHealth += crom.PlayerAI.GetAverageHealth();
            avLifeTime += crom.PlayerAI.GetLifeTime();
            avgFitness += crom.Fitness;
            for (int i = 0; i < crom.PlayerAI.DataAI.Count; i++)
            {
                data[i].currentVal +=crom.PlayerAI.DataAI[i].currentVal;
            }
        }
        avgHealth /= (float)chromosomes.Count;
        avLifeTime /= (float)chromosomes.Count;
        avgFitness /= (float)chromosomes.Count;
        for (int i = 0; i < data.Count; i++)
        {
            data[i].currentVal /= (float)chromosomes.Count;
        }

        saverCSV.WriteToCSVAvg(new DataChromosome(null, name, avgHealth,avLifeTime, avgFitness), data, generation);
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
