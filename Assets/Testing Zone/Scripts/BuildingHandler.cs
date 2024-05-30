using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHandler : MonoBehaviour
{
    public Transform playerTransform; // Reference to the player's transform
    public LayerMask ignoreLayers; // Layers to ignore during raycast (e.g., UI elements)

    private bool isPlayerVisible;

    void Update()
    {
        // Cast a ray from the camera to the player
        Vector3 direction = playerTransform.position - transform.position;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity, ~ignoreLayers))
        {
            // Check if the raycast hit anything besides the player
            if (hit.transform != playerTransform)
            {
                isPlayerVisible = false;
            }
            else
            {
                isPlayerVisible = true;
            }
        }
        else
        {
            // If the raycast didn't hit anything, the player is visible
            isPlayerVisible = true;
        }

        if(isPlayerVisible)
        {
            Debug.Log("Visible");
        }
        else if(!isPlayerVisible)
        {
            Debug.Log("Non-visible");
        }

        // You can use the isPlayerVisible flag in other scripts to trigger actions
        // when the player is not directly visible.
    }
}
