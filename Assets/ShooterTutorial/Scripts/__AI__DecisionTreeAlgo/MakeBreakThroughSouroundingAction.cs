using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TopShooter;
using UnityEngine;

public class MakeBreakThroughSouroundingAction : DT_Action
{
    public override Queue<Vector3> MakeAction(PlayerAI player)
    {
        float angleOffset = 45;
        float startAngle = 0;
        float souroundingDistance = player.DataAI.Find(x => x.nameVal == VariableName.SouroundingDist).currentVal;
        float souroundingWallDist = player.DataAI.Find(x => x.nameVal == VariableName.SouroundingWallDist).currentVal;
        var tooCloseEnemies = player.Enemies.FindAll(x => souroundingDistance > Vector3.Distance(player.transform.position, x.transform.position));

        float minAngle = 0;
        float maxAngle = 45;
        List<Directions> directions = new List<Directions>();
        for (int i = 0; i < 8; i++)
        {
            var dirAngleFurstum = Utility.DirectionFromAngle(startAngle);
            bool iswall = Utility.IsWall(player, dirAngleFurstum, souroundingWallDist);
            int enemies = 0;
            if (!iswall)
            {
                foreach (var en in tooCloseEnemies)
                {
                    var dirr = (en.transform.position - player.transform.position).normalized;
                    //Debug.DrawRay(player.transform.position, dirr, Color.magenta, 4);
                    dirr[1] = 0;
                    var enAngle = Vector3.SignedAngle(dirAngleFurstum, dirr, Vector3.up);
                    if(enAngle < maxAngle && enAngle > minAngle)
				    {
                        enemies++;
				    }
                }
            }
			if (!iswall)// jesli sourdist jest za du¿y >10, to bêdzie wszêdzie wykrywa³ walle
			{
                directions.Add(new Directions(dirAngleFurstum,enemies));
			}            
            startAngle += angleOffset;
        }
        //Debug.Log("Break act");
        var direction = directions.Aggregate((d1,d2)=> d1.enemies<d2.enemies?d1:d2);
        
        //Debug.DrawRay(player.transform.position, direction.dir*souroundingDistance, Color.yellow, 4);
        var targetPos =player.transform.position +direction.dir * souroundingDistance;
        targetPos = new Vector3(Mathf.Clamp(targetPos.x, 0 + player.MapData.OriginPoint.x, 19.5f + player.MapData.OriginPoint.x), 0,
            Mathf.Clamp(targetPos.z, 0 + player.MapData.OriginPoint.z, 19.5f + player.MapData.OriginPoint.z));
        Queue<Vector3> road = new Queue<Vector3>();
        road.Enqueue(targetPos);

        player.DecisionUpdateTime = player.DataAI.Find(x => x.nameVal == VariableName.SouroundingTimeUpdate).currentVal;
        return road;
    }

    private struct Directions
	{
        public Vector3 dir;
        public int enemies;

		public Directions(Vector3 dir,int enemies)
		{
			this.dir = dir;
			this.enemies = enemies;
		}
	}
}
