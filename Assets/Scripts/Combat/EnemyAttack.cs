using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ES.Core;
using UnityEngine.Events;

namespace ES.Combat
{
    public class EnemyAttack : MonoBehaviour
    {
        PlayerHealth target;
        [SerializeField] float damage = 10f;

        [SerializeField] UnityEvent onAttack;

        void Start()
        {
            target = FindObjectOfType<PlayerHealth>();
        }

        public void AttackHitEvent() // Animation event
        {
            if (target == null) return;
            onAttack.Invoke();
            target.TakeDamage(damage);
            target.GetComponent<DisplayDamage>().DisplayDamageImpact();
        }
    }
}
