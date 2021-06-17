using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData : MonoBehaviour
{
    [SerializeField] private Transform originPoint;
    [SerializeField] int width=15;
    [SerializeField] int height=15;
    private List<List<AstarNode>> astarNodesMap;

    public List<List<AstarNode>> AstarNodesMap { get => astarNodesMap; set => astarNodesMap = value; }
    public Transform OriginPoint { get => originPoint; set => originPoint = value; }

    private void Awake()
    {
        astarNodesMap = new List<List<AstarNode>>();

        Vector2 offsetPos = Vector2.zero;
        offsetPos.x = originPoint.position.x;        
        for (int i = 0; i < width; i++)
        {
            offsetPos.y = originPoint.position.z;
            astarNodesMap.Add(new List<AstarNode>());
            for (int j = 0; j < height; j++)
            {
                offsetPos.y += 1;
                astarNodesMap[i].Add(new AstarNode(offsetPos, new Vector2(i, j),true));
            }
            offsetPos.x += 1;
        }
    }
}
