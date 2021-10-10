using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopShooter
{
    public class LivingEntity : MonoBehaviour, IDamageable
    {
        public float startingHealth;
        public float health { get; protected set; }
		public bool Dead { get => dead; }

		protected bool dead;

		public event System.Action OnDeath;

        protected virtual void Start()
        {
            health = startingHealth;
        }

        public virtual void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
        {
            // Do some stuff here with hit var
            TakeDamage(damage);
        }

        public virtual void TakeDamage(float damage)
        {
			if (health>0)
			{
                health -= damage;
			}
            if (health <= 0 && !Dead)
            {
                Die();
            }
        }

        [ContextMenu("Self Destruct")]
        public virtual void Die()
        {
            dead = true;
            OnDeath?.Invoke();
            //Destroy(gameObject);            
        }
    }
}
