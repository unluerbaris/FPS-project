using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ES.Core;

namespace ES.Combat
{
    public class EnemyAttack : MonoBehaviour
    {
        PlayerHealth target;
        [SerializeField] float damage = 10f;

        void Start()
        {
            target = FindObjectOfType<PlayerHealth>();
        }

        public void AttackHitEvent() // Animation event
        {
            if (target == null) return;
            target.TakeDamage(damage);
            target.GetComponent<DisplayDamage>().DisplayDamageImpact();
        }
    }
}
