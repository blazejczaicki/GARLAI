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

        public ParticleSystem deathEffect;
        public static event System.Action OnDeathStatic;

        private NavMeshAgent pathfinder;
        private Transform target;
        private LivingEntity livingEntityTarget;
        Material skinMaterial;

        Color originalColor;

        float attackDistanceThreshold = 0.5f;
        float distanceThreshold = 0.3f;
        float damage = 1f;
        float nextAttackTime;
        float timeBetweenAttacks = 1;
        float myCollisionRadius;
        float targetCollisionRadius;

        bool hasTarget;

        public PlayerAI PlayerAI { get => playerAI; set => playerAI = value; }

        private void Awake()
        {
            pathfinder = GetComponent<NavMeshAgent>();

            if (GameObject.FindGameObjectWithTag("Player") != null)
            {
                hasTarget = true;

                target = GameObject.FindGameObjectWithTag("Player").transform;
                livingEntityTarget = target.GetComponent<LivingEntity>();

                myCollisionRadius = GetComponent<CapsuleCollider>().radius;
                targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;
            }
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
            pathfinder.speed = moveSpeed;

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

        private void Update()
        {
            if (hasTarget)
            {

            if (Time.time > nextAttackTime)
            {
                float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;
                if (sqrDstToTarget <= Mathf.Pow(attackDistanceThreshold + myCollisionRadius + targetCollisionRadius, 2))
                {
                        nextAttackTime = Time.time + timeBetweenAttacks;
                        AudioManager.instance.PlaySound("Enemy Attack", transform.position);
                        StartCoroutine(Attack());
                }
            }
            }
        }

        IEnumerator Attack()
        {
            pathfinder.enabled = false;
            currentState = State.Attacking;

            Vector3 originalPosition = transform.position;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            Vector3 attackPosition = target.position - dirToTarget * (myCollisionRadius);

            float attackSpeed = 3;
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
            pathfinder.enabled = true;
        }

        public override void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
        {
            AudioManager.instance.PlaySound("Impact", transform.position);
            if ((damage >= health && !dead))
            {
                OnDeathStatic?.Invoke();
                AudioManager.instance.PlaySound("Enemy Death", transform.position);
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
                    if (!dead)
                    {
                        pathfinder.SetDestination(targetPosition);
                    }
                }
                yield return new WaitForSeconds(refreshRate);
            }
        }
    }
}
