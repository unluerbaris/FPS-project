using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ES.Combat
{
    public class EnemyAttack : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] float damage = 10f;

        void Start()
        {

        }

        public void AttackHitEvent() // Animation event
        {
            if (target == null) return;
            Debug.Log("Enemy is attacking to " + target.name);
        }
    }
}
