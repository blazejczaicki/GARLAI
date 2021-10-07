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
        MoveCR();
    }

    private void MoveCR()
    {
        //var moveVec = (currentTarget - transform.position).normalized * Speed;// * Time.deltaTime;
        //CharController.SimpleMove(new Vector3(moveVec.x, 0, moveVec.z));
    }

	public override void OnActionReceived(ActionBuffers actions)
	{
		base.OnActionReceived(actions);


	}

	public override void CollectObservations(VectorSensor sensor)
	{
		base.CollectObservations(sensor);

        //if enemie null then vector 0 else pozycja

        Debug.Log("Collect call");
	}


	public void OnNewGeneration(float t)
    {
        PlayerEnt.OnNewGeneration(t);
        gameObject.SetActive(true);
    }
}