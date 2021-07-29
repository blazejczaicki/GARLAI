using System.Collections;
using System.Collections.Generic;
using TopShooter;
using UnityEngine;
namespace TopShooter
{
	public class GameManager : MonoBehaviour
	{
		public static GameManager instance;

		[SerializeField] private MapData boardTemplate;
		[SerializeField] private PlayerAI playerAiTemplate;

		[SerializeField] private Transform boardsParent;
		[SerializeField] private Transform playersParent;

		[SerializeField] [HideInInspector] private List<MapData> boards = new List<MapData>();
		[SerializeField] [HideInInspector] private List<PlayerAI> players = new List<PlayerAI>();
		[SerializeField] [HideInInspector] private List<NewSpawner> spawners = new List<NewSpawner>();

		[SerializeField] private float simTime = 60;
		[SerializeField] private float maxPlayerHealth = 20;

		[SerializeField] private float resetTimeSpan = 10;
		private float nextResetTime = 0;

		[SerializeField] private int width = 3;
		[SerializeField] private int height = 3;

		private GA_GeneticAlgorithm geneticAlgorithm;

		private bool allPlayersDead = false;

		public float SimTime { get => simTime; set => simTime = value; }
		public float MaxPlayerHealth { get => maxPlayerHealth; set => maxPlayerHealth = value; }

		private void Awake()
		{
			if (instance == null)
			{
				instance = this;
			}
			else
			{
				Destroy(gameObject);
			}
		}

		private void Start()
		{
			nextResetTime = resetTimeSpan;
			geneticAlgorithm = GetComponent<GA_GeneticAlgorithm>();
			//geneticAlgorithm.Init(players);
		}

		private void Update()
		{
			UpdatePlayers();
			UpdateEnemies();

			if (allPlayersDead)//Time.time > nextResetTime || 
			{
				Debug.Log("MARTWI AGENTSI");
				//nextResetTime = Time.time + resetTimeSpan;
				//geneticAlgorithm.UpdateAlgorithm();
				//ResetWorld();
			}
		}

		private void UpdatePlayers()
		{
			allPlayersDead = true;
			foreach (var player in players)
			{
				if (player.gameObject.activeSelf)
				{
					allPlayersDead = false;
					player.OnUpdate();
				}
			}
		}

		private void UpdateEnemies()
		{
			foreach (var b in boards)
			{
				foreach (var en in b.Enemies)
				{
					en.OnUpdate();
				}
			}
		}

		[EasyButtons.Button]
		private void CreateTestWorld()
		{
			boards.Clear();
			players.Clear();
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					var newBoard = Instantiate(boardTemplate) as MapData;
					boards.Add(newBoard);
					newBoard.transform.SetParent(boardsParent);
					newBoard.OriginPoint = new Vector3(j * 25, 0, i * 25); //j to x, i to z
																		   //newBoard.CreateBoardContent(new Vector3(j*25,0,i*25));
					newBoard.transform.position = new Vector3(newBoard.OriginPoint.x + newBoard.Width * 0.5f, 0, newBoard.OriginPoint.z + newBoard.Height * 0.5f);

					var newPlayerAI = Instantiate(playerAiTemplate) as PlayerAI;
					players.Add(newPlayerAI);
					newPlayerAI.transform.SetParent(playersParent);
					newPlayerAI.MapData = newBoard;

					var newSpawner = newBoard.GetComponent<NewSpawner>();
					newSpawner.PlayerAI = newPlayerAI;
					spawners.Add(newSpawner);
				}
			}
		}

		private void ResetWorld()
		{
			boards.ForEach(x => x.ResetMapWorld());
			spawners.ForEach(x => x.ResetSpawnersWorld());
			players.ForEach(x => x.Enemies.Clear());
		}
	}
}