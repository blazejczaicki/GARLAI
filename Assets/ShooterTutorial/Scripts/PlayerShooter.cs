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
        PlayerControllerShooter controller;
        GunController gunController;

        private Stopwatch stopWatch;

        //public Crosshairs crosshairs;

        private void Awake()
        {
            controller = GetComponent<PlayerControllerShooter>();
            gunController = GetComponent<GunController>();
            viewCamera = Camera.main;
            //FindObjectOfType<Spawner>().OnNewWave += OnNewWave;
            stopWatch = new Stopwatch();
        }

        protected override void Start()
        {
            stopWatch.Start();
            base.Start();
            gunController.EquipGun(0);
            health = startingHealth;
        }

        void OnNewWave(int waveNumber)
        {
            health = startingHealth;
            gunController.EquipGun(waveNumber - 1);
        }

        private void Update()
        {
            Time.timeScale = scaleeTime;

            Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

            Vector3 moveVelocity = moveInput.normalized * moveSpeed;
            controller.Move(moveVelocity);

            Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

            if (groundPlane.Raycast(ray, out float rayDistance))
            {
                Vector3 point = ray.GetPoint(rayDistance);
                UnityEngine.Debug.DrawLine(ray.origin, point, Color.red);
                controller.LookAtMouse(point);
                //crosshairs.transform.position = point;
                //crosshairs.DetectTargets(ray);
                if ((new Vector2(point.x, point.z) - new Vector2(transform.position.x, transform.position.z)).sqrMagnitude > 1)
                {
                    gunController.Aim(point);
                }
            }

            // Weapon input
            if (Input.GetMouseButton(0))
            {
                gunController.OnTriggerHold();
            }
            if (Input.GetMouseButtonUp(0))
            {
                gunController.OnTriggerRelease();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                gunController.Reload();
            }

            if (transform.position.y < -10)
            {
                TakeDamage(health);
            }
        }

        public override void Die()
        {
            AudioManager.instance.PlaySound("Player Death", transform.position);
            stopWatch.Stop();
            UnityEngine.Debug.Log("Tyle przeżył: " + stopWatch.ElapsedMilliseconds);
            base.Die();
        }
    }
}
