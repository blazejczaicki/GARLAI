using System.Collections;
using System.Collections.Generic;
using TopShooter;
using UnityEngine;

public class IsSouroundedDecision : DT_Decision
{
    public override DT_IGameTreeNode GetBranch(PlayerAI player)
    {
        if (IsSourounded(player))
        {
            //Debug.Log("IsSourounded");
            return TrueNode.MakeDecision(player);
        }
        else
        {
           // Debug.Log("NOT IsSourounded");
            return FalseNode.MakeDecision(player);
        }
    }

    private bool IsSourounded(PlayerAI player)
    {
        float angleOffset = 90;
        float startAngle = 0;
        float minSouroundingDistance = player.DataAI.Find(x => x.nameVal == VariableName.MinSouroundingDist).currentVal;
        var tooCloseEnemies = player.Enemies.FindAll(x => minSouroundingDistance > Vector3.Distance(player.transform.position, x.transform.position));
        float minAngle = 0;
        float maxAngle = 90;
        List<bool> sides = new List<bool>();
        for (int i = 0; i < 4; i++)
        {            
            var dirAngleFurstum = Utility.DirectionFromAngle(startAngle);
            bool iswall = Utility.IsWall(player, dirAngleFurstum, minSouroundingDistance);
            bool isenemie = false;
			foreach (var en in tooCloseEnemies)
			{
                var dirr = (en.transform.position - player.transform.position).normalized;
               // Debug.DrawRay(player.transform.position, dirr, Color.magenta, 4);
                dirr[1] = 0;
                var enAngle = Vector3.SignedAngle(dirAngleFurstum, dirr, Vector3.up);
                isenemie = enAngle < maxAngle && enAngle>minAngle;
            }

            if (isenemie || iswall)
            {
                //Debug.DrawRay(player.transform.position, dirAngleFurstum, Color.green, 4);
                sides.Add(true);
            }
            startAngle += angleOffset;
        }
        return sides.Count>=3;
    }
}
