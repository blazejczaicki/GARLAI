using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar 
{
    private List<AstarNode> openList;
    private List<AstarNode> closedList;

    public List<AstarNode> FindPath(Vector2Int startPos, Vector2Int endPos, List<List<AstarNode>> astarNodes, MapData mapData)
    {
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
            foreach (var neighbourNode in neighbours)
            {
                if (closedList.Exists((x) => x.position2d == neighbourNode.position2d) != true && neighbourNode.isMoveable)
                {
                    int tentativeG = currentNode.g + DistanceCost(currentNode, neighbourNode);
                    if (tentativeG < neighbourNode.g)
                    {
                        neighbourNode.previousNode = currentNode;
                        neighbourNode.g = tentativeG;
                        neighbourNode.h = DistanceCost(neighbourNode, end);
                        neighbourNode.CalculateF();
                        if (openList.Exists((x) => x.position2d == neighbourNode.position2d) != true)
                        {
                            openList.Add(neighbourNode);
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
        foreach (var node in nodes)
        {
            if (node.f < lowestFNode.f)
            {
                lowestFNode = node;
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
