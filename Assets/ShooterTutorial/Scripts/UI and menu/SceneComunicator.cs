using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneComunicator : MonoBehaviour
{
	public static SceneComunicator instance;
	public SceneTaker sceneTaker;
	public bool isChanged;
	public int iterations = 15;
	public int currentIT = 0;

	public int generationLimits = 0;
	public float playerSpeed = 0;
	public float enemySpeed = 0;
	public float attackDistanceThreshold = 0;
	public float time = 0;

	public int manualIndex = 0;

	public bool randomMode = false;
	public bool manual = false;
	public float seed = 20;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}
}

public enum SceneTaker
{
	None,
	DT,
	BN,
	ML
}