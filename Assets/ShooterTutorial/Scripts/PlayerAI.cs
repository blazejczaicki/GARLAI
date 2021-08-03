using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace TopShooter
{
    public class PlayerAI : Player
    {
        private DecisionTree decisionTree;
        private PlayerShooter playerShooter;
        private CharacterController characterController;
        private Astar astarPathfinding;
        private GA_Chromosome chromosome;


        [SerializeField] private Vector3 targetPosition;
        private List<AstarNode> astarNodes= new List<AstarNode>();
        private Queue<Vector3> path = new Queue<Vector3>();
        [SerializeField] private Vector3 currentTarget;
        [SerializeField] private float radius = 0.1f;
        [SerializeField] private float speed = 10f;
        public float LifeTime { get; set; }
        List<Enemy> enemiesTooClose = new List<Enemy>();

        [SerializeField] private Vector3 movementPosition;
        [SerializeField] private MapData mapData;

        [SerializeField] private Vector3 debugTargetPos;       

        //AI datas
        [SerializeField] private float decisionUpdateTime=0.25f;
        private float previousUpdateTime = 0;
        [SerializeField] private List<Enemy> enemies;
        [SerializeField] private AreaManager areaManager;
        //retreat
        [SerializeField] private List<DataAI> dataAI;

        public event Action<List<DataAI>> OnPlayerClick;

        public List<Enemy> Enemies { get => enemies; set => enemies = value; }
        public AreaManager AreaManager { get => areaManager; set => areaManager = value; }
		public Vector3 CurrentTarget { get => currentTarget; set => currentTarget = value; }
		public List<DataAI> DataAI { get => dataAI; set => dataAI = value; }
		public MapData MapData { get => mapData; set => mapData = value; }
		public CharacterController CharController { get => characterController; set => characterController = value; }
		public List<Enemy> EnemiesTooClose { get => enemiesTooClose; set => enemiesTooClose = value; }
		public float DecisionUpdateTime { get => decisionUpdateTime; set => decisionUpdateTime = value; }
		public GA_Chromosome Chromosome { get => chromosome; set => chromosome = value; }

		private void Awake()
        {
            Chromosome = GetComponent<GA_Chromosome>();
            CharController = GetComponent<CharacterController>();
            decisionTree = GetComponent<DecisionTree>();
            playerShooter = GetComponent<PlayerShooter>();

            astarPathfinding = new Astar();
            Enemies = new List<Enemy>();
            decisionTree.CreateWalkModeTree();
        }

        private void Start()
        {
            movementPosition = transform.position;
        }

        public float GetAverageHealth()
        {
            return playerShooter.HealthOnSeconds;
        }

        public float GetLifeTime()
        {
            return playerShooter.LifeTime;
        }

        public void ResetPlayerWorld()
		{
            this.StopAllCoroutines();
            targetPosition = transform.position;
            previousUpdateTime = Time.time;
            path.Clear();
            for (int i = 0; i < astarNodes.Count; i++)
            {
                astarNodes[i].debugTile.sharedMaterial.color = Color.black;
            }
            astarNodes.Clear();
        }

        public void OnUpdate()
        {
			UpdateDecisions();
			MoveOnPath();
			MoveCR();
		}

        private void UpdateDecisions()
        {
            if (Time.time-previousUpdateTime>DecisionUpdateTime)
            {
                previousUpdateTime = Time.time;
                var decisionQueue = decisionTree.MakeDecision(this);
				if (decisionQueue.Count>0)
				{
                    targetPosition = decisionQueue.Dequeue(); 
				}
                //StartCoroutine(FindPath());
                FindPathh();
            }
        }

        private IEnumerator FindPath()
		{
            MapData.ResetMapData();
            yield return null;
			if (gameObject.activeSelf==false)
			{
                Debug.Log("xd");
			}
            CalculatePath();
        }

        private void FindPathh()
		{
            MapData.ResetMapData();
            CalculatePath();
        }

        [EasyButtons.Button]
        private void CalculatePath()
        {
            path.Clear();
			for (int i = 0; i < astarNodes.Count; i++)
			{
                astarNodes[i].debugTile.sharedMaterial.color = Color.black;
			}
            astarNodes = astarPathfinding.FindPath(MapData.ConvertToMapGridPos(transform.position), MapData.ConvertToMapGridPos(targetPosition), MapData.AstarNodesMap, MapData);
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
        }

        private void MoveOnPath() //wykonuje ruch po œcie¿ce
		{
			if (path.Count>0 && (transform.position-CurrentTarget).sqrMagnitude<radius)
			{
                CurrentTarget = path.Dequeue();
			}
		}

        private void MoveCR()
		{
            var moveVec = (currentTarget - transform.position).normalized * speed;// * Time.deltaTime;
            CharController.SimpleMove(new Vector3(moveVec.x,0,moveVec.z));
		}

        public void UpdateCloseEnemies(float minEnemyDistance)
        {
            var count = Enemies.Count;
            for (int i = 0; i < count; i++)
            {
                if (minEnemyDistance > Vector3.Distance(transform.position, Enemies[i].transform.position))
                {
                    enemiesTooClose.Add(Enemies[i]);
                }
            }
        }

        public Vector3 GetAverageDirectionByEnemies()
		{
            Vector3 averageDirection = Vector3.zero;
            foreach (var etc in enemiesTooClose)
            {
                averageDirection = (averageDirection + (etc.transform.position - transform.position).normalized).normalized;
            }
            return averageDirection;
        }

        public void OnNewGeneration()
		{
            Enemies.Clear();
            playerShooter.OnNewGeneration();
        }

		private void OnMouseDown()
		{
            OnPlayerClick(dataAI);
		}

		public Vector3 gobackDir=Vector3.zero;
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(movementPosition, 0.5f);
            Gizmos.color = Color.magenta;
            Handles.color = Color.magenta;
            gobackDir[1] = 0;
            Handles.DrawLine(transform.position, transform.position + gobackDir, 5);
            //Gizmos.DrawRay(transform.position, gobackDir);
        }
    }    

    public enum VariableName
	{
        EnemiesTooCloseDist,
        SafetyAroundDist,
        GoBackEscapeDist,
        WallBackEscapeDist,
        SouroundingDist,
        MinSouroundingDist,
        SouroundingWallDist,
        StayTimeUpdate,
        SafetyTimeUpdate,
        SouroundingTimeUpdate,
        RetreatTimeUpdate,
        WallRetreatTimeUpdate,
	}

    [Serializable]
    public class DataAI
	{
        public VariableName nameVal;
        public float currentVal;
        public float maxVal;
        public float minVal;
	}
}