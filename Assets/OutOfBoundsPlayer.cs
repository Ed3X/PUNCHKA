using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBoundsPlayer : MonoBehaviour
{
    public Transform PlayerGroundChecker;
    public GameObject Player;
    public float groundCheckerRadius = 0.1f;
    public LayerMask WhatIsWalkable;

    // Update is called once per frame
    void Update()
    {
        if ((!IsInWalkable()))
        {
            Debug.Log("Out of bounds");
            if(Player != null)
            {
                Debug.Log("Out of bounds player being moved");
                Player.transform.position = new Vector3(0f,0f,0f);
            }
            
        }
        
    }

    private bool IsInWalkable()
    {
        return Physics.CheckSphere(PlayerGroundChecker.position, groundCheckerRadius, WhatIsWalkable);
    }
}
