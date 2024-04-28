using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightGizmo : MonoBehaviour
{
    public float DetectionRange;
    public float FieldOfView;
    public LayerMask WhatIsOpaque;

    public Transform _player;

    private void Start()
    {
        _player = GameObject.FindWithTag("Player").transform;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, DetectionRange);

        var direction = Quaternion.AngleAxis(FieldOfView / 2, transform.up) * transform.forward;
        Gizmos.DrawRay(transform.position, direction * DetectionRange);

        var direction2 = Quaternion.AngleAxis(-FieldOfView / 2, transform.up) * transform.forward;
        Gizmos.DrawRay(transform.position, direction2 * DetectionRange);

        Gizmos.color = Color.white;
    }
    void Update()
    {
        if (IsPlayerClose())
        {
            if (IsInFieldOfView())
            {
                Debug.Log(CanSeePlayer());
            }
        }
    }

    private object CanSeePlayer()
    {
        Vector3 direction = _player.position - transform.position;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, direction, out hit, DetectionRange, WhatIsOpaque))
        {
            if (hit.collider.transform == _player)
            {
                return true;
            }
        }
        return false;
    }

    private bool IsInFieldOfView()
    {
        Vector3 EP = _player.position - transform.position;
        float angle = Vector3.Angle(EP, transform.forward);

        return angle < FieldOfView / 2;
    }

    private bool IsPlayerClose()
    {
        return Vector3.Distance(transform.position, _player.position) < DetectionRange;
    }
}
