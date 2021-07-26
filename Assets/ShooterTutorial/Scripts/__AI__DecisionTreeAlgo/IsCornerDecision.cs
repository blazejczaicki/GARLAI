using System.Collections;
using System.Collections.Generic;
using TopShooter;
using UnityEngine;

public class IsCornerDecision : DT_Decision
{
    public override DT_IGameTreeNode GetBranch(PlayerAI player)
    {
        if (IsWall(player))
        {
            return TrueNode.MakeDecision(player);
        }
        else
        {
            return FalseNode.MakeDecision(player);
        }
    }

    private bool IsWall(PlayerAI player)
    {
        Vector3 averageDirection = player.GetAverageDirectionByEnemies();
        Vector3 escapePosition = player.transform.position + averageDirection * player.escapeDistance;

        return escapePosition.x < player.MapData.OriginPoint.x || escapePosition.x > player.MapData.OriginPoint.x + 19.5f ||
            escapePosition.z < player.MapData.OriginPoint.z || escapePosition.z > player.MapData.OriginPoint.z + 19.5f;
    }
}
