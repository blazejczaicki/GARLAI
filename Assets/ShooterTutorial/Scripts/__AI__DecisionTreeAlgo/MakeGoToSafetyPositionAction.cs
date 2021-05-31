using System.Collections;
using System.Collections.Generic;
using TopShooter;
using UnityEngine;

public class MakeGoToSafetyPositionAction : DT_Action
{
    public override Vector3 MakeAction(PlayerAI player)
    {
        Debug.Log("Safety Act");
        return player.transform.position;
    }
}
