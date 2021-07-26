using System.Collections;
using System.Collections.Generic;
using TopShooter;
using UnityEngine;

public class IsSafetyAroundDecision : DT_Decision
{
    public override DT_IGameTreeNode GetBranch(PlayerAI player)
    {
        if (IsSafetyAround(player))
        {
            Debug.Log("IsSafetyAround");
            return TrueNode.MakeDecision(player);
        }
        else
        {
            Debug.Log("NOT isSafetyAround");
            return FalseNode.MakeDecision(player);
        }
    }

    public bool IsSafetyAround(PlayerAI player)
    {
        float minEnemyDistance = player.DataAI.Find(x => x.nameVal == DecisionName.IsEnemiesTooClose).currentVal;
        float minEnemyDistanceSafety = player.DataAI.Find(x => x.nameVal == DecisionName.IsSafetyAround).currentVal;
        float min = minEnemyDistance + minEnemyDistanceSafety;
        return player.Enemies.Exists(x => min < Vector3.Distance(player.transform.position, x.transform.position)); 
    }
}
