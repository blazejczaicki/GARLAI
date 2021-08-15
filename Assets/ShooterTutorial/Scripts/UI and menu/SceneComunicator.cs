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