using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFader : MonoBehaviour
{
    private Material[] MatsToChange;
    private Material[] MatsToRevert;

    public Material FadedMaterial;

    public bool DoFade = false;


    void Start()
    {
        MatsToChange = GetComponent<Renderer>().materials;   
        MatsToRevert = GetComponent<Renderer>().materials;
        
    }

    void Update()
    {
        if (DoFade)
        {
            FadeNow();
        }
        else
        {
            ResetFade();
        }
    }

    void FadeNow()
    {
        for (int i=0; i<MatsToChange.Length; i++)
        {
            MatsToChange[i] = FadedMaterial;
            
        }
        GetComponent<Renderer>().materials = MatsToChange;
    }
    void ResetFade()
    {
        for(int i=0; i<MatsToRevert.Length; i++)
        {
            MatsToChange[i] = MatsToRevert[i];
        }
        GetComponent <Renderer>().materials = MatsToRevert;
    }
}
