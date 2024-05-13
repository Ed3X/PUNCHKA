using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchImages : MonoBehaviour
{
    public RawImage[] offImages;
    public RawImage[] onImages;
    public InputController inputController;
    public float effectDuration = 10f;
    public float speedPowerUp = 10f; // Velocidad modificada
    public float dashCooldownPowerUp = 0.5f; // Cooldown del dash modificado

    private bool isEffectActive = false;
    private bool isPowerUpActive = false; // Indica si el power up está activo

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SwitchImage(2); // Hide ON_Blue, Show OFF_Blue
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwitchImage(3); // Hide ON_Green, Show OFF_Green
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchImage(1); // Hide ON_Yellow, Show OFF_Yellow
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && !isEffectActive && !isPowerUpActive)
        {
            StartCoroutine(ActivateEffect());
        }
    }

    private void SwitchImage(int index)
    {
        if (index >= 0 && index < onImages.Length)
        {
            // Desactiva ON_Blue y activa OFF_Blue
            onImages[index].gameObject.SetActive(false);
            offImages[index].gameObject.SetActive(true);
        }
    }

    private IEnumerator ActivateEffect()
    {
        isEffectActive = true;
        isPowerUpActive = true;

        // Guarda los valores originales
        float originalSpeed = inputController.GetSpeed();
        float originalDashCooldown = inputController.GetDashCooldown();

        // Oculta ON_Blue y muestra OFF_Blue
        SwitchImage(0);

        // Aplica los efectos modificados
        inputController.SetSpeed(speedPowerUp);
        inputController.SetDashCooldown(dashCooldownPowerUp);

        // Espera el tiempo del efecto
        yield return new WaitForSeconds(effectDuration);

        // Restaura los valores originales
        inputController.SetSpeed(originalSpeed);
        inputController.SetDashCooldown(originalDashCooldown);

        // Muestra ON_Blue y oculta OFF_Blue
        SwitchImage(0);

        isEffectActive = false;
        isPowerUpActive = false;
    }
}
    