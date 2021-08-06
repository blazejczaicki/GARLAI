using System.Collections;
using System.Collections.Generic;
using TopShooter;
using UnityEngine;

public class ActionRunAway
{
	public Queue<Vector3> ReleaseAction(PlayerAI player)
	{
        player.UpdateCloseEnemies(8);
        Vector3 averageDirection = player.GetAverageDirectionByEnemies();
        float escapeDist = 8;
        Vector3 escapePosition = player.transform.position + averageDirection * escapeDist;
        escapePosition = new Vector3(Mathf.Clamp(escapePosition.x, 0 + player.MapData.OriginPoint.x, 19.5f + player.MapData.OriginPoint.x), 0,
            Mathf.Clamp(escapePosition.z, 0 + player.MapData.OriginPoint.z, 19.5f + player.MapData.OriginPoint.z));
        Queue<Vector3> road = new Queue<Vector3>();
        road.Enqueue(escapePosition);
        return road;
	}
}
