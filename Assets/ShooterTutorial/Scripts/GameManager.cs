using System.Collections;
using System.Collections.Generic;
using TopShooter;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField] private MapData boardTemplate;
	[SerializeField] private PlayerAI playerAiTemplate;

	[SerializeField] private Transform boardsParent;
	[SerializeField] private Transform playersParent;

	private List<MapData> boards = new List<MapData>();
	private List<PlayerAI> players = new List<PlayerAI>();


    [SerializeField] private float simTime = 60;
    [SerializeField] private float maxPlayerHealth = 20;

    [SerializeField] private int width = 3;
    [SerializeField] private int height = 3;

	public float SimTime { get => simTime; set => simTime = value; }
	public float MaxPlayerHealth { get => maxPlayerHealth; set => maxPlayerHealth = value; }

	private void Awake()
	{
		//CreateTestWorld();
	}

	private void SetPlayers()
	{

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
				newBoard.transform.position = new Vector3(newBoard.OriginPoint.x+ newBoard.Width*0.5f, 0, newBoard.OriginPoint.z + newBoard.Height * 0.5f);
				
				var newPlayerAI = Instantiate(playerAiTemplate) as PlayerAI;
				players.Add(newPlayerAI);
				newPlayerAI.transform.SetParent(playersParent);
				newPlayerAI.MapData = newBoard;

				var newSpawner = newBoard.GetComponent<NewSpawner>();
				newSpawner.PlayerAI = newPlayerAI;
			}
		}
	}

	private void Update()
	{
		
	}
}
