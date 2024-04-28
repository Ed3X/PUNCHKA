using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrugBar : MonoBehaviour
{
    public Image barraDeDrogaDer;
    public Image barraDeDrogaIzq;

    public float drogaActual;
    public float drogaMaxima;

    public float decayRate = 10f;

    private GameObject playerObject;


    private void Update()
    {
        drogaActual = Mathf.Clamp(drogaActual - decayRate * Time.deltaTime, 0f, drogaMaxima);

        barraDeDrogaDer.fillAmount = drogaActual / drogaMaxima;
        barraDeDrogaIzq.fillAmount = drogaActual / drogaMaxima;

        if(drogaActual <= 0) 
        { 
            playerObject = GameObject.FindGameObjectWithTag("Player");
            playerObject.GetComponent<PlayerHealthSystem>().PlayerTakesDamage(10);
        }
    }
}
