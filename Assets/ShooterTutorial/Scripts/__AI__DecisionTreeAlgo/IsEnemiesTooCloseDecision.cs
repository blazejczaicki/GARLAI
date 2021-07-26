using System.Collections;
using System.Collections.Generic;
using TopShooter;
using UnityEngine;

public class IsEnemiesTooCloseDecision : DT_Decision
{
    public override DT_IGameTreeNode GetBranch(PlayerAI player)
    {
        if (IsEnemiesTooClose(player))
        {
            Debug.Log("IsClose");
            return TrueNode.MakeDecision(player);
        }
        else
        {
            Debug.Log("NOT IsClose");
            return FalseNode.MakeDecision(player);
        }
    }

    private bool IsEnemiesTooClose(PlayerAI player)
    {
        float minEnemyDistance = player.DataAI.Find(x => x.nameVal == DecisionName.IsEnemiesTooClose).currentVal;
        return player.Enemies.Exists(x=> minEnemyDistance > Vector3.Distance(player.transform.position, x.transform.position));
    }
}
