using System.Collections;
using System.Collections.Generic;
using TopShooter;
using UnityEngine;

public abstract class DT_Decision : DT_IGameTreeNode
{
    protected DT_IGameTreeNode trueNode;
    protected DT_IGameTreeNode falseNode;

    public DT_IGameTreeNode TrueNode { get => trueNode; set => trueNode = value; }
    public DT_IGameTreeNode FalseNode { get => falseNode; set => falseNode = value; }

    public abstract DT_IGameTreeNode GetBranch(PlayerAI player);//tu warunek testu jest sprawdzany


    public void InitDecisionValues()
    {
        throw new System.NotImplementedException();
    }

    public DT_IGameTreeNode MakeDecision(PlayerAI player)
    {
        return GetBranch(player);
    }

    public Vector3 MakeAction(PlayerAI player)
    {
        throw new System.NotImplementedException();
    }
}
