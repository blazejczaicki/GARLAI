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

    int mask = 1 << 10;

    private float previousUpdateTime = 0;
    private float roundTimeSpan = 1;

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

		if (Time.time - previousUpdateTime > roundTimeSpan)
        {
            previousUpdateTime = Time.time;
            SetReward(1f);
		}

        var moveVec = new Vector3(x,0,z) * Speed*Time.deltaTime;
        CharController.Move(moveVec);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);
        sensor.AddObservation(transform.position);
        RaycastHit hit;
		if (Physics.Raycast(transform.position, Vector3.forward, out hit, 21, mask))
		{
			//Debug.Log(hit.point);
			sensor.AddObservation(hit.point);
			//forward = hit.point;
		}
		else
		{
			sensor.AddObservation(Vector3.zero);
		}
		if (Physics.Raycast(transform.position, Vector3.back, out hit, 21, mask))
		{
			// Debug.Log(hit.point);
			sensor.AddObservation(hit.point);
			//back = hit.point;
		}
		else
		{
			sensor.AddObservation(Vector3.zero);
		}
		if (Physics.Raycast(transform.position, Vector3.right, out hit, 21, mask))
		{
			//Debug.Log(hit.point);
			sensor.AddObservation(hit.point);
		}
		else
		{
			sensor.AddObservation(Vector3.zero);
		}
		if (Physics.Raycast(transform.position, Vector3.left, out hit, 21, mask))
		{
			//Debug.Log(hit.point);
			sensor.AddObservation(hit.point);
		}
		else
		{
			sensor.AddObservation(Vector3.zero);
		}

		// Debug.Log("Collect call");
		for (int i = 0; i < 10; i++)
        {
            if (mapData.EnemiesMLA[i] != null)
            {
                sensor.AddObservation(mapData.EnemiesMLA[i].transform.position);
            }
            else
            {
                sensor.AddObservation(Vector3.zero);
            }
        }

    }

	private void OnCollisionEnter(Collision collision)
	{
        SetReward(-2f);
	}

	public void RewardPlayer()
	{
        SetReward(-5);
	}

	public void OnNewGeneration()
    {
        EndEpisode();
    }

    Vector3 right, left, forward, back;
	//private void OnDrawGizmos()
	//{
 //       Gizmos.color = Color.blue;
 //       Gizmos.DrawLine(transform.position, forward);

 //       Gizmos.color = Color.yellow;
 //       Gizmos.DrawLine(transform.position, back);

 //   }
}
