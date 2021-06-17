using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TopShooter
{
    public class PlayerAI : Player
    {
        private DecisionTree decisionTree;
        private NavMeshAgent pathfinder;
        //[SerializeField] private float moveSpeed = 5;
        //[SerializeField] private float scaleeTime = 0.2f;
        [SerializeField] private Vector3 movementPosition;



        private float refreshMovementRate = 0.25f;

       

        //AI datas
        [SerializeField] private float minEnemyDistance=3;
        [SerializeField] private float minSouroundingDistance=3;
        [SerializeField] private float decisionUpdateTime=0.25f;
        [SerializeField] private float minimalRetreatDistance=6;
        private float previousUpdateTime = 0;
        [SerializeField] private List<Enemy> enemies;
        [SerializeField] private AreaManager areaManager;


        public List<Enemy> Enemies { get => enemies; set => enemies = value; }
        public float MinEnemyDistance { get => minEnemyDistance; set => minEnemyDistance = value; }
        public float MinimalRetreatDistance { get => minimalRetreatDistance; set => minimalRetreatDistance = value; }
        public AreaManager AreaManager { get => areaManager; set => areaManager = value; }
        public float MinSouroundingDistance { get => minSouroundingDistance; set => minSouroundingDistance = value; }

        private void Awake()
        {
            pathfinder = GetComponent<NavMeshAgent>();
            //pathfinder.enabled = false;
            Enemies = new List<Enemy>();
            decisionTree = GetComponent<DecisionTree>();
            decisionTree.CreateTmpTree();
        }

        private void Start()
        {
            movementPosition = transform.position;
        }

        private void Update()
        {
            UpdateDecisions();
            Move();
        }

        private void UpdateDecisions()
        {
            if (Time.time-previousUpdateTime>decisionUpdateTime)
            {
                previousUpdateTime = Time.time;
                movementPosition = decisionTree.MakeDecision(this);
            }
        }

        public void Move()
        {
            pathfinder.SetDestination(movementPosition);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(movementPosition, 0.5f);
        }
    }
}