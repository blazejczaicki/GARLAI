using System.Collections;
using System.Collections.Generic;
using TopShooter;
using UnityEngine;

public class IsTooCloseBorderDecision : DT_Decision
{
    public override DT_IGameTreeNode GetBranch(PlayerAI player)
    {
        if (IsTooCloseBorder(player))
        {
            return TrueNode.MakeDecision(player);
        }
        else
        {
            return FalseNode.MakeDecision(player);
        }
    }

    private bool IsTooCloseBorder(PlayerAI player)
    {
        return true;
    }
}
