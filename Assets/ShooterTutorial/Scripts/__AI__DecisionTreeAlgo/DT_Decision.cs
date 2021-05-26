using System.Collections;
using System.Collections.Generic;
using TopShooter;
using UnityEngine;

public abstract class DT_Decision : DT_IGameTreeNode
{
    DT_IGameTreeNode trueNode;
    DT_IGameTreeNode falseNode;

    public abstract void GetBranch(PlayerAiKnowledgeData playerAiKnowledgeData);//tu warunek testu jest sprawdzany


    public void InitDecisionValues()
    {
        throw new System.NotImplementedException();
    }

    public abstract DT_IGameTreeNode MakeDecision(PlayerAI player);
    public Vector3 MakeAction(PlayerAI player)
    {
        throw new System.NotImplementedException();
    }
}
