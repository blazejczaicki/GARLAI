using System.Collections;
using System.Collections.Generic;
using TopShooter;
using UnityEngine;

public class AstarNode
{
    public Vector2 position2d;
    public Vector3 position3d;
    public Vector2 mapPosition;
    private const int gDefaultValue = 9999999;

    public int g;
    public int h;
    public int f;
    public int enemyInfluence;

    public bool isMoveable;
    public AstarNode previousNode;

    public MeshRenderer debugTile;

    public List<AstarNode> neighbours;

    public AstarNode(Vector2 position, Vector2 mapPosition, bool moveable)
    {
        g = gDefaultValue;
        enemyInfluence = h = f = 0;
        isMoveable = moveable;
        previousNode = null;
        this.position2d = new Vector2((int)position.x, (int)position.y);
        this.position2d = position;
        this.position3d = new Vector3(position.y+0.5f, 0, position.x + 0.5f);
        this.mapPosition = mapPosition;
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

    public void UpdateEnemyInfluence(List<Enemy> enemies, float influenceRadiusSqr, float scaler, float unitval, int enemiesCount, int booster=2)
	{
        enemyInfluence = 0;
		for (int i = 0; i < enemiesCount; i++)
		{
            enemyInfluence +=(int)(System.Math.Max(0,influenceRadiusSqr - (enemies[i].transform.position - position3d).sqrMagnitude)/scaler);
		}
        enemyInfluence *= booster;
        float max = unitval * enemiesCount*booster;
        debugTile.sharedMaterial.color = new Color(enemyInfluence / max, 0, 0);
	}

    public void SetNeighbours(List<List<AstarNode>> astarNodes, MapData mapData)
	{
		if ((int)position2d.x>0)
		{
            neighbours.Add(astarNodes[(int)position2d.x - 1][(int)position2d.y]);
			if ((int)position2d.y > 0)
			{
                neighbours.Add(astarNodes[(int)position2d.x - 1][(int)position2d.y-1]);
            }
			if ((int)position2d.y < mapData.Height-1)
			{
                neighbours.Add(astarNodes[(int)position2d.x - 1][(int)position2d.y + 1]);
            }
		}
		if ((int)position2d.x < mapData.Width-1)
		{
            neighbours.Add(astarNodes[(int)position2d.x + 1][(int)position2d.y]);
			if ((int)position2d.y > 0)
			{
                neighbours.Add(astarNodes[(int)position2d.x + 1][(int)position2d.y-1]);
            }
			if ((int)position2d.y < mapData.Height-1)
			{
                neighbours.Add(astarNodes[(int)position2d.x + 1][(int)position2d.y + 1]);
            }
		}
		if ((int)position2d.y>0)
		{
            neighbours.Add(astarNodes[(int)position2d.x][(int)position2d.y - 1]);
        }
		if ((int)position2d.y< mapData.Height-1)
		{
            neighbours.Add(astarNodes[(int)position2d.x][(int)position2d.y + 1]);
        }
	}
}
