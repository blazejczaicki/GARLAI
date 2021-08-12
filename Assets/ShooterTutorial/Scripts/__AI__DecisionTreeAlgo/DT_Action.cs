using System.Collections;
using System.Collections.Generic;
using TopShooter;
using UnityEngine;

public abstract class DT_Action : DT_IGameTreeNode
{
    public abstract Queue<Vector3> MakeAction(PlayerAI player, ref string actName);
    public DT_IGameTreeNode MakeDecision(PlayerAI player)
    {
        return this;
    }
}
