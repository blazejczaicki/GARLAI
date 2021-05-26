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

        private void Awake()
        {
            pathfinder = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            movementPosition = transform.position;
        }

        private void Update()
        {
            Move();
        }

        public void Move()
        {
            pathfinder.SetDestination(movementPosition);
        }
    }
}