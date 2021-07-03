using System.Collections;
using System.Collections.Generic;
using TopShooter;
using UnityEngine;

public interface DT_IGameTreeNode 
{
    DT_IGameTreeNode MakeDecision(PlayerAI player);
    List<Vector3> MakeAction(PlayerAI player);
}
