using System.Collections;
using System.Collections.Generic;
using TopShooter;
using UnityEngine;
using Jackyjjc.Bayesianet;

public class GA_BayesChromosome : GA_Chromosome
{
    public List<double[]> bayesianGenes { get; set; }
    public List<BayesianNode> nodes { get; set; }


	public GA_BayesChromosome(PlayerAI player) : base(player)
	{
        bayesianGenes = new List<double[]>();
        nodes = player.BayesianNet.GetNodes();
        foreach (var n in nodes)
        {
            bayesianGenes.Add(n.values);
        }
    }

	public GA_BayesChromosome(GA_BayesChromosome crom):base(crom)
	{
        bayesianGenes = new List<double[]>();
		foreach (var valArray in crom.bayesianGenes)
		{
            double[] arr = new double[valArray.Length];
            valArray.CopyTo(arr,0);
            bayesianGenes.Add(arr);
		}
    }


    public override void SetData(PlayerAI player)
    {
        base.SetData(player);
        nodes = player.BayesianNet.GetNodes();
		for (int i = 0; i < nodes.Count; i++)
		{
            nodes[i].ResetNode(bayesianGenes[i]);
        }
    }

    public override void InitChromosome()
    {
        foreach (var gen in Genes)
        {
            CalculateRandomVal(gen);
        }
        for (int i = 0; i < nodes.Count; i++)
        {
            CalculateRandomValBayesian(bayesianGenes[i]);
            //nodes[i].ResetNode(bayesianGenes[i]);
        }
    }

    public override void Crossover(GA_Chromosome otherCrom)
    {
        GA_BayesChromosome otherBayesCrom = otherCrom as GA_BayesChromosome;
        int crossoverPoint = Random.Range(0, bayesianGenes.Count);
		for (int i = crossoverPoint; i < bayesianGenes.Count; i++)
		{
            var tmpGen = new double[bayesianGenes[i].Length];
            var tmpGenOther = new double[bayesianGenes[i].Length];
            bayesianGenes[i].CopyTo(tmpGen, 0);
            otherBayesCrom.bayesianGenes[i].CopyTo(tmpGenOther,0);
            bayesianGenes[i] = tmpGenOther;
            otherBayesCrom.bayesianGenes[i] = tmpGen;
        }
    }

    public override void Mutate()
    {
        var index = (int)Random.Range(0, Genes.Count - 0.01f);
        CalculateRandomVal(Genes[index]);
        var bayesIndex= (int)Random.Range(0, bayesianGenes.Count - 0.01f);
        CalculateRandomValBayesian(bayesianGenes[bayesIndex]);
        //nodes[bayesIndex].ResetNode(bayesianGenes[bayesIndex]);
    }

    private void CalculateRandomValBayesian(double[] values)
    {
		for (int i = 0; i < values.Length; i+=2)
		{
            var newProbality= Random.Range(0.01f, 0.99f);
            values[i] = newProbality;
            values[i+1] = 1-newProbality;
        }
    }
}
