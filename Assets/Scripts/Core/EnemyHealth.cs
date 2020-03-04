using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ES.Core
{
    public class EnemyHealth : MonoBehaviour
    {
        [SerializeField] float hitPoints = 100f;

        public void TakeDamage(float damage)
        {
            BroadcastMessage("OnDamageTaken"); // Call OnDamageTaken to provoke the enemy

            hitPoints -= damage;
            if(hitPoints <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
