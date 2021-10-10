using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using TopShooter;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class PlayerMLA : Agent
{
    private PlayerEntity playerEntity;
    private CharacterController characterController;

    [SerializeField] private float radius = 0.1f;
    [SerializeField] private float speed = 10f;

    [SerializeField] private MapData mapData;

    public MapData MapData { get => mapData; set => mapData = value; }
    public CharacterController CharController { get => characterController; set => characterController = value; }
    public PlayerEntity PlayerEnt { get => playerEntity; set => playerEntity = value; }
    public float Speed { get => speed; set => speed = value; }

    private void Awake()
    {
        CharController = GetComponent<CharacterController>();
        PlayerEnt = GetComponent<PlayerEntity>();

        PlayerEnt.OnDeath += OnNewGeneration;
    }

    public float GetAverageHealth()
    {
        return PlayerEnt.HealthOnSeconds;
    }

    public float GetRestHealth()
    {
        return PlayerEnt.health;
    }

    public float GetLifeTime()
    {
        return PlayerEnt.LifeTime;
    }

    public void OnEndGeneration()
    {
        PlayerEnt.OnEndGeneration();
    }

    public void ResetPlayerWorld()
    {
        this.StopAllCoroutines();
    }

    public void OnUpdate(float t)
    {
        PlayerEnt.OnUpdate(t);
    }


	public override void OnEpisodeBegin()
	{
		base.OnEpisodeBegin();
        mapData.ResetMapWorld();
        PlayerEnt.OnNewGeneration(GameManagerMLA.instance.tt);
        OnEndGeneration();
        //gameObject.SetActive(true);
    }

	public override void OnActionReceived(ActionBuffers actions)
	{
		base.OnActionReceived(actions);
        float x = actions.ContinuousActions[0];
        float z = actions.ContinuousActions[1];

        AddReward(0.1f);

        var moveVec = new Vector3(x,0,z) * Speed;
        CharController.SimpleMove(moveVec);
    }   

	public override void CollectObservations(VectorSensor sensor)
	{
		base.CollectObservations(sensor);
        sensor.AddObservation(transform.position);

        for (int i = 0; i < 10; i++)
		{
		    if (mapData.EnemiesMLA[i]!=null)
		    {
                sensor.AddObservation(mapData.EnemiesMLA[i].transform.position);
            }
		    else
		    {
                sensor.AddObservation(Vector3.zero);
		    }
		}

       // Debug.Log("Collect call");
	}

    public void RewardPlayer()
	{
        SetReward(-1);
	}

	public void OnNewGeneration()
    {
        EndEpisode();
    }
}
