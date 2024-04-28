using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider slider;
    public Color fillColor = Color.green;
    public Image barradeVidaDer;
    public Image barradeVidaIzq;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        // Update fill color based on gradient (assuming both images use the same gradient)
        barradeVidaDer.color = fillColor;
        barradeVidaIzq.color = fillColor;
    }

    public void SetHealth(int health)
    {
        slider.value = health;

        // Update fill amount and color based on slider value and gradient
        float normalizedValue = slider.value / slider.maxValue;
        barradeVidaDer.fillAmount = normalizedValue;
        barradeVidaIzq.fillAmount = normalizedValue;
    }
}