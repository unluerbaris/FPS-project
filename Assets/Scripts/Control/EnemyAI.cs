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
        [SerializeField] float turnSpeed = 5f;

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
            FaceTarget(); // Look at the target first

            if (distanceToTarget > navMeshAgent.stoppingDistance)
            {
                ChaseTarget();
            }

            if (distanceToTarget <= navMeshAgent.stoppingDistance)
            {
                AttackTarget();
            }
        }

        public void OnDamageTaken() // Call this method with BroadcastMessage
        {
            isProvoked = true;
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

        public void FaceTarget()
        {
            Vector3 direction = target.position - transform.position;
            Vector3 normalDirection = direction.normalized;

            Quaternion lookRotation = Quaternion.LookRotation(new Vector3 (normalDirection.x,
                                                                           0,
                                                                           normalDirection.z));
            // Turn the enemy's body to the target smoothly with a rotation speed.
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                  lookRotation,
                                                  Time.deltaTime * turnSpeed);
        }

        void OnDrawGizmosSelected()
        {
            // Display the chaseRange radius when selected
            Gizmos.color = new Color(1, 1, 0, 0.75f); // yellow
            Gizmos.DrawWireSphere(transform.position, chaseRange);
        }
    }
}
