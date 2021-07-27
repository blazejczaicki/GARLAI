using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TopShooter;
using UnityEngine;

public class MakeBreakThroughSouroundingAction : DT_Action
{
    public override Queue<Vector3> MakeAction(PlayerAI player)
    {
        float angleOffset = 90;
        float startAngle = 0;
        float minSouroundingDistance = player.DataAI.Find(x => x.nameVal == DecisionName.IsSourounded).currentVal;
        var tooCloseEnemies = player.Enemies.FindAll(x => minSouroundingDistance > Vector3.Distance(player.transform.position, x.transform.position));
        float dist = 8;

        float minAngle = 0;
        float maxAngle = 45;
        List<Directions> directions = new List<Directions>();
        for (int i = 0; i < 8; i++)
        {
            var dirAngleFurstum = Utility.DirectionFromAngle(startAngle);
            bool iswall = Utility.IsWall(player, dirAngleFurstum, dist);
            int enemies = 0;
            if (!iswall)
            {
                foreach (var en in tooCloseEnemies)
                {
                    var dirr = (en.transform.position - player.transform.position).normalized;
                    Debug.DrawRay(player.transform.position, dirr, Color.magenta, 4);
                    dirr[1] = 0;
                    var enAngle = Vector3.SignedAngle(dirAngleFurstum, dirr, Vector3.up);
                    if(enAngle < maxAngle && enAngle > minAngle)
				    {
                        enemies++;
				    }
                }
            }
            directions.Add(new Directions(dirAngleFurstum,iswall,enemies));
            startAngle += angleOffset;
        }

        var direction = directions.Aggregate((d1,d2)=> d1.enemies<d2.enemies?d1:d2);
        Queue<Vector3> road = new Queue<Vector3>();
        road.Enqueue(direction.dir*dist+player.transform.position);
  //      Vector3 avgPos = Vector3.zero;
  //      Vector3 mapCenter = player.MapData.GetMapCenter();// new Vector3(10, 0, 10);
  //      float rad = 10;
		//foreach (var en in player.Enemies)
		//{
		//	avgPos += en.transform.position;
		//}
		//avgPos /= player.Enemies.Count;
		//avgPos *= -1;
  //      avgPos[1] = 0;
		//if ((avgPos- mapCenter).sqrMagnitude< rad)
		//{
  //          avgPos += new Vector3(1, 0, 1) * 5;
		//}
		//Debug.Log("Break Act");
  //      //Debug.Log(avgPos);
  //      //avgPos = new Vector3(Mathf.Clamp(avgPos.x, 0, 19.5f), 0, Mathf.Clamp(avgPos.z, 0, 19.5f));
  //      avgPos = new Vector3(Mathf.Clamp(avgPos.x, 0 + player.MapData.OriginPoint.x, 19.5f + player.MapData.OriginPoint.x), 0, 
  //          Mathf.Clamp(avgPos.z, 0 + player.MapData.OriginPoint.z, 19.5f + player.MapData.OriginPoint.z));
  //      road.Enqueue(avgPos);
        return road;
    }

    private struct Directions
	{
        public Vector3 dir;
        public bool wall;
        public int enemies;

		public Directions(Vector3 dir, bool wall, int enemies)
		{
			this.dir = dir;
			this.wall = wall;
			this.enemies = enemies;
		}
	}
}
