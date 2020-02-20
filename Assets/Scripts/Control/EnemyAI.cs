using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ES.Control
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] Transform target;
        NavMeshAgent navMeshAgent;

        void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            navMeshAgent.SetDestination(target.position);
        }
    }
}
