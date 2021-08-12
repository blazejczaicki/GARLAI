using System.Collections;
using System.Collections.Generic;
using TopShooter;
using UnityEngine;

public class MakeGoCenterAction : DT_Action
{
    private string actName = "GoCenter";

    public override Queue<Vector3> MakeAction(PlayerAI player, ref string actName)
    {
        //Debug.Log("GoCenter Act");
        Queue<Vector3> road = new Queue<Vector3>();
        road.Enqueue(player.MapData.GetMapCenter());
        player.DecisionUpdateTime = player.DataAI.Find(x => x.nameVal == VariableName.WallRetreatTimeUpdate).currentVal;
        actName = this.actName;
        return road;
    }
}
