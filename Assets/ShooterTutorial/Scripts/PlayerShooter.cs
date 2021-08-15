using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

namespace TopShooter
{
    public class PlayerShooter : LivingEntity
    {
        public float moveSpeed = 5;
        public float scaleeTime = 0.2f;

        Camera viewCamera;
        GunController gunController;

        private float previousUpdateTime = 0;
        private float dataTime = 1f;

        private float healthOnSeconds = 0;
        private float seconds = 0;
        private float lifeTime = 0;

		public float HealthOnSeconds { get => healthOnSeconds/seconds; }
		public float LifeTime { get => lifeTime; set => lifeTime = value; }

		private void Awake()
        {
            gunController = GetComponent<GunController>();
            viewCamera = Camera.main;
            //stopWatch = new Stopwatch();
        }

        protected override void Start()
        {
            //stopWatch.Start();
            base.Start();
            gunController.EquipGun(0);
            health = startingHealth;
        }

        public void OnEndGeneration()
        {
            //if (stopWatch.IsRunning)
            //{
            //    stopWatch.Stop();
            //    var ts = stopWatch.Elapsed;
            //    LifeTime = Mathf.Floor(ts.Seconds);
            //}
            if (lifeTime == 0)
			{
                lifeTime = seconds;
            }
            StopAllCoroutines();
        }

        public void OnStart(float time)
        {
           previousUpdateTime = time;
        }

        public void OnNewGeneration(float time)
        {
            health = startingHealth;
            dead = false;
            LifeTime = 0;
            seconds = 0;
            healthOnSeconds = 0;
            previousUpdateTime = time;
            //stopWatch.Restart();
        }

        public void OnUpdate(float time)
        {
            if (time - previousUpdateTime > dataTime)
            {
                previousUpdateTime = time;
                healthOnSeconds += health;
                seconds++;
            }    
        }

        public override void Die()
        {
            //stopWatch.Stop();
            //var ts = stopWatch.Elapsed;
            //LifeTime = Mathf.Floor(ts.Seconds);
            lifeTime = seconds;
            //UnityEngine.Debug.Log("Tyle przeżył: " + stopWatch.ElapsedMilliseconds);
            base.Die();
        }
    }
}
