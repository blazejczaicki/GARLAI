using System.Collections;
using System.Collections.Generic;
using TopShooter;
using UnityEngine;

public class IsWallOnRoadDecision : DT_Decision
{
    public override DT_IGameTreeNode GetBranch(PlayerAI player)
    {
        if (IsTooCloseWall(player))
        {
            return TrueNode.MakeDecision(player);
        }
        else
        {
            return FalseNode.MakeDecision(player);
        }
    }

    private bool IsTooCloseWall(PlayerAI player)
    {
        float minEnemyDistance = player.DataAI.Find(x => x.nameVal == VariableName.EnemiesTooCloseDist).currentVal;
        float escapeDistance = player.DataAI.Find(x => x.nameVal == VariableName.WallBackEscapeDist).currentVal;
        player.UpdateCloseEnemies(minEnemyDistance);
        Vector3 averageDirection = player.GetAverageDirectionByEnemies();
        Vector3 escapePosition = player.transform.position - averageDirection * escapeDistance;

        return escapePosition.x<player.MapData.OriginPoint.x || escapePosition.x > player.MapData.OriginPoint.x + 19.5f ||
            escapePosition.z < player.MapData.OriginPoint.z || escapePosition.z > player.MapData.OriginPoint.z + 19.5f;
    }
}
