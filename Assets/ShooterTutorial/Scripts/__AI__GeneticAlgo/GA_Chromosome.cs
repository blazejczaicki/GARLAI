using System.Collections;
using System.Collections.Generic;
using TopShooter;
using UnityEngine;

public class GA_Chromosome
{
    /// <summary>
    /// all decisions of player decision tree, list should the same order, kind nad number decisions for each ai player
    /// </summary>
    private List<DataAI> genes;
    private float fitness = 0;
    private float mutationRate = 0.05f;
    private PlayerAI playerAI;


    public float Fitness { get => fitness; set => fitness = value; }
    public float MutationRate { get => mutationRate; set => mutationRate = value; }
	public PlayerAI PlayerAI { get => playerAI; set => playerAI = value; }
	public List<DataAI> Genes { get => genes; set => genes = value; }

	public GA_Chromosome(PlayerAI player)
	{
        PlayerAI = player;
        Genes = PlayerAI.DataAI;
    }
    
	public GA_Chromosome(GA_Chromosome crom)
	{
        Genes = crom.Genes;
    }

    public void SetData(PlayerAI player)
	{
        PlayerAI = player;
        playerAI.DataAI = Genes;
    }

	public void InitChromosome()
    {
        foreach (var gen in Genes)
        {
            CalculateRandomVal(gen);
        }
    }

    public void Mutate()
    {
        var index = (int)Random.Range(0, Genes.Count-0.01f);
        CalculateRandomVal(Genes[index]);
    }

    private void CalculateRandomVal(DataAI data)
	{
        data.currentVal = Random.Range(data.minVal, data.maxVal);
    }

    public void CalculateFitness()
    {
        fitness =(PlayerAI.GetAverageHealth()*PlayerAI.GetLifeTime())/(TopShooter.GameManager.instance.RoundTimeSpan * TopShooter.GameManager.instance.MaxPlayerHealth);//playerAI.LifeTime *
    }
}
