using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHandler : MonoBehaviour
{
    private ObjectFader _currentFader;
    private ObjectFader _previousFader;
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
                    if (_currentFader != null)
                    {
                        _currentFader.DoFade = false;
                        _currentFader = null;
                    }
                }
                else
                {
                    _currentFader = hit.collider.gameObject.GetComponent<ObjectFader>();
                    if (_currentFader != null)
                    {
                        _currentFader.DoFade = true;
                    }

                    // Restore the previous fader if it's different from the current one
                    if (_previousFader != null && _previousFader != _currentFader)
                    {
                        _previousFader.DoFade = false;
                    }

                    // Update the previous fader to the current one
                    _previousFader = _currentFader;
                }
            }
            else
            {
                // No hit, restore the previous fader if exists
                if (_currentFader != null)
                {
                    _currentFader.DoFade = false;
                    _currentFader = null;
                }

                if (_previousFader != null)
                {
                    _previousFader.DoFade = false;
                    _previousFader = null;
                }
            }
        }
    }
}
