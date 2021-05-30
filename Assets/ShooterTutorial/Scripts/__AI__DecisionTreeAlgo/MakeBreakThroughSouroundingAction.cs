using System.Collections;
using System.Collections.Generic;
using TopShooter;
using UnityEngine;

public class MakeBreakThroughSouroundingAction : DT_Action
{
    public override Vector3 MakeAction(PlayerAI player)
    {
        Debug.Log("Break Act");
        return Vector3.zero;
    }
}
