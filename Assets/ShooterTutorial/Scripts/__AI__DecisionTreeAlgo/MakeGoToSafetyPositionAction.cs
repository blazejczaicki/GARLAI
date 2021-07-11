using System.Collections;
using System.Collections.Generic;
using TopShooter;
using UnityEngine;

public class MakeGoToSafetyPositionAction : DT_Action
{
    public override Queue<Vector3> MakeAction(PlayerAI player)
    {
        Debug.Log("Safety Act");
        Queue<Vector3> road = new Queue<Vector3>();
        road.Enqueue(player.transform.position);

        //4 strefy kwadratowe i po œrodku ko³owa, wybór odpowiedniej strefy, która ma mniej przeciwników
        return road;
    }
}
