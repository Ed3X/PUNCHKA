using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchImages : MonoBehaviour
{
    public RawImage[] offImages;
    public RawImage[] onImages;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchImage(2); // Hide ON_Blue, Show OFF_Blue
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchImage(3); // Hide ON_Green, Show OFF_Green
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwitchImage(1); // Hide ON_Yellow, Show OFF_Yellow
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SwitchImage(0); // Hide ON_Red, Show OFF_Red
        }
    }

    private void SwitchImage(int index)
    {
        if (index >= 0 && index < onImages.Length)
        {
            onImages[index].gameObject.SetActive(false);
            offImages[index].gameObject.SetActive(true);
        }
    }
}
