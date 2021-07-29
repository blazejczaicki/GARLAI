using System.Collections;
using System.Collections.Generic;
using TopShooter;
using UnityEngine;

public class IsCornerDecision : DT_Decision
{
    public override DT_IGameTreeNode GetBranch(PlayerAI player)
    {
        float escapeDist = player.DataAI.Find(x => x.nameVal == VariableName.WallBackEscapeDist).currentVal;
        if (Utility.IsWall(player, player.GetAverageDirectionByEnemies(), escapeDist))
        {
            return TrueNode.MakeDecision(player);
        }
        else
        {
            return FalseNode.MakeDecision(player);
        }
    }
}
