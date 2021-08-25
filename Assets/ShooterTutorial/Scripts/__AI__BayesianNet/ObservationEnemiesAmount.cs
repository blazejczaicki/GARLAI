using System.Collections;
using System.Collections.Generic;
using TopShooter;
using UnityEngine;

public class ObservationEnemiesAmount
{
	private string underWhelm = "Underwhelm";
	private string overWhelm = "Overwhelm";
	private string none = "None";

	public string GetEnemyAmount(PlayerAI player, float v)
	{
		//var enemyAmount = player.Enemies.Count;
		var enemyAmount = v;
		string result;
		if (enemyAmount == 0) result = none;
		else if (enemyAmount <= 3) result = underWhelm;
		else result = overWhelm;
		return result;
	}
}
