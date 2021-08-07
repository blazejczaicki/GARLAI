using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar 
{
    private List<AstarNode> openList;
    private List<AstarNode> closedList;

    public List<AstarNode> FindPath(Vector2Int startPos, Vector2Int endPos, List<List<AstarNode>> astarNodes, MapData mapData)
    {
		//if (startPos.x>=20|| startPos.x < 0 || startPos.y >= 20 || startPos.y < 0 || endPos.x >= 20 || endPos.x < 0|| endPos.y >= 20 || endPos.x < 0)
		//{
  //          Debug.Log("xd");
		//}

        AstarNode start =astarNodes[startPos.y][startPos.x];
        AstarNode end = astarNodes[endPos.y][endPos.x];

        openList = new List<AstarNode> { start };
        closedList = new List<AstarNode>();

        start.g = 0;
        start.h = DistanceCost(start, end);
        start.CalculateF();
        while (openList.Count > 0)
        {
            AstarNode currentNode = GetLowestF(openList);
            if (currentNode.position2d == end.position2d)
            {
                end = currentNode;
                return ComputePath(end);
            }
            openList.Remove(currentNode);
            closedList.Add(currentNode);
            var neighbours = currentNode.neighbours;
            int count = neighbours.Count;
			for (int i = 0; i < count; i++)
            {
                if (closedList.Exists((x) => x.position2d == neighbours[i].position2d) != true && neighbours[i].isMoveable)
                {
                    int tentativeG = currentNode.g + DistanceCost(currentNode, neighbours[i]);
                    if (tentativeG < neighbours[i].g)
                    {
                        neighbours[i].previousNode = currentNode;
                        neighbours[i].g = tentativeG;
                        neighbours[i].h = DistanceCost(neighbours[i], end);
                        neighbours[i].CalculateF();
                        if (openList.Exists((x) => x.position2d == neighbours[i].position2d) != true)
                        {
                            openList.Add(neighbours[i]);
                        }
                    }
                }
            }
        }
        return null;
    }
    
    private AstarNode GetLowestF(List<AstarNode> nodes)
    {
        AstarNode lowestFNode = nodes[0];
        int count= nodes.Count;
		for (int i = 0; i < count; i++)
        {
            if (nodes[i].f < lowestFNode.f)
            {
                lowestFNode = nodes[i];
            }
        }
        return lowestFNode;
    }

    private List<AstarNode> ComputePath(AstarNode end)
    {
        List<AstarNode> path = new List<AstarNode>();
        path.Add(end);
        AstarNode currenNode = end;
        while (currenNode.previousNode != null)
        {
            path.Add(currenNode.previousNode);
            currenNode = currenNode.previousNode;
        }
        path.Reverse();
        return path;
    }

    private int DistanceCost(AstarNode start, AstarNode end)
    {
        return (int)(Vector2.Distance(start.position2d, end.position2d) * 10.0f);
    }
}
