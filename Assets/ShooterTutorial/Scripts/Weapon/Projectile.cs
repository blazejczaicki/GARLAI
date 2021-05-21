using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopShooter
{
    public class Projectile : MonoBehaviour
    {
        public LayerMask collisionMask;
        public Color trailColour;
        private float speed = 10;
        private float damage = 1;

        float lifeTime = 2f;
        float skinWidth = 0.1f;

        public float Speed { get => speed; set => speed = value; }

        private void Start()
        {
            Destroy(gameObject, lifeTime);

            Collider[] initialCollisions = Physics.OverlapSphere(transform.position, 0.1f, collisionMask);
            if (initialCollisions.Length>0)
            {
                OnHitObject(initialCollisions[0], transform.position);
            }
            GetComponent<TrailRenderer>().material.SetColor("_TintColor", trailColour);
        }

        private void Update()
        {
            float moveDistance = speed * Time.deltaTime;
            CheckCollisions(moveDistance);
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }

        private void CheckCollisions(float moveDistance)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, moveDistance + skinWidth, collisionMask, QueryTriggerInteraction.Collide))
            {
                OnHitObject(hit.collider, hit.point);
            }
        }

        private void OnHitObject(Collider c, Vector3 hitPoint)
        {
            IDamageable damageableObject = c.GetComponent<IDamageable>();
            if (damageableObject!=null)
            {
                damageableObject.TakeHit(damage, hitPoint, transform.forward);
            }
            GameObject.Destroy(gameObject);
        }
    }
}
