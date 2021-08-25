using System.Collections;
using System.Collections.Generic;
using TopShooter;
using UnityEngine;

public class ActionGoRandom
{
	public Queue<Vector3> ReleaseAction(PlayerAI player)
	{
		Vector3 bestPos = Vector3.zero;
		float enemyInfluence = float.MaxValue;
		for (int i = 0; i < 4; i++)
		{
			var randomNode = player.MapData.GetRandomNode();
			if (randomNode.enemyInfluence <= enemyInfluence)
			{
				bestPos = new Vector3(randomNode.position2d.x, 0, randomNode.position2d.y);//uwaga na konwersjê
				enemyInfluence = randomNode.enemyInfluence;
			}
		}
		Queue<Vector3> road = new Queue<Vector3>();
		road.Enqueue(bestPos);
		return road;
	}
}
