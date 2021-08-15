using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TopShooter;
using UnityEngine;

public class SaverCSV : MonoBehaviour
{
	[SerializeField] private string generationName;
	[SerializeField] private string bestName;
	[SerializeField] private string bestFirstName;
	[SerializeField] private string avgName;
	//D:\Unity projekty\_MgrProj\GARLAI\Rezultaty_Pokoleniowe.csv

	private void Awake()
	{
		generationName = Application.dataPath + "\\Rezultaty_Pokoleniowe.csv";
		bestName = Application.dataPath + "\\Rezultaty_TheBests.csv";
		bestFirstName = Application.dataPath + "\\Rezultaty_TheBestsFirs.csv";
		avgName = Application.dataPath + "\\Rezultaty_Srednie.csv";
	}

	public void WriteToCSVGenerations(DataChromosome bestCrom, int generation)
	{
		generationName= Application.dataPath + "\\RezultatyGeneracji\\Rezultaty_Pokoleniowe" + SceneComunicator.instance.currentIT +".csv";
		if (!File.Exists(generationName))
		{
			CreateFile(bestCrom.chromosome.PlayerAI.DataAI, generationName);
			AppendData(bestCrom, generation, generationName);
		}
		else
		{
			AppendData(bestCrom, generation, generationName);
		}		
	}

	public void WriteToCSVFinal(DataChromosome bestCrom, int generation, bool isFirst=false)
	{
		string nam = isFirst ?bestFirstName:bestName;
		if (!File.Exists(nam))
		{
			CreateFileAvg(bestCrom.chromosome.PlayerAI.DataAI, nam);
			AppendDataFinalFirst(bestCrom, generation, nam);
		}
		else
		{
			AppendDataFinalFirst(bestCrom, generation, nam);
		}
	}

	public void WriteToCSVAvg(DataChromosome avgCrom, List<DataAI> dataAi, int generation)
	{
		if (!File.Exists(avgName))
		{
			CreateFileAvg(dataAi, avgName);
			AppendDataAvg(avgCrom, dataAi, generation, avgName);
		}
		else
		{
			AppendDataAvg(avgCrom, dataAi, generation, avgName);
		}
	}

	private void CreateFile(List<DataAI> data, string filename)
	{
		using (StreamWriter tww = File.CreateText(filename))
		{
			StringBuilder stringBuilderr = new StringBuilder();
			stringBuilderr.Append("Player Name;Generation;");
			foreach (var item in data)
			{
				stringBuilderr.Append(item.nameVal.ToString() + ";");
			}
			stringBuilderr.Append("Life Time;Average Health;Fitness;IsDead");
			tww.WriteLine(stringBuilderr.ToString());
			stringBuilderr.Clear();
		}
	}

	private void AppendData(DataChromosome dataChromosome, int generation, string filename)
	{
		using (StreamWriter tw = File.AppendText(filename))
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(dataChromosome.name + ";" + generation+";");
			foreach (var item in dataChromosome.chromosome.PlayerAI.DataAI)
			{
				stringBuilder.Append(item.currentVal+ ";");
			}//.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)
			stringBuilder.Append(dataChromosome.lifeTime + ";"
				+ dataChromosome.averageHealth + ";"
				+ dataChromosome.fitness + ";"+dataChromosome.chromosome.PlayerAI.PlayerShooter.Dead.ToString());
			tw.WriteLine(stringBuilder.ToString());
		}
	}

	private void AppendDataFinalFirst(DataChromosome dataChromosome, int generation, string filename)
	{
		using (StreamWriter tw = File.AppendText(filename))
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(dataChromosome.name + ";" + SceneComunicator.instance.currentIT + ";" + generation+";");
			foreach (var item in dataChromosome.chromosome.PlayerAI.DataAI)
			{
				stringBuilder.Append(item.currentVal+ ";");
			}//.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)
			stringBuilder.Append(dataChromosome.lifeTime + ";"
				+ dataChromosome.averageHealth + ";"
				+ dataChromosome.fitness + ";"+dataChromosome.chromosome.PlayerAI.PlayerShooter.Dead.ToString());
			tw.WriteLine(stringBuilder.ToString());
		}
	}

	private void CreateFileAvg(List<DataAI> data, string filename)
	{
		using (StreamWriter tww = File.CreateText(filename))
		{
			StringBuilder stringBuilderr = new StringBuilder();
			stringBuilderr.Append("Player Name;Iteration;Generation;");
			foreach (var item in data)
			{
				stringBuilderr.Append(item.nameVal.ToString() + ";");
			}
			stringBuilderr.Append("Life Time;Average Health;Fitness;IsDead");
			tww.WriteLine(stringBuilderr.ToString());
			stringBuilderr.Clear();
		}
	}
	
	private void AppendDataAvg(DataChromosome dataChromosome, List<DataAI> dataAIs, int generation, string filename)
	{
		using (StreamWriter tw = File.AppendText(filename))
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(dataChromosome.name + ";" + SceneComunicator.instance.currentIT +";" + generation +";");
			foreach (var item in dataAIs)
			{
				stringBuilder.Append(item.currentVal+ ";");
			}//.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)
			stringBuilder.Append(dataChromosome.lifeTime + ";"
				+ dataChromosome.averageHealth + ";"
				+ dataChromosome.fitness);
			tw.WriteLine(stringBuilder.ToString());
		}
	}
}
