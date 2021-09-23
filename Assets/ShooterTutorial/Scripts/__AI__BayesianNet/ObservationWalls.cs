using System.Collections;
using System.Collections.Generic;
using TopShooter;
using UnityEngine;

public class ObservationWalls
{
	private string near = "near";
	private string far = "far";

	private float minDist = 3;

	public string GetDistanceInfo(PlayerAI player, float v)
	{
		string result;
		if (IsTooCloseWall(player)) result = near;
		else result = far;
		return result;
	}

	private bool IsTooCloseWall(PlayerAI player)
	{
		return player.transform.position.x < player.MapData.OriginPoint.x + minDist || player.transform.position.x > player.MapData.OriginPoint.x + 19.5f - minDist ||
			player.transform.position.z < player.MapData.OriginPoint.z + minDist || player.transform.position.z > player.MapData.OriginPoint.z + 19.5f- minDist;
	}
}
