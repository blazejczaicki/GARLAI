using System.Collections;
using System.Collections.Generic;
using TopShooter;
using UnityEngine;

public class MakeGoBackWallAction : DT_Action
{
    private string actName = "GoWall";
    public override Queue<Vector3> MakeAction(PlayerAI player, ref string actName)
    {
        Vector3 averageDirection = player.GetAverageDirectionByEnemies();
        float escapeDist = player.DataAI.Find(x => x.nameVal == VariableName.WallBackEscapeDist).currentVal;
        //Debug.Log("GoBackWall Act");
        //player.gobackDir = averageDirection;
        Vector3 escapePosition = player.transform.position + averageDirection * escapeDist;
        escapePosition = new Vector3(Mathf.Clamp(escapePosition.x, 0 + player.MapData.OriginPoint.x, 19.5f + player.MapData.OriginPoint.x), 0,
            Mathf.Clamp(escapePosition.z, 0 + player.MapData.OriginPoint.z, 19.5f + player.MapData.OriginPoint.z));
        actName = this.actName;
        Queue<Vector3> road = new Queue<Vector3>();
        road.Enqueue(escapePosition);
        player.DecisionUpdateTime = player.DataAI.Find(x => x.nameVal == VariableName.WallRetreatTimeUpdate).currentVal;
        return road;
    }
}
