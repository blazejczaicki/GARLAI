using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TopShooter;
using UnityEngine;

public class MakeGoToSafetyPositionAction : DT_Action
{
    public override Queue<Vector3> MakeAction(PlayerAI player)
    {
        Vector3 bestPos = Vector3.zero;
		for (int i = 0; i < 4; i++)
		{
            var randomNode = player.MapData.GetRandomNode();
			if (randomNode.enemyInfluence <= player.MapData.GetRandomNode().enemyInfluence)
			{
                bestPos = new Vector3(randomNode.position2d.x, 0, randomNode.position2d.y);
            }
		}

        Debug.Log("Not Safety Act");
        Queue<Vector3> road = new Queue<Vector3>();
        road.Enqueue(bestPos);
        player.DecisionUpdateTime = player.DataAI.Find(x => x.nameVal == VariableName.SafetyTimeUpdate).currentVal;
        //4 strefy kwadratowe i po œrodku ko³owa, wybór odpowiedniej strefy, która ma mniej przeciwników
        return road;
    }
}
