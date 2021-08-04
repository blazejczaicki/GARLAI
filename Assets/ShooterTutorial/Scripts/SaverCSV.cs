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
	//D:\Unity projekty\_MgrProj\GARLAI\Rezultaty_Pokoleniowe.csv

	private void Awake()
	{
		generationName = Application.dataPath + "\\Rezultaty_Pokoleniowe.csv";
		bestName = Application.dataPath + "\\Rezultaty_TheBests.csv";
	}

	public void WriteToCSVGenerations(DataChromosome bestCrom, int generation)
	{
		if (!File.Exists(generationName))
		{
			CreateFile(bestCrom.chromosome.Genes, generationName);
			AppendData(bestCrom, generation, generationName);
		}
		else
		{
			AppendData(bestCrom, generation, generationName);
		}		
	}

	public void WriteToCSVFinal(DataChromosome bestCrom, int generation)
	{
		if (!File.Exists(bestName))
		{
			CreateFile(bestCrom.chromosome.Genes, bestName);
			AppendData(bestCrom, generation, bestName);
		}
		else
		{
			AppendData(bestCrom, generation, bestName);
		}
	}

	private void CreateFile(List<DataAI> data, string filename)
	{
		using (StreamWriter tww = File.CreateText(filename))
		{
			StringBuilder stringBuilderr = new StringBuilder();
			stringBuilderr.Append("Player Name,Generation,");
			foreach (var item in data)
			{
				stringBuilderr.Append(item.nameVal.ToString() + ",");
			}
			stringBuilderr.Append("Life Time,Average Health,Fitness");
			tww.WriteLine(stringBuilderr.ToString());
			stringBuilderr.Clear();
		}
	}

	private void AppendData(DataChromosome dataChromosome, int generation, string filename)
	{
		using (StreamWriter tw = File.AppendText(filename))
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(dataChromosome.name + "," + generation+",");
			foreach (var item in dataChromosome.chromosome.Genes)
			{
				stringBuilder.Append(item.currentVal.ToString("0.0000", System.Globalization.CultureInfo.InvariantCulture) + ",");
			}//.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)
			stringBuilder.Append(dataChromosome.lifeTime.ToString("0.0000", System.Globalization.CultureInfo.InvariantCulture) + ","
				+ dataChromosome.averageHealth.ToString("0.0000", System.Globalization.CultureInfo.InvariantCulture) + ","
				+ dataChromosome.fitness.ToString("0.0000", System.Globalization.CultureInfo.InvariantCulture));
			tw.WriteLine(stringBuilder.ToString());
		}
	}
}
