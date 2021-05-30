using System.Collections;
using System.Collections.Generic;
using TopShooter;
using UnityEngine;

public class MakeGoBackAction : DT_Action
{
    public override Vector3 MakeAction(PlayerAI player)
    {
        //var count = player.Enemies.Count;
        //List<Vector3> enemiesPositions = new List<Vector3>();
        //for (int i = 0; i < count; i++)
        //{
        //    if (player.MinEnemyDistance > Vector3.Distance(player.transform.position, player.Enemies[i].transform.position))
        //    {
        //        enemiesPositions.Add(player.Enemies[i].transform.position);
        //    }
        //}
        Debug.Log("GoBack Act");
        return Vector3.zero;
    }
}
