using System.Collections;
using System.Collections.Generic;
using TopShooter;
using UnityEngine;

public class GA_DT_Chromosome : GA_Chromosome
{
    public GA_DT_Chromosome(PlayerAI player):base(player)
    {    }

    public GA_DT_Chromosome(GA_Chromosome crom):base(crom)
    {    }

    public override void InitChromosome()
    {
        foreach (var gen in Genes)
        {
            CalculateRandomVal(gen);
        }
    }

    public override void Mutate()
    {
        var index = (int)Random.Range(0, Genes.Count - 0.01f);
        CalculateRandomVal(Genes[index]);
    }
}
