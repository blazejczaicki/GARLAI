using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardsGenerator : MonoBehaviour
{
    [SerializeField] private Transform boardTemplate;
    [SerializeField] private Transform tileTemplate;
    [SerializeField] private float width=15;
    [SerializeField] private float height=15;
    [SerializeField] private int boardNumX=1;
    [SerializeField] private int boardNumY=1;
    [SerializeField] private int boardOffset =20;

    private void Awake()
    {
        InstanceMultipleBoards();
    }

    public void CreateSingleBoard()
    {
        Transform board = new GameObject("Board").transform;
        var floorCollider= board.gameObject.AddComponent<BoxCollider>();
        floorCollider.center = new Vector3(width / 2.0f, 0, height / 2.0f);
        floorCollider.size = new Vector3(width, 0.1f, height);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Transform newTile = Instantiate(tileTemplate, new Vector3(i+0.5f, 0, j + 0.5f), tileTemplate.rotation, board);
            }
        }
    }

    public void InstanceMultipleBoards()
    {

    }
}
