using System.Collections;
using System.Collections.Generic;
using TopShooter;
using UnityEngine;

public abstract class DT_Action : DT_IGameTreeNode
{
    public abstract Vector3 MakeAction(PlayerAI player);
    public abstract DT_IGameTreeNode MakeDecision(PlayerAI player);
}
