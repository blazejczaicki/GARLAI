using System.Collections;
using System.Collections.Generic;
using TopShooter;
using UnityEngine;

public abstract class GA_Chromosome
{
    protected float fitness = 0;
    protected PlayerAI playerAI;
    protected List<DataAI> genes;

    public List<DataAI> Genes { get => genes; set => genes = value; }
    public float Fitness { get => fitness; set => fitness = value; }
	public PlayerAI PlayerAI { get => playerAI; set => playerAI = value; }

    public GA_Chromosome()
    {    }

    public GA_Chromosome(PlayerAI player)
	{
        PlayerAI = player;
        Genes = PlayerAI.DataAI;
    }

    public GA_Chromosome(GA_Chromosome crom)
    {
        Genes = new List<DataAI>();
        crom.Genes.ForEach(x => Genes.Add(new DataAI(x)));
        //Genes = crom.Genes;
    }

    public virtual void SetData(PlayerAI player)
    {
        PlayerAI = player;
        playerAI.DataAI = Genes;
    }

    public abstract void InitChromosome();

    public abstract void Mutate();

    public abstract void Crossover(GA_Chromosome otherCrom);

    public void CalculateRandomVal(DataAI data)
    {
        data.currentVal = Random.Range(data.minVal, data.maxVal);
    }

    public void CalculateFitness()
    {
        fitness = (PlayerAI.GetAverageHealth() * PlayerAI.GetLifeTime()) / ((TopShooter.GameManager.instance.RoundTimeSpan-1) * TopShooter.GameManager.instance.MaxPlayerHealth);//playerAI.LifeTime *
    }
}
