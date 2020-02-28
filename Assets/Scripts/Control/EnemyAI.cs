using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ES.Control
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] float chaseRange = 8f;

        NavMeshAgent navMeshAgent;
        Animator animator;

        float distanceToTarget = Mathf.Infinity;
        bool isProvoked = false;

        void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            distanceToTarget = Vector3.Distance(target.position, transform.position);
            if (isProvoked)
            {
                EngageTarget();
            }
            else if(distanceToTarget <= chaseRange)
            {
                isProvoked = true;
            }
        }

        private void EngageTarget()
        {
            if (distanceToTarget > navMeshAgent.stoppingDistance)
            {
                ChaseTarget();
            }

            if (distanceToTarget <= navMeshAgent.stoppingDistance)
            {
                AttackTarget();
            }
        }

        private void ChaseTarget()
        {
            animator.SetBool("attack", false);
            animator.SetTrigger("move");
            navMeshAgent.SetDestination(target.position);
        }

        private void AttackTarget()
        {
            animator.SetBool("attack", true);
        }

        void OnDrawGizmosSelected()
        {
            // Display the chaseRange radius when selected
            Gizmos.color = new Color(1, 1, 0, 0.75f); // yellow
            Gizmos.DrawWireSphere(transform.position, chaseRange);
        }
    }
}
