using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TopShooter;
using UnityEngine;

public class MakeGoToSafetyPositionAction : DT_Action
{
    private string actName = "GoSafety";
    public override Queue<Vector3> MakeAction(PlayerAI player, ref string actName)
    {
        Vector3 bestPos = Vector3.zero;
        float enemyInfluence = float.MaxValue;
		for (int i = 0; i < 4; i++)
		{
            var randomNode = player.MapData.GetRandomNode();
			if (randomNode.enemyInfluence <=enemyInfluence)
			{
                bestPos = new Vector3(randomNode.position2d.x, 0, randomNode.position2d.y);
                enemyInfluence = randomNode.enemyInfluence;
            }
		}
		if (bestPos==Vector3.zero)
		{
                Debug.Log("Not Safety Act");
        }
        actName = this.actName;
        //Debug.Log("Not Safety Act");
        Queue<Vector3> road = new Queue<Vector3>();
        road.Enqueue(bestPos);
        player.DecisionUpdateTime = player.DataAI.Find(x => x.nameVal == VariableName.SafetyTimeUpdate).currentVal;
        //4 strefy kwadratowe i po œrodku ko³owa, wybór odpowiedniej strefy, która ma mniej przeciwników
        return road;
    }
}
