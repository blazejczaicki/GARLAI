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
    private Queue<AstarNode> shuffledTileCoords;

    [SerializeField] private Transform debugTileTemplate;
    [SerializeField] private Transform debugTileParent;
    private List<List<Transform>> debugMap = new List<List<Transform>>();

    public List<List<AstarNode>> AstarNodesMap { get => astarNodesMap; set => astarNodesMap = value; }
    public Vector3 OriginPoint { get => originPoint; set => originPoint = value; }
	public int Height { get => height; set => height = value; }
	public int Width { get => width; set => width = value; }
	public List<Enemy> Enemies { get => enemies; set => enemies = value; }

	private void Awake()
    {
        astarNodesMap = new List<List<AstarNode>>();

        Vector2 offsetPos = Vector2.zero;
        offsetPos.x = originPoint.x;        
        for (int i = 0; i < Width; i++)
        {
            offsetPos.y = originPoint.z;
            astarNodesMap.Add(new List<AstarNode>());

            debugMap.Add(new List<Transform>());

            for (int j = 0; j < Height; j++)
            {                
                astarNodesMap[i].Add(new AstarNode(offsetPos, new Vector2(i, j),true));
                var debugTile = Instantiate(debugTileTemplate);
                debugTile.transform.position = new Vector3(offsetPos.y+0.5f, 0.1f,offsetPos.x + 0.5f);
                debugTile.transform.SetParent(debugTileParent);
                var nCon = debugTile.GetComponent<NodeController>();
                nCon.node = astarNodesMap[i][j];
                astarNodesMap[i][j].debugTile = debugTile.GetComponent<MeshRenderer>();
                astarNodesMap[i][j].debugTile.sharedMaterial = new Material(astarNodesMap[i][j].debugTile.sharedMaterial);
                astarNodesMap[i][j].debugTile.sharedMaterial.color=Color.black;
                debugMap[i].Add(debugTile);
                offsetPos.y += 1;
            }
            offsetPos.x += 1;
        }
		for (int i = 0; i < Width; i++)
		{
			for (int j = 0; j < Height; j++)
			{
                astarNodesMap[i][j].SetNeighbours(astarNodesMap, this);
             }
		}
        shuffledTileCoords = new Queue<AstarNode>(TopShooter.Utility.ShuffleArray(AstarNodesMap.SelectMany(d => d).ToArray(), seed));
    }

	private void Update()
	{
        UpdateHeightMap();
    }

	public void UpdateHeightMap()
	{
        float sqrRad = enemyInfluenceRadius * enemyInfluenceRadius;
        float scaler = sqrRad / unitInfluenceLimit;
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                astarNodesMap[i][j].UpdateEnemyInfluence(enemies, sqrRad, scaler, unitInfluenceLimit, enemies.Count);
            }
        }
    }



	public IEnumerator ResetMapData()
	{
        yield return null;
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

    public Vector2Int ConvertToMapGridPos(Vector3 position)
	{
        return new Vector2Int((int)position.x,(int)position.z);
	}
}
