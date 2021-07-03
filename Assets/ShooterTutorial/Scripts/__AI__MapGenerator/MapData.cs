using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapData : MonoBehaviour
{
    [SerializeField] private Vector3 originPoint;
    [SerializeField] private int width =20;
    [SerializeField] private int height =20;
    [SerializeField] private int seed =20;
    private List<List<AstarNode>> astarNodesMap;
    private Queue<AstarNode> shuffledTileCoords;

    public List<List<AstarNode>> AstarNodesMap { get => astarNodesMap; set => astarNodesMap = value; }
    public Vector3 OriginPoint { get => originPoint; set => originPoint = value; }

    private void Awake()
    {
        astarNodesMap = new List<List<AstarNode>>();

        Vector2 offsetPos = Vector2.zero;
        offsetPos.x = originPoint.x;        
        for (int i = 0; i < width; i++)
        {
            offsetPos.y = originPoint.z;
            astarNodesMap.Add(new List<AstarNode>());
            for (int j = 0; j < height; j++)
            {
                offsetPos.y += 1;
                astarNodesMap[i].Add(new AstarNode(offsetPos, new Vector2(i, j),true));
            }
            offsetPos.x += 1;
        }
        shuffledTileCoords = new Queue<AstarNode>(TopShooter.Utility.ShuffleArray(AstarNodesMap.SelectMany(d => d).ToArray(), seed));
    }

    public Vector3 GetMapCenter()
    {
        return new Vector3(OriginPoint.x + width * 0.5f, 0, OriginPoint.z + height * 0.5f);
    }

    public Vector3 GetRandomPlace()
    {
        AstarNode randomNode = shuffledTileCoords.Dequeue();
        shuffledTileCoords.Enqueue(randomNode);
        return new Vector3(randomNode.position.x,0, randomNode.position.y);
    }
}
