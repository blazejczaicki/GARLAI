using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TopShooter;
using UnityEngine;
using Jackyjjc.Bayesianet;
using UnityEngine.SceneManagement;

public class GameManagerMLA : MonoBehaviour
{
	public static GameManagerMLA instance;

	[SerializeField] private MapData boardTemplate;
	[SerializeField] private PlayerMLA playerMLATemplate;

	[SerializeField] private Material mat;

	[SerializeField] private DataAiUi ui;

	[SerializeField] private Transform boardsParent;
	[SerializeField] private Transform playersParent;

	[SerializeField] [HideInInspector] private List<MapData> boards = new List<MapData>();
	[SerializeField] [HideInInspector] private List<PlayerMLA> players = new List<PlayerMLA>();
	[SerializeField] [HideInInspector] private List<NewSpawner> spawners = new List<NewSpawner>();

	[SerializeField] private float simRounds = 60;
	[SerializeField] private float currentRound = 0;
	[SerializeField] private float maxPlayerHealth = 20;

	[SerializeField] private float roundTimeSpan = 90;
	private float nextResetTime = 0;

	[SerializeField] private int width = 3;
	[SerializeField] private int height = 3;


	private float previousUpdateTime = 0;
	private bool allPlayersDead = false;

	public float tt =0;

	public float MaxPlayerHealth { get => maxPlayerHealth; set => maxPlayerHealth = value; }
	public float RoundTimeSpan { get => roundTimeSpan; set => roundTimeSpan = value; }

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
		{
			var t = Time.time;
			//nextResetTime = RoundTimeSpan;
			previousUpdateTime = t;
			foreach (var player in players)
			{
				player.PlayerEnt.OnStart(t);
			}
		}
		simRounds = SceneComunicator.instance.generationLimits;
		roundTimeSpan = SceneComunicator.instance.time;
		ui.ShowSimData((int)simRounds, (int)currentRound, SceneComunicator.instance.iterations, SceneComunicator.instance.currentIT);

		foreach (var p in players)
		{
			p.Speed = SceneComunicator.instance.playerSpeed;
		}
		foreach (var s in spawners)
		{
			s.enemySpeed = SceneComunicator.instance.enemySpeed;
			s.attackDistanceThreshold = SceneComunicator.instance.attackDistanceThreshold;
		}
	}


	private void Update()
	{
		ui.ShowTime(Time.time - previousUpdateTime);
		
		{
			//tu ifa ¿e koniec
			UpdatePlayers();
			UpdateEnemies();

			if (Time.time> RoundTimeSpan* simRounds)
			{
				Debug.Log("Koniec");
				FinishCycle();
			}
				//if (Time.time > nextResetTime || allPlayersDead)// 
			//	if (Time.time - previousUpdateTime > RoundTimeSpan || allPlayersDead)
			//{
			//	Debug.Log("MARTWI AGENTSI");
			//	players.ForEach(p => p.OnEndGeneration());
			//	currentRound++;
			//	if (simRounds == currentRound)
			//	{
			//		FinishCycle();
			//	}
			//	previousUpdateTime = Time.time;
			//	ui.ShowSimData((int)simRounds, (int)currentRound, SceneComunicator.instance.iterations, SceneComunicator.instance.currentIT);
			//	allPlayersDead = false;
			//	float tt = Time.time;
			//	players.ForEach(x => x.OnNewGeneration());
			//	//nextResetTime = Time.time + RoundTimeSpan;
			//}
		}
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Time.timeScale = Time.timeScale == 1 ? 0 : 1;
		}
	}

	private void UpdatePlayers()
	{
		allPlayersDead = true;
		float t = Time.time;
		foreach (var player in players)
		{
			player.OnUpdate(t);
			//if (player.gameObject.activeSelf)
			//{
			//	allPlayersDead = false;
				
			//}
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

	public void ResetWorld()
	{
		boards.ForEach(x => x.ResetMapWorld());
		spawners.ForEach(x => x.ResetSpawnersWorld());
	}

	private void FinishCycle()
	{
		SceneComunicator.instance.currentIT++;
		SceneComunicator.instance.isChanged = true;
		Destroy(gameObject);
		SceneManager.LoadScene(0);
		//Time.timeScale = Time.timeScale == 1 ? 0 : 1;
		//Debug.Log("Koniec");
	}

	[EasyButtons.Button]
	private void CreateTestWorld()
	{
		boards.Clear();
		spawners.Clear();
		players.Clear();
		int index = 0;

		int offsetDistance = 50;

		PlayerMLA templatePlayer =  playerMLATemplate;
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				var newBoard = Instantiate(boardTemplate) as MapData;
				boards.Add(newBoard);
				newBoard.transform.SetParent(boardsParent);
				newBoard.OriginPoint = new Vector3(j * offsetDistance, 0, i * offsetDistance); //j to x, i to z
																	   //newBoard.CreateBoardContent(new Vector3(j*25,0,i*25));
				newBoard.transform.position = new Vector3(newBoard.OriginPoint.x + newBoard.Width * 0.5f, 0, newBoard.OriginPoint.z + newBoard.Height * 0.5f);

				var newPlayerAI = Instantiate(templatePlayer) as PlayerMLA;
				newPlayerAI.gameObject.name = "Player " + index;
				players.Add(newPlayerAI);
				newPlayerAI.transform.SetParent(playersParent);
				newPlayerAI.MapData = newBoard;
				index++;
				var newSpawner = newBoard.GetComponent<NewSpawner>();
				newSpawner.PlayerMLA = newPlayerAI;
				spawners.Add(newSpawner);

				newBoard.newSpawner = newSpawner;
			}
		}
		foreach (var bo in boards)
		{
			CombineMeshTiles(bo.transform);
		}
	}

	public void CombineMeshTiles(Transform board)
	{
		var tiles = board.GetChild(0);
		var oldPos = tiles.transform.position;
		tiles.transform.position = Vector3.zero;
		MeshFilter[] meshFilters = tiles.GetComponentsInChildren<MeshFilter>();
		CombineInstance[] combine = new CombineInstance[meshFilters.Length];

		int i = 0;
		while (i < meshFilters.Length)
		{
			combine[i].mesh = meshFilters[i].sharedMesh;
			combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
			meshFilters[i].gameObject.SetActive(false);
			DestroyImmediate(meshFilters[i].gameObject);
			i++;
		}
		for (int j = 0; j < 10; j++)
		{
			foreach (Transform tile in tiles)
			{
				DestroyImmediate(tile.gameObject);
			}
		}
		tiles.transform.position = oldPos;
		tiles.gameObject.AddComponent<MeshRenderer>().sharedMaterial = mat;
		tiles.gameObject.AddComponent<MeshFilter>().sharedMesh = new Mesh();
		tiles.GetComponent<MeshFilter>().sharedMesh.CombineMeshes(combine);
		tiles.gameObject.SetActive(true);
	}
}
