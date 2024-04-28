using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TutorialController : MonoBehaviour
{
    public GameObject pantallaTutorial1;
    public GameObject pantallaTutorial2;


    private bool tutorial1Visible = true;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (tutorial1Visible)
            {
                pantallaTutorial1.SetActive(false);
                pantallaTutorial2.SetActive(true);
                tutorial1Visible = false;
            }
            else
            {
                pantallaTutorial2.SetActive(false);
            }
        }
    }
}
