using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar 
{
    private List<AstarNode> openList;
    private List<AstarNode> closedList;

    public List<AstarNode> FindPath(Vector2 startPos, Vector2 endPos)
    {
        AstarNode start = new AstarNode(startPos, true);
        AstarNode end = new AstarNode(endPos, true);

        openList = new List<AstarNode> { start };
        closedList = new List<AstarNode>();

        start.g = 0;
        start.h = DistanceCost(start, end);
        start.CalculateF();
        while (openList.Count > 0)
        {
            AstarNode currentNode = GetLowestF(openList);
            if (currentNode.position == end.position)
            {
                end = currentNode;
                return ComputePath(end);
            }
            openList.Remove(currentNode);
            closedList.Add(currentNode);
            foreach (var neighbourNode in GetNeighbours(currentNode))
            {
                if (closedList.Exists((x) => x.position == neighbourNode.position) != true && neighbourNode.isMoveable)
                {
                    int tentativeG = currentNode.g + DistanceCost(currentNode, neighbourNode);
                    if (tentativeG < neighbourNode.g)
                    {
                        neighbourNode.previousNode = currentNode;
                        neighbourNode.g = tentativeG;
                        neighbourNode.h = DistanceCost(neighbourNode, end);
                        neighbourNode.CalculateF();
                        if (openList.Exists((x) => x.position == neighbourNode.position) != true)
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

    private List<AstarNode> GetNeighbours(AstarNode current)
    {
        List<AstarNode> neighbourList = new List<AstarNode>();
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (true)
                {
                    //neighbourList.Add();
                }
            }
        }
        return neighbourList;
    }

    private int DistanceCost(AstarNode start, AstarNode end)
    {
        return (int)(Vector2.Distance(start.position, end.position) * 10.0f);
    }
}
