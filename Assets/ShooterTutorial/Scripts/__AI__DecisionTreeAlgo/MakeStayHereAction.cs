using System.Collections;
using System.Collections.Generic;
using TopShooter;
using UnityEngine;

public class MakeStayHereAction : DT_Action
{
    public override Queue<Vector3> MakeAction(PlayerAI player)
    {
        Debug.Log("Safety Act");
        Queue<Vector3> road = new Queue<Vector3>();
        road.Enqueue(player.transform.position);
        player.DecisionUpdateTime= player.DataAI.Find(x => x.nameVal == VariableName.StayTimeUpdate).currentVal;
        return road;
    }
}
