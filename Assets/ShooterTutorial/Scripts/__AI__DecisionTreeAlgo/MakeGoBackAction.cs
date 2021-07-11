using System.Collections;
using System.Collections.Generic;
using TopShooter;
using System.Linq;
using UnityEngine;

public class MakeGoBackAction : DT_Action
{
    public override Queue<Vector3> MakeAction(PlayerAI player)
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
            averageDirection = (averageDirection +(etc.transform.position - player.transform.position).normalized).normalized;
        }
        Debug.Log("GoBack Act");//averageDirection*player.MinimalRetreatDistance

        float escapeDistance = 8;
        Vector3 escapePosition = player.transform.position + averageDirection * escapeDistance;
        escapePosition = new Vector3(Mathf.Clamp(escapePosition.x,0,19.5f),0, Mathf.Clamp(escapePosition.z, 0, 19.5f));
        Queue<Vector3> road = new Queue<Vector3>();
        road.Enqueue(escapePosition);
        return road;
    }
}
