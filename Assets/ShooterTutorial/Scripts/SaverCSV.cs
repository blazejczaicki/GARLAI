using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaverCSV : MonoBehaviour
{
	[SerializeField] private string filename = "";

    public void WriteToCSV(GA_GeneticAlgorithm ga)
	{
		TextWriter tw = new StreamWriter(filename, false);
		tw.WriteLine("");
		tw.Close();
		tw = new StreamWriter(filename, true);

		tw.Close();
	}
}
