using System.Collections;
using System.Collections.Generic;
using TopShooter;
using UnityEngine;
using Jackyjjc.Bayesianet;

public class GA_BayesChromosome : GA_Chromosome
{
    public List<double[]> bayesianGenes { get; set; }


	public GA_BayesChromosome(PlayerAI player) : base(player)
	{
        bayesianGenes = new List<double[]>();
        var nodes = player.BayesianNet.GetNodes();
        foreach (var n in nodes)
        {
            bayesianGenes.Add(n.values);
        }
    }

	public GA_BayesChromosome(GA_BayesChromosome crom):base(crom)
	{
	    
	}

    public override void SetData(PlayerAI player)
    {
        base.SetData(player);
        var nodes = player.BayesianNet.GetNodes();
		for (int i = 0; i < nodes.Count; i++)
		{

		}
    }

    public override void InitChromosome()
    {
        foreach (var gen in Genes)
        {
            CalculateRandomVal(gen);
        }
        foreach (var gen in bayesianGenes)
        {
            CalculateRandomValBayesian(gen);
        }
    }

    public override void Mutate()
    {
        var index = (int)Random.Range(0, Genes.Count - 0.01f);
        CalculateRandomVal(Genes[index]);
    }

    private void CalculateRandomValBayesian(double[] values)
    {

    }
}
