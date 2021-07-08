using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeController : MonoBehaviour
{
    public AstarNode node;

    public Vector2 position;
    //public Vector2 mapPosition;
    //public int g;
    //public int h;
    public int f;
    //public bool isMoveable;

   // public MeshRenderer debugTile;

    //public AstarNode previousNode;
    public List<MeshRenderer> neighbours;

    [EasyButtons.Button]
    public void ShowData()
	{
        neighbours.Clear();
        position = node.position2d;
        f = node.f;
        node.neighbours.ForEach(x=>neighbours.Add(x.debugTile));
	}
}
