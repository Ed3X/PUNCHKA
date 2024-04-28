using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMesh : MonoBehaviour
{
    [SerializeField] private Transform movePositionTransform;

    private NavMeshAgent navMeshAgent;
    public Transform TrackerTransform;
    public float sightRadius = 5.0f;
    public float auditionRadius = 10f;
    public bool isChasing = false;

    private void Start()
    {
        TrackerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, TrackerTransform.position);

        if (distanceToPlayer <= sightRadius)
        {
            isChasing = true;
            navMeshAgent.destination = movePositionTransform.position;
        }
        else if(distanceToPlayer <= auditionRadius)
        {
            isChasing = true;
            navMeshAgent.destination = movePositionTransform.position;
        }
        else if(isChasing)
        {
            isChasing = false;
            navMeshAgent.ResetPath();
        }
        
    }
}
