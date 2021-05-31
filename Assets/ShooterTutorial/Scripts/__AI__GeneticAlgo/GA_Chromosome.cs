using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GA_Chromosome : MonoBehaviour
{
    /// <summary>
    /// all decisions of player decision tree, list should the same order, kind nad number decisions for each ai player
    /// </summary>
    private List<DT_Decision> genes;
    private float fitness = 0;
    private float mutationRate = 0.05f;

    public float Fitness { get => fitness; set => fitness = value; }
    public float MutationRate { get => mutationRate; set => mutationRate = value; }

    public void InitChromosome()
    {
        foreach (var gen in genes)
        {
            gen.InitDecisionValues();
        }
    }

    public void Mutate()
    {
        var index = (int)Random.Range(0, genes.Count-0.01f);
        genes[index].InitDecisionValues();
    }

    public void CalculateFitness()
    {

    }
}
