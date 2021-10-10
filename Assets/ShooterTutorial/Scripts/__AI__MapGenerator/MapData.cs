using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TopShooter;
using UnityEngine;

public class MapData : MonoBehaviour
{
    [SerializeField] private Vector3 originPoint;
    [SerializeField] private int width =20;
    [SerializeField] private int height =20;
    [SerializeField] private int seed =20;
    [SerializeField] private int enemyInfluenceRadius =5;
    [SerializeField] private int unitInfluenceLimit =25;
    private List<Enemy> enemies= new List<Enemy>();
    private List<List<AstarNode>> astarNodesMap;
    private Queue<AstarNode> defaultShuffledTileCoords;
    private Queue<AstarNode> shuffledTileCoords;

    [Header("debug height tiles")]
    [SerializeField] private Transform debugTileTemplate;
    [SerializeField] private Transform debugTileParent;
    [SerializeField] private bool debugMode = false;

    public NewSpawner newSpawner;

    private Enemy[] enemiesMLA = new Enemy[10];
    public Enemy[] EnemiesMLA { get => enemiesMLA; }

    public List<List<AstarNode>> AstarNodesMap { get => astarNodesMap; set => astarNodesMap = value; }
    public Vector3 OriginPoint { get => originPoint; set => originPoint = value; }
	public int Height { get => height; set => height = value; }
	public int Width { get => width; set => width = value; }
	public List<Enemy> Enemies { get => enemies; set => enemies = value; }
	public bool DebugMode { get => debugMode;}
	public int Seed { get => seed; set => seed = value; }

	private void Awake()
	{
		CreateBoardContent(originPoint);
	}

	[EasyButtons.Button]
    public void CreateBoardContent(Vector3 originPt)
	{
        originPoint = originPt;
        astarNodesMap = new List<List<AstarNode>>();

        Vector2 offsetPos = Vector2.zero;
             
            offsetPos.y = originPoint.z;
        for (int i = 0; i < Width; i++)
        {
            offsetPos.x = originPoint.x;

            astarNodesMap.Add(new List<AstarNode>());

            for (int j = 0; j < Height; j++)
            {
                astarNodesMap[i].Add(new AstarNode(offsetPos, new Vector2Int(i, j), true));
                var debugTile = Instantiate(debugTileTemplate);
                debugTile.transform.SetParent(debugTileParent);
                debugTile.transform.localPosition = new Vector3(j + 0.5f, 0.1f, i + 0.5f);
                var nCon = debugTile.GetComponent<NodeController>();
                nCon.node = astarNodesMap[i][j];
                astarNodesMap[i][j].debugTile = debugTile.GetComponent<MeshRenderer>();
                astarNodesMap[i][j].debugTile.sharedMaterial = new Material(astarNodesMap[i][j].debugTile.sharedMaterial);
                astarNodesMap[i][j].debugTile.sharedMaterial.color = Color.black;
				if (!DebugMode)
				{
                    astarNodesMap[i][j].debugTile.gameObject.SetActive(false);
                }
                offsetPos.x += 1;
            }
            offsetPos.y += 1;
        }
		for (int i = 0; i < Width; i++)
		{
			for (int j = 0; j < Height; j++)
			{
                astarNodesMap[i][j].SetNeighbours(astarNodesMap, this);
             }
		}
        seed = (int)SceneComunicator.instance.seed;
        if (SceneComunicator.instance.randomMode)
        {
            shuffledTileCoords = new Queue<AstarNode>(TopShooter.Utility.ShuffleArray(AstarNodesMap.SelectMany(d => d).ToArray(), Random.Range(0, 99999)));
            defaultShuffledTileCoords = new Queue<AstarNode>(shuffledTileCoords);
        }
        else
        {
            shuffledTileCoords = new Queue<AstarNode>(TopShooter.Utility.ShuffleArray(AstarNodesMap.SelectMany(d => d).ToArray(), Seed));
            defaultShuffledTileCoords = new Queue<AstarNode>(shuffledTileCoords);
        }
    }

	private void Update()
	{
        UpdateHeightMap();
    }

    [EasyButtons.Button]
	public void UpdateHeightMap()
	{
        float sqrRad = enemyInfluenceRadius * enemyInfluenceRadius;
        float scaler = sqrRad / unitInfluenceLimit;
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                astarNodesMap[i][j].UpdateEnemyInfluence(enemies, sqrRad, scaler, unitInfluenceLimit, enemies.Count, debugMode);
            }
        }
    }

    public void ResetMapWorld()
	{
        ResetMapData();
        newSpawner.ResetSpawnersWorld();
        enemies.ForEach(x => { x.StopAllCoroutines(); Destroy(x.gameObject); });
        enemies.Clear();
        enemiesMLA = new Enemy[10];
        if (SceneComunicator.instance.randomMode)
		{
            shuffledTileCoords = new Queue<AstarNode>(TopShooter.Utility.ShuffleArray(AstarNodesMap.SelectMany(d => d).ToArray(), Random.Range(0, 99999)));
        }
		else
		{
            shuffledTileCoords = new Queue<AstarNode>(defaultShuffledTileCoords);
		}
	}

	public void ResetMapData()
	{
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                astarNodesMap[i][j].ResetData();
            }
        }
    }

	public Vector3 GetMapCenter()
    {
        return new Vector3(OriginPoint.x + Width * 0.5f, 0, OriginPoint.z + Height * 0.5f);
    }

    public Vector3 GetRandomPlace()
    {
        AstarNode randomNode = shuffledTileCoords.Dequeue();
        shuffledTileCoords.Enqueue(randomNode);
        return new Vector3(randomNode.position2d.x,0, randomNode.position2d.y);
    }

    public AstarNode GetRandomNode()
    {
        AstarNode randomNode = shuffledTileCoords.Dequeue();
        shuffledTileCoords.Enqueue(randomNode);
        return randomNode;
    }

    public Vector2Int ConvertToMapGridPos(Vector3 position)
	{
        var converted = new Vector2Int((int)(position.x - originPoint.x), (int)(position.z - originPoint.z));
		if (converted.x>19||converted.y>19||converted.x<0||converted.y<0)
		{
            Debug.Log("XD");
		}
        return converted;
	}
}
