using Jackyjjc.Bayesianet;
using System.Collections;
using System.Collections.Generic;
using EasyButtons;
using UnityEngine;
using TopShooter;
using System.Linq;

public class BayesNet : MonoBehaviour
{
	private VariableElimination ve;

	[SerializeField] [Range(0, 10)] private int amountEnemies = 0;
	[SerializeField] [Range(0, 6)] private float enemiesDist = 6;

	private ObservationEnemiesAmount observationEnemiesAmount;
	private ObservationEnemiesDistance observationEnemiesDistance;
	private ActionRunAway actionRunAway;
	private ActionGoRandom actionGoRandom;

	private void Awake()
	{
		string networkJson = (Resources.Load("NewNetworkShort") as TextAsset).text;
		ve = new VariableElimination(new BayesianJsonParser().Parse(networkJson));

		observationEnemiesAmount = new ObservationEnemiesAmount();
		observationEnemiesDistance = new ObservationEnemiesDistance();
		actionRunAway = new ActionRunAway();
		actionGoRandom = new ActionGoRandom();
	}

	public List<BayesianNode> GetNodes()
	{
		return ve.GetNetwork().GetNodes().ToList();
	}

	[EasyButtons.Button]
	public Queue<Vector3> InferNet(PlayerAI player)
	{
		BayesianNetwork network = ve.GetNetwork();
		Proposition enemyAmountProp = network.FindNode("enemy_amount").Instantiate(observationEnemiesAmount.GetEnemyAmount(player));
		Proposition distanceProp = network.FindNode("enemy_distance").Instantiate(observationEnemiesDistance.GetEnemyDistance(player));
		
		BayesianNode runAwayNode = ve.GetNetwork().FindNode("run_away");
		double[] runawayDistribution = ve.Infer(runAwayNode, enemyAmountProp, distanceProp);
		bool runaway = ve.PickOne(runawayDistribution) == runAwayNode.var.GetTokenIndex("True");

		Proposition runwayProp = network.FindNode("run_away").Instantiate(runaway.ToString());
		BayesianNode goRandomNode = ve.GetNetwork().FindNode("go_random");
		double[] goRandomDistribution = ve.Infer(runAwayNode, enemyAmountProp, runwayProp);
		bool goRandom = ve.PickOne(goRandomDistribution) == goRandomNode.var.GetTokenIndex("True");
		
		return ReleaseDecisions(runaway, goRandom, player);
	}

	private string GetEnemyAmount(int enemyAmount)
	{
		string result;
		if (enemyAmount <= 3) result = "Underwhelm";
		else result = "Overwhelm";
		return result;
	}

	private string GetEnemyDistance(float dist)
	{
		string result;
		if (dist >= 4) result = "Far";
		else result = "Near";
		return result;
	}

	private Queue<Vector3> ReleaseDecisions(bool isRunAway, bool isGoRandom, PlayerAI player)
	{
		Queue<Vector3> targetpos = new Queue<Vector3>();
		if (isRunAway)
		{
			Debug.Log("Run away");
			targetpos=actionRunAway.ReleaseAction(player);
		}
		else if (isGoRandom)
		{
			Debug.Log("go random");
			targetpos=actionGoRandom.ReleaseAction(player);
		}
		else
		{
			Debug.Log("Stay here");
			targetpos.Enqueue(player.transform.position);
		}
		return targetpos;
	}
}
