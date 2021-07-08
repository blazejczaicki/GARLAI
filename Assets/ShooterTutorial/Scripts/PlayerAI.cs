using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace TopShooter
{
    public class PlayerAI : Player
    {
        private DecisionTree decisionTree;
        private NavMeshAgent pathfinder;
        private CharacterController characterController;
        private Astar astarPathfinding;


        [SerializeField] private Vector3 targetPosition;
        private List<AstarNode> astarNodes= new List<AstarNode>();
        private Queue<Vector3> path = new Queue<Vector3>();
        [SerializeField] private Vector3 currentTarget;
        [SerializeField] private float radius = 0.1f;
        [SerializeField] private float speed = 10f;

        //[SerializeField] private float moveSpeed = 5;
        //[SerializeField] private float scaleeTime = 0.2f;
        
        [SerializeField] private Vector3 movementPosition;
        [SerializeField] private MapData mapData;

        [SerializeField] private Vector3 debugTargetPos;

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
		public Vector3 CurrentTarget { get => currentTarget; set => currentTarget = value; }

		private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            astarPathfinding = new Astar();
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
            //UpdateDecisions();
            MoveOnPath();
            MoveCR();
            //Move();
        }

        private void UpdateDecisions()
        {
            if (Time.time-previousUpdateTime>decisionUpdateTime)
            {
                previousUpdateTime = Time.time;
                targetPosition = decisionTree.MakeDecision(this).First(); // tu tylko pozycjê ostateczn¹
                CalculatePath(targetPosition);
            }
        }

        [EasyButtons.Button]
        private void CalculatePath()
        {
            path.Clear();
			for (int i = 0; i < astarNodes.Count; i++)
			{
                astarNodes[i].debugTile.sharedMaterial.color = Color.black;
			}
            astarNodes = astarPathfinding.FindPath(mapData.ConvertToMapGridPos(transform.position), mapData.ConvertToMapGridPos(targetPosition), mapData.AstarNodesMap, mapData);
            astarNodes.ForEach(x => path.Enqueue(x.position3d));
			if (path.Count>1)
			{
                CurrentTarget = path.Dequeue();
                CurrentTarget = path.Dequeue();
			}
            for (int i = 0; i < astarNodes.Count; i++)
            {
                astarNodes[i].debugTile.sharedMaterial.color = Color.yellow;
            }
            Debug.Log(astarNodes.Count);
            StartCoroutine(mapData.ResetMapData());
        }

        private void CalculatePath(Vector3 targetPosition)
		{
            astarNodes.ForEach(x => x.debugTile.sharedMaterial.color = Color.black);
            astarNodes= astarPathfinding.FindPath(mapData.ConvertToMapGridPos(transform.position), mapData.ConvertToMapGridPos(targetPosition), mapData.AstarNodesMap, mapData);
		}

        private void MoveOnPath() //wykonuje ruch po œcie¿ce
		{
			if (path.Count>0 && (transform.position-CurrentTarget).sqrMagnitude<radius)
			{
                Debug.Log("xdmop");
                CurrentTarget = path.Dequeue();
			}
		}

        private void MoveCR()
		{
            var moveVec = (currentTarget - transform.position).normalized * speed;// * Time.deltaTime;
            characterController.SimpleMove(new Vector3(moveVec.x,0,moveVec.z));
		}

        public void Move()
        {
            pathfinder.SetDestination(CurrentTarget);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(movementPosition, 0.5f);
        }
    }
}