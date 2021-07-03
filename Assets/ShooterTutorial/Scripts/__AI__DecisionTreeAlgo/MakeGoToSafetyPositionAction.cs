using System.Collections;
using System.Collections.Generic;
using TopShooter;
using UnityEngine;

public class MakeGoToSafetyPositionAction : DT_Action
{
    public override List<Vector3> MakeAction(PlayerAI player)
    {
        Debug.Log("Safety Act");
        List<Vector3> road = new List<Vector3>() { player.transform.position };

        //4 strefy kwadratowe i po �rodku ko�owa, wyb�r odpowiedniej strefy, kt�ra ma mniej przeciwnik�w
        return road;
    }
}
