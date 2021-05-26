using System.Collections;
using System.Collections.Generic;
using TopShooter;
using UnityEngine;

public class DecisionTree : MonoBehaviour
{
    private DT_IGameTreeNode root;

    public void CreateTmpTree()
    {

    }

    public Vector3 MakeDecision(PlayerAI player)
    {
        DT_IGameTreeNode actionNode = root.MakeDecision(player);
        return actionNode.MakeAction(player);
    }
}
