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

        float distanceToTarget = Mathf.Infinity;

        void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            distanceToTarget = Vector3.Distance(target.position, transform.position);
            if (distanceToTarget <= chaseRange)
            {
                navMeshAgent.SetDestination(target.position);
            }
        }

        void OnDrawGizmosSelected()
        {
            // Display the chaseRange radius when selected
            Gizmos.color = new Color(1, 1, 0, 0.75f); // yellow
            Gizmos.DrawWireSphere(transform.position, chaseRange);
        }
    }
}
