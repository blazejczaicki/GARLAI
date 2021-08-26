using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TopShooter;
using UnityEngine;

public class ObservationEnemiesDistance 
{
	private string far = "Far";
	private string near = "Near";

	public string GetEnemyDistance(PlayerAI player, float v)
	{
		float minDist = 100;
		//float minDist = v;
		if (player.Enemies.Count > 0)
		{
			minDist = player.Enemies.Min(x => Vector3.Distance(x.transform.position, player.transform.position));
		}

		string result;
		if (minDist >= 4) result = far;
		else result = near;
		return result;
	}
}
