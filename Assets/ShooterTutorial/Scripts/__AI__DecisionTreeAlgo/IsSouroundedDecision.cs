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
            Debug.Log("IsSourounded");
            return TrueNode.MakeDecision(player);
        }
        else
        {
            Debug.Log("NOT IsSourounded");
            return FalseNode.MakeDecision(player);
        }
    }

    private bool IsSourounded(PlayerAI player)
    {
        float angleOffset = 90;
        float startAngle = 0;
        float minSouroundingDistance = player.DataAI.Find(x => x.nameVal == DecisionName.IsSourounded).currentVal;
        var tooCloseEnemies = player.Enemies.FindAll(x => minSouroundingDistance > Vector3.Distance(player.transform.position, x.transform.position));
        for (int i = 0; i < 4; i++)
        {
            startAngle += angleOffset;
            float minAngle = startAngle - angleOffset*0.5f;
            float maxAngle = startAngle + angleOffset * 0.5f;
            var dirAngleFurstum = Utility.DirectionFromAngle(startAngle);

            Debug.DrawRay(player.transform.position, dirAngleFurstum, Color.green, 4);
            if (!( tooCloseEnemies.Exists(x=> Vector3.Angle((player.transform.position-x.transform.position).normalized, dirAngleFurstum)>angleOffset*0.5f)))
            {//IsWall(player, startAngle) ||

                return false;
            }
        }
        return true;
    }
    //private bool IsWall(PlayerAI player, float angle)
    //{

    //}
}
