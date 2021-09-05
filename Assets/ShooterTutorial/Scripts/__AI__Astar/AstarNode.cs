using System.Collections;
using System.Collections.Generic;
using TopShooter;
using UnityEngine;

public class AstarNode
{
    public Vector2 position2d;
    public Vector3 position3d;
    public Vector2Int mapPosition;
    public Vector2 offsetPos;
    private const int gDefaultValue = 9999999;

    public int g;
    public int h;
    public int f;
    public int enemyInfluence;

    public bool isMoveable;
    public AstarNode previousNode;

    public MeshRenderer debugTile;

    public List<AstarNode> neighbours;

    public AstarNode(Vector2 position, Vector2Int mapPosition, bool moveable)
    {
        g = gDefaultValue;
        enemyInfluence = h = f = 0;
        isMoveable = moveable;
        previousNode = null;
        this.position2d = position;
        this.position3d = new Vector3(position.x+0.5f, 0, position.y + 0.5f);
        this.mapPosition = mapPosition;
        this.offsetPos = position;
        neighbours = new List<AstarNode>();
    }

    public void ResetData()
	{
        g = gDefaultValue;
        h = f = 0;
        previousNode = null;
    }

    public void CalculateF()
    {
        f = g + h+ enemyInfluence;
    }

    public void UpdateEnemyInfluence(List<Enemy> enemies, float influenceRadiusSqr, float scaler, float unitval, int enemiesCount, bool debugMode, int booster=10)
	{
        enemyInfluence = 0;
		for (int i = 0; i < enemiesCount; i++)
		{
            enemyInfluence +=(int)(System.Math.Max(0,influenceRadiusSqr - (enemies[i].transform.position - position3d).sqrMagnitude)/scaler);
		}
		//if (enemyInfluence>0)
		//{
  //          Debug.Log("xd");
		//}
        enemyInfluence *= booster;
        enemyInfluence *= enemyInfluence;
        float max = unitval * enemiesCount*booster;
        max *= max;
        if (debugMode)
        {
            debugTile.sharedMaterial.color =(debugTile.sharedMaterial.color==Color.yellow)? Color.yellow: new Color(enemyInfluence / max, 0, 0);
        }
	}

    public void SetNeighbours(List<List<AstarNode>> astarNodes, MapData mapData)
	{
		if (mapPosition.x>0)
		{
            neighbours.Add(astarNodes[mapPosition.x - 1][mapPosition.y]);
			if (mapPosition.y > 0)
			{
                neighbours.Add(astarNodes[mapPosition.x - 1][mapPosition.y-1]);
            }
			if (mapPosition.y < mapData.Height-1)
			{
                neighbours.Add(astarNodes[mapPosition.x - 1][mapPosition.y + 1]);
            }
		}
		if (mapPosition.x < mapData.Width-1)
		{
            neighbours.Add(astarNodes[mapPosition.x + 1][mapPosition.y]);
			if (mapPosition.y > 0)
			{
                neighbours.Add(astarNodes[mapPosition.x + 1][mapPosition.y-1]);
            }
			if (mapPosition.y < mapData.Height-1)
			{
                neighbours.Add(astarNodes[mapPosition.x + 1][mapPosition.y + 1]);
            }
		}
		if (mapPosition.y>0)
		{
            neighbours.Add(astarNodes[mapPosition.x][mapPosition.y - 1]);
        }
		if (mapPosition.y< mapData.Height-1)
		{
            neighbours.Add(astarNodes[mapPosition.x][mapPosition.y + 1]);
        }
	}
}
