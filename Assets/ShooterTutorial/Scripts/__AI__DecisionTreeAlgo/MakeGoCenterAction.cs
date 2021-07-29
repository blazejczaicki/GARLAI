using System.Collections;
using System.Collections.Generic;
using TopShooter;
using UnityEngine;

public class MakeGoCenterAction : DT_Action
{
    public override Queue<Vector3> MakeAction(PlayerAI player)
    {
        Debug.Log("GoCenter Act");
        Queue<Vector3> road = new Queue<Vector3>();
        road.Enqueue(player.MapData.GetMapCenter());
        player.DecisionUpdateTime = player.DataAI.Find(x => x.nameVal == VariableName.WallRetreatTimeUpdate).currentVal;
        return road;
    }
}
