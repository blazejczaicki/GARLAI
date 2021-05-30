using System.Collections;
using System.Collections.Generic;
using TopShooter;
using UnityEngine;

public class IsHeavyHurtedDecision : DT_Decision
{
    public override DT_IGameTreeNode GetBranch(PlayerAI player)
    {
        if (IsHeavyHurted(player))
        {
            return TrueNode.MakeDecision(player);
        }
        else
        {
            return FalseNode.MakeDecision(player);
        }
    }
    
    private bool IsHeavyHurted(PlayerAI player)
    {
        return true;
    }
}
