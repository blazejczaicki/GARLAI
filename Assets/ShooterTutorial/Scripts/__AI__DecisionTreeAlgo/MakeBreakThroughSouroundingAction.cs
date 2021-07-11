using System.Collections;
using System.Collections.Generic;
using TopShooter;
using UnityEngine;

public class MakeBreakThroughSouroundingAction : DT_Action
{
    public override Queue<Vector3> MakeAction(PlayerAI player)
    {
        //float angleOffset = 90;
        //float startAngle = 0;

        //List<List<Enemy>> enemiesQuarts = new List<List<Enemy>>();
        //var tooCloseEnemies = player.Enemies.FindAll(x => player.MinEnemyDistance > Vector3.Distance(player.transform.position, x.transform.position));
        //for (int i = 0; i < 4; i++)
        //{
        //    startAngle += angleOffset;
        //    float minAngle = startAngle - angleOffset * 0.5f;
        //    float maxAngle = startAngle + angleOffset * 0.5f;
        //    var dirAngleFurstum = Utility.DirectionFromAngle(startAngle);

        //    enemiesQuarts.Add(tooCloseEnemies.FindAll(x=>Vector3.Angle((player.transform.position - x.transform.position).normalized, dirAngleFurstum) > angleOffset * 0.5f));

        //    //Debug.DrawRay(player.transform.position, dirAngleFurstum, Color.green, 4);
        //    //if (!(tooCloseEnemies.Exists(x => Vector3.Angle((player.transform.position - x.transform.position).normalized, dirAngleFurstum) > angleOffset * 0.5f)))
        //    //{//IsWall(player, startAngle) ||

        //    //}
        //}

        //float maxSpace = 0;
        //List<Enemy> bestQuart = new List<Enemy>();

        Queue<Vector3> road = new Queue<Vector3>();
        Vector3 avgPos = Vector3.zero;
        Vector3 mapCenter = new Vector3(10, 0, 10);
        float rad = 10;
		foreach (var en in player.Enemies)
		{
			avgPos += en.transform.position;
		}
		avgPos /= player.Enemies.Count;
		avgPos *= -1;
        avgPos[1] = 0;
		if ((avgPos- mapCenter).sqrMagnitude< rad)
		{
            avgPos += new Vector3(1, 0, 1) * 5;
		}
		Debug.Log("Break Act");
        //Debug.Log(avgPos);
        avgPos = new Vector3(Mathf.Clamp(avgPos.x, 0, 19.5f), 0, Mathf.Clamp(avgPos.z, 0, 19.5f));
        road.Enqueue(avgPos);
        return road;
    }
}
