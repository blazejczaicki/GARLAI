using System.Collections;
using System.Collections.Generic;
using TopShooter;
using System.Linq;
using UnityEngine;

public class MakeGoBackAction : DT_Action
{
    public override Vector3 MakeAction(PlayerAI player)
    {
        var count = player.Enemies.Count;
        List<Enemy> enemiesTooClose = new List<Enemy>();
        for (int i = 0; i < count; i++)
        {
            if (player.MinEnemyDistance > Vector3.Distance(player.transform.position, player.Enemies[i].transform.position))
            {
                enemiesTooClose.Add(player.Enemies[i]);
            }
        }
        Vector3 averageDirection = Vector3.zero;
        foreach (var etc in enemiesTooClose)
        {
            averageDirection = (etc.transform.position - player.transform.position).normalized;
        }
        averageDirection += averageDirection / (float)enemiesTooClose.Count;
        Debug.Log("GoBack Act");
        return averageDirection*player.MinimalRetreatDistance;
    }
}
