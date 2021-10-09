using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TopShooter
{
    public class Enemy : LivingEntity
    {
        public enum State
        {
            Idle, Chasing, Attacking
        }
        State currentState;

        [SerializeField] private PlayerAI playerAI;
        [SerializeField] private PlayerMLA playerMLA;

        public ParticleSystem deathEffect;
        public static event System.Action OnDeathStatic;

        private NavMeshAgent pathfinder;
        private Transform target;
        private LivingEntity livingEntityTarget;
        Material skinMaterial;

        Color originalColor;

        public float attackDistanceThreshold = 1f;
        float distanceThreshold = 0.3f;
        float damage = 1f;
        float nextAttackTime;
        float timeBetweenAttacks = 1;
        float myCollisionRadius;
        float targetCollisionRadius;


        float attactDist=1f;

        public float attackSpeed = 3;

        bool hasTarget;

        public PlayerAI PlayerAI { get => playerAI; set => playerAI = value; }
		public NavMeshAgent Pathfinder { get => pathfinder; set => pathfinder = value; }
		public PlayerMLA PlayerMLA { get => playerMLA; set => playerMLA = value; }

		private void Awake()
        {
            Pathfinder = GetComponent<NavMeshAgent>();
            myCollisionRadius = GetComponent<CapsuleCollider>().radius;
            hasTarget = true;
        }

        public void SetTarget(Transform target)
		{
            this.target = target.transform;
            livingEntityTarget = target.GetComponent<LivingEntity>();
            targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;
        }

        protected override void Start()
        {
            base.Start();
            OnDeath += OnDeadThis;
            if (hasTarget)
            {
                currentState = State.Chasing;
                livingEntityTarget.OnDeath += OnTargetDeath;

                StartCoroutine(UpdatePath());
            }
        }

        public void SetCharacteristics(float moveSpeed, int hitsToKillPlayer, float enemyHealth, Color skinColour)
        {
            //pathfinder.speed = moveSpeed;

            if (hasTarget)
            {
                damage = Mathf.Ceil(livingEntityTarget.startingHealth / hitsToKillPlayer);
            }
            startingHealth = enemyHealth;
            deathEffect.startColor = new Color(skinColour.r, skinColour.g, skinColour.b, 1);
            skinMaterial = GetComponent<Renderer>().material;
            //skinMaterial = GetComponent<Renderer>().sharedMaterial;
            skinMaterial.color = skinColour;
            originalColor = skinMaterial.color;
        }

        void OnDeadThis()
        {
            PlayerAI.Enemies.Remove(this);
        }

        void OnTargetDeath()
        {
            hasTarget = false;
            currentState = State.Idle;
        }

        public void OnUpdate()
        {
            if (hasTarget)
            {

            if (Time.time > nextAttackTime)
            {
                float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;
                    //if (sqrDstToTarget < attactDist)
                    //{
                        if (sqrDstToTarget <= Mathf.Pow(attackDistanceThreshold + myCollisionRadius + targetCollisionRadius, 2))
                        {
                            nextAttackTime = Time.time + timeBetweenAttacks;
                            StartCoroutine(Attack());
                        }
                    //}
            }
            }
        }

        IEnumerator Attack()
        {
            Pathfinder.enabled = false;
            currentState = State.Attacking;

            playerMLA.RewardPlayer();

            Vector3 originalPosition = transform.position;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            Vector3 attackPosition = target.position - dirToTarget * (myCollisionRadius);

            
            float percent = 0;

            bool hasAppliedDamage = false;

            skinMaterial.color = Color.red;
            while (percent <= 1)
            {
                if (percent>=0.5f && !hasAppliedDamage)
                {
                    hasAppliedDamage = true;
                    livingEntityTarget.TakeDamage(damage);
                }

                percent += Time.deltaTime * attackSpeed;
                float interpolation = (-(percent * percent) + percent) * 4;
                transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);
                yield return null;
            }
            skinMaterial.color = originalColor;
            currentState = State.Chasing;
            Pathfinder.enabled = true;
        }

        public override void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
        {
            if ((damage >= health && !Dead))
            {
                OnDeathStatic?.Invoke();
                Destroy(Instantiate(deathEffect.gameObject, hitPoint, Quaternion.FromToRotation(Vector3.forward, hitDirection)) as GameObject,  deathEffect.main.startLifetimeMultiplier);
            }
            base.TakeHit(damage, hitPoint, hitDirection);
        }

        IEnumerator UpdatePath()
        {
            float refreshRate = 0.25f;

            while (hasTarget)
            {
                if (currentState == State.Chasing)
                {
                    Vector3 dirToTarget = (target.position - transform.position).normalized;
                    Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + distanceThreshold);
                    if (!Dead)
                    {
                        Pathfinder.SetDestination(targetPosition);
                    }
                }
                yield return new WaitForSeconds(refreshRate);
            }
        }
    }
}
