using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TopShooter;
using UnityEngine;

public class SaverCSV : MonoBehaviour
{
	[SerializeField] private string filename;
	//D:\Unity projekty\_MgrProj\GARLAI\Rezultaty_Pokoleniowe.csv

	private void Awake()
	{
		filename = Application.dataPath + "\\Rezultaty_Pokoleniowe.csv";
	}

	public void WriteToCSV(GA_GeneticAlgorithm ga, PlayerAI playerAi)
	{
		if (!File.Exists(filename))
		{
			CreateFile(playerAi);
			AppendData(ga, playerAi);
		}
		else
		{
			AppendData(ga, playerAi);
		}		
	}

	private void CreateFile(PlayerAI playerAi)
	{
		using (StreamWriter tww = File.CreateText(filename))
		{
			StringBuilder stringBuilderr = new StringBuilder();
			stringBuilderr.Append("Player Name,");
			foreach (var item in playerAi.DataAI)
			{
				stringBuilderr.Append(item.nameVal.ToString() + ",");
			}
			stringBuilderr.Append("Life Time,Health Points,Fitness");
			tww.WriteLine(stringBuilderr.ToString());
			stringBuilderr.Clear();
		}
	}

	private void AppendData(GA_GeneticAlgorithm ga, PlayerAI playerAi)
	{
		using (StreamWriter tw = File.AppendText(filename))
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(playerAi.gameObject.name + ",");
			foreach (var item in playerAi.DataAI)
			{
				stringBuilder.Append(item.currentVal.ToString("0.0000", System.Globalization.CultureInfo.InvariantCulture) + ",");
			}//.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)
			stringBuilder.Append(playerAi.GetLifeTime().ToString("0.0000", System.Globalization.CultureInfo.InvariantCulture) + ","
				+ playerAi.GetAverageHealth().ToString("0.0000", System.Globalization.CultureInfo.InvariantCulture) + ","
				+ ga.BestResult.Fitness.ToString("0.0000", System.Globalization.CultureInfo.InvariantCulture));
			tw.WriteLine(stringBuilder.ToString());
		}
	}
}
