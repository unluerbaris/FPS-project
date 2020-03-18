using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ES.Core
{
    public class EnemyHealth : MonoBehaviour
    {
        [SerializeField] float hitPoints = 100f;
        bool isDead = false;

        [SerializeField] UnityEvent onTakeDamage;

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(float damage)
        {
            onTakeDamage.Invoke();
            BroadcastMessage("OnDamageTaken"); // Call OnDamageTaken to provoke the enemy

            hitPoints -= damage;
            if(hitPoints <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if (isDead) return;
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
        }
    }
}
