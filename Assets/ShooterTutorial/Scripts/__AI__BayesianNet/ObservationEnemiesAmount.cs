using System.Collections;
using System.Collections.Generic;
using TopShooter;
using UnityEngine;

public class ObservationEnemiesAmount
{
	private string underWhelm = "Underwhelm";
	private string overWhelm = "Overwhelm";

	public string GetEnemyAmount(PlayerAI player)
	{
		var enemyAmount = player.Enemies.Count;

		string result;
		if (enemyAmount <= 3) result = underWhelm;
		else result = overWhelm;
		return result;
	}
}
