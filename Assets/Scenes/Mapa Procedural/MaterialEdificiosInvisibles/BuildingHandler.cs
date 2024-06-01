using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHandler : MonoBehaviour
{
    private ObjectFader _fader;

    private void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("BuildingPlayerHandler");

        if(player != null)
        {
            Vector3 dir = player.transform.position - transform.position;
            Ray ray = new Ray(transform.position, dir);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                if(hit.collider == null)
                {
                    return;
                }

                if(hit.collider.gameObject == player)
                {
                    // nothing in front of the player
                    if(_fader != null)
                    {
                        // PROBLEM HERE doesn't identify player
                        _fader.DoFade = false;
                    }
                }

                else
                {
                    _fader = hit.collider.gameObject.GetComponent<ObjectFader>();
                    if(_fader != null)
                    {
                        _fader.DoFade = true;
                    }
                }
            }
        }
    }
}
