using System;
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

    public Transform EnemyGroundChecker;
    public float groundSphereRadius = 0.1f;
    public LayerMask WhatIsWalkable;

    public Transform closestCalle;

    private void Start()
    {
        TrackerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        closestCalle = FindClosestCalle();
    }

    private Transform FindClosestCalle()
    {
        Transform closest = null;
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {            
            if (obj.layer == LayerMask.NameToLayer("Calle"))
            {
                float distance = Vector3.Distance(transform.position, obj.transform.position);
                if(distance < 5)
                {
                    closest = obj.transform;
                    return closest;
                }
            }
        }
        return closest;
    }

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (!isInWalkable())
        {
            transform.position = closestCalle.position;
        }

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

    private bool isInWalkable()
    {
        return Physics.CheckSphere(EnemyGroundChecker.position, groundSphereRadius, WhatIsWalkable);
    }
}
