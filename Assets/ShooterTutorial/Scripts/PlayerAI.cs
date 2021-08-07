using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Unity.Profiling;
using UnityEngine.AI;

namespace TopShooter
{
    public class PlayerAI : MonoBehaviour
    {
        private DecisionTree decisionTree;
        private BayesNet bayesianNet;
        private PlayerShooter playerShooter;
        private CharacterController characterController;
        private Astar astarPathfinding;
        public GA_Chromosome chromosome { get; set; }
        [SerializeField] private bool isBayesian = false;


        [SerializeField] private Vector3 targetPosition;
        private List<AstarNode> astarNodes= new List<AstarNode>();
        private Queue<Vector3> path = new Queue<Vector3>();
        [SerializeField] private Vector3 currentTarget;
        [SerializeField] private float radius = 0.1f;
        [SerializeField] private float speed = 10f;
        List<Enemy> enemiesTooClose = new List<Enemy>();

        [SerializeField] private MapData mapData;
    

        //AI datas
        [SerializeField] private float decisionUpdateTime=0.25f;
        private float previousUpdateTime = 0;
        [SerializeField] private List<Enemy> enemies;
        //retreat
        [SerializeField] private List<DataAI> dataAI;

        public event Action<List<DataAI>> OnPlayerClick;

        public List<Enemy> Enemies { get => enemies; set => enemies = value; }
		public Vector3 CurrentTarget { get => currentTarget; set => currentTarget = value; }
		public List<DataAI> DataAI { get => dataAI; set => dataAI = value; }
		public MapData MapData { get => mapData; set => mapData = value; }
		public CharacterController CharController { get => characterController; set => characterController = value; }
		public float DecisionUpdateTime { get => decisionUpdateTime; set => decisionUpdateTime = value; }
		public bool IsBayesian { get => isBayesian; set => isBayesian = value; }
		public BayesNet BayesianNet { get => bayesianNet; set => bayesianNet = value; }

		static readonly ProfilerMarker s_PreparePerfMarker = new ProfilerMarker("MySystem.Prepare");
        private void Awake()
        {
			if (IsBayesian)
			{
                chromosome = new GA_BayesChromosome(this);
            }
			else
			{
                chromosome = new GA_DT_Chromosome(this);
			}

            CharController = GetComponent<CharacterController>();
            decisionTree = GetComponent<DecisionTree>();
            playerShooter = GetComponent<PlayerShooter>();
            BayesianNet = GetComponent<BayesNet>();

            astarPathfinding = new Astar();
            Enemies = new List<Enemy>();
			if (!IsBayesian)
			{
                decisionTree.CreateWalkModeTree();
			}
        }

        public float GetAverageHealth()
        {
            return playerShooter.HealthOnSeconds;
        }

        public float GetRestHealth()
        {
            return playerShooter.health;
        }

        public float GetLifeTime()
        {
            return playerShooter.LifeTime;
        }

        public void OnEndGeneration()
		{
            playerShooter.OnEndGeneration();
		}

        public void ResetPlayerWorld()
		{
            this.StopAllCoroutines();
            targetPosition = transform.position;
            previousUpdateTime = Time.time;
            path.Clear();
            if (mapData.DebugMode)
            {
                for (int i = 0; i < astarNodes.Count; i++)
                {
                    astarNodes[i].debugTile.sharedMaterial.color = Color.black;
                }
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
                if (IsBayesian)
                {
                    var decisionQueue = BayesianNet.InferNet(this);
                    if (decisionQueue.Count > 0)
                    {
                        targetPosition = decisionQueue.Dequeue();
                    }
                }
                else
                {
                    var decisionQueue = decisionTree.MakeDecision(this);
                    if (decisionQueue.Count > 0)
                    {
                        targetPosition = decisionQueue.Dequeue();
                    }
                }
                //StartCoroutine(FindPath());
                s_PreparePerfMarker.Begin();
                FindPathh();
                s_PreparePerfMarker.End();
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
			if (mapData.DebugMode)
			{
			    for (int i = 0; i < astarNodes.Count; i++)
			    {
                    astarNodes[i].debugTile.sharedMaterial.color = Color.black;
			    }
			}
            astarNodes = astarPathfinding.FindPath(MapData.ConvertToMapGridPos(transform.position), MapData.ConvertToMapGridPos(targetPosition), MapData.AstarNodesMap, MapData);
            astarNodes.ForEach(x => path.Enqueue(x.position3d));
			if (path.Count>1)
			{
                CurrentTarget = path.Dequeue();
                CurrentTarget = path.Dequeue();
			}
			if (mapData.DebugMode)
			{
                for (int i = 0; i < astarNodes.Count; i++)
                {
                    astarNodes[i].debugTile.sharedMaterial.color = Color.yellow;
                }
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
            enemiesTooClose.Clear();
            Enemies.Clear();
            playerShooter.OnNewGeneration();
            gameObject.SetActive(true);
        }

		private void OnMouseDown()
		{
            OnPlayerClick(dataAI);
		}

		public Vector3 gobackDir=Vector3.zero;
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
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