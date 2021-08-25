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
	[SerializeField] [Range(0, 6)] private float wallDist = 6;

	private ObservationEnemiesAmount observationEnemiesAmount;
	private ObservationEnemiesDistance observationEnemiesDistance;
	private ObservationWalls observationWalls;
	private ActionRunAway actionRunAway;
	private ActionGoRandom actionGoRandom;

	private string enAmount= "enemy_amount";
	private string enDist= "enemy_distance";
	private string enWallDist= "wall_distance";
	private string runAwayStr= "run_away";
	private string gorandStr= "go_random";
	private string gobackStr= "go_back";
	private string surrStr= "surrounded";
	private string breaksurrStr= "break_surrounding";
	private string passStr= "pass_enemies";
	private string trueStr= "True";

	private void Awake()
	{
		string networkJson = (Resources.Load("NewNetworkShort") as TextAsset).text;
		ve = new VariableElimination(new BayesianJsonParser().Parse(networkJson));

		observationEnemiesAmount = new ObservationEnemiesAmount();
		observationEnemiesDistance = new ObservationEnemiesDistance();
		observationWalls = new ObservationWalls();
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
		Proposition enemyAmountProp = network.FindNode(enAmount).Instantiate(observationEnemiesAmount.GetEnemyAmount(player, amountEnemies));
		Proposition distanceProp = network.FindNode(enDist).Instantiate(observationEnemiesDistance.GetEnemyDistance(player, enemiesDist));
		Proposition wallDistProp = network.FindNode(enWallDist).Instantiate(observationWalls.GetDistanceInfo(player, wallDist));
		
		BayesianNode runAwayNode = ve.GetNetwork().FindNode(runAwayStr);
		double[] runawayDistribution = ve.Infer(runAwayNode, enemyAmountProp, distanceProp);
		bool isrunaway = ve.PickOne(runawayDistribution) == runAwayNode.var.GetTokenIndex(trueStr);

		Proposition runwayProp = network.FindNode(runAwayStr).Instantiate(isrunaway.ToString());
		BayesianNode goRandomNode = ve.GetNetwork().FindNode(gorandStr);
		double[] goRandomDistribution = ve.Infer(goRandomNode, enemyAmountProp, runwayProp);
		bool isgoRandom = ve.PickOne(goRandomDistribution) == goRandomNode.var.GetTokenIndex(trueStr);

		BayesianNode surrNode = ve.GetNetwork().FindNode(surrStr);
		double[] surrDistribution = ve.Infer(surrNode, wallDistProp, distanceProp);
		bool isSurrounded = ve.PickOne(surrDistribution) == surrNode.var.GetTokenIndex(trueStr);

		Proposition surrProp = network.FindNode(surrStr).Instantiate(isSurrounded.ToString());
		BayesianNode breaksurrNode = ve.GetNetwork().FindNode(breaksurrStr);
		double[] breaksurrDistribution = ve.Infer(breaksurrNode, surrProp, runwayProp);
		bool isbreakSurrounding = ve.PickOne(breaksurrDistribution) == breaksurrNode.var.GetTokenIndex(trueStr);

		BayesianNode passNode = ve.GetNetwork().FindNode(passStr);
		double[] passDistribution = ve.Infer(passNode, enemyAmountProp, runwayProp);
		bool isPassEnemies = ve.PickOne(passDistribution) == passNode.var.GetTokenIndex(trueStr);


		BayesianNode gobackNode = ve.GetNetwork().FindNode(gobackStr);
		double[] gobackNodeDistribution = ve.Infer(gobackNode, runwayProp, surrProp, wallDistProp);
		bool isgoBack = ve.PickOne(gobackNodeDistribution) == gobackNode.var.GetTokenIndex(trueStr);

		return ReleaseDecisions(isgoRandom, isgoBack, isbreakSurrounding, isPassEnemies, player);
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

	private Queue<Vector3> ReleaseDecisions(bool isgoRandom, bool isgoBack, bool isbreakSurrounding, bool isPassEnemies, PlayerAI player)
	{
		Queue<Vector3> targetpos = new Queue<Vector3>();
		if (isbreakSurrounding)
		{
			Debug.Log("surrr");
			//targetpos=actionRunAway.ReleaseAction(player);
		}
		else if (isPassEnemies)
		{
			Debug.Log("pass");
			//targetpos=actionGoRandom.ReleaseAction(player);
		}
		else if (isgoBack)
		{
			Debug.Log("go back");
			//targetpos=actionGoRandom.ReleaseAction(player);
		}
		else if (isgoRandom)
		{
			Debug.Log("go random");
			//targetpos=actionGoRandom.ReleaseAction(player);
		}
		else
		{
			Debug.Log("Stay here");
			//targetpos.Enqueue(player.transform.position);
		}
		targetpos.Enqueue(player.transform.position);
		return targetpos;
	}
}
