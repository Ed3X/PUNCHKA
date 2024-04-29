using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurt_Layout : MonoBehaviour
{
    public GameObject hurtLayout;

    public bool hurtVisible = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            if(hurtVisible)
            {
                hurtLayout.SetActive(false);
                hurtVisible = false;
            }
            else if (!hurtVisible)
            {
                hurtLayout.SetActive(true);
                hurtVisible = true;
            }
        }
    }
}
