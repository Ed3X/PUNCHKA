using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHandler : MonoBehaviour
{
    private ObjectFader _fader;
    [SerializeField] private float sphereCastRadius = 10f; // Adjust radius as needed

    private void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("BuildingPlayerHandler");

        if (player != null)
        {
            Vector3 dir = player.transform.position - transform.position;
            RaycastHit hit;

            // Perform SphereCast instead of Raycast
            if (Physics.SphereCast(transform.position, sphereCastRadius, dir, out hit))
            {
                if (hit.collider == null)
                {
                    return;
                }

                if (hit.collider.gameObject == player)
                {
                    // Nothing in front of the player
                    if (_fader != null)
                    {
                        // PROBLEM HERE doesn't identify player
                        _fader.DoFade = false;
                    }
                }
                else
                {
                    _fader = hit.collider.gameObject.GetComponent<ObjectFader>();
                    if (_fader != null)
                    {
                        _fader.DoFade = true;
                        // You might want to check the distance of the hit object 
                        // for a smoother fade effect based on proximity
                    }
                }
            }
        }
    }
}