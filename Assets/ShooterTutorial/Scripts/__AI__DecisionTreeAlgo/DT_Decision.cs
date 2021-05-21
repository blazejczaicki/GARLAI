using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DT_Decision : MonoBehaviour, DT_GameTreeNode
{
    DT_GameTreeNode trueNode;
    DT_GameTreeNode falseNode;

    public abstract void getBranch(PlayerAiKnowledgeData playerAiKnowledgeData);//tu warunek testu jest sprawdzany

    public void MakeDecision()
    {
        throw new System.NotImplementedException();
    }
}
