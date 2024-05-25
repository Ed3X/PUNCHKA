using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SwitchImages : MonoBehaviour
{
    public RawImage[] offImages;
    public RawImage[] onImages;
    public InputController inputController;
    public CombatController combatController; // Referencia al CombatController
    public PlayerHealthSystem playerHealthSystem; // Referencia al PlayerHealthSystem
    public PlayerCombat playerCombat; // Referencia al PlayerCombat
    public float effectDuration = 10f;
    public float speedPowerUp = 10f; // Velocidad modificada
    public float dashCooldownPowerUp = 0.5f; // Cooldown del dash modificado
    public int damagePowerUp = 30; // Daño aumentado durante el power-up de daño

    private bool isEffectActive = false;
    private bool isPowerUpActive = false; // Indica si el power-up está activo

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            StartCoroutine(ActivateHealingEffect());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchImage(3); // Hide ON_Green, Show OFF_Green
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5) && !isEffectActive && !isPowerUpActive)
        {
            StartCoroutine(ActivateDamagePowerUp());
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
            onImages[index].gameObject.SetActive(false);
            offImages[index].gameObject.SetActive(true);
        }
    }

    private IEnumerator ActivateEffect()
    {
        isEffectActive = true;
        isPowerUpActive = true;

        float originalSpeed = inputController.GetSpeed();
        float originalDashCooldown = inputController.GetDashCooldown();

        SwitchImage(0);

        inputController.SetSpeed(speedPowerUp);
        inputController.SetDashCooldown(dashCooldownPowerUp);

        combatController.SetPowerUpActive(true);

        yield return new WaitForSeconds(effectDuration);

        inputController.SetSpeed(originalSpeed);
        inputController.SetDashCooldown(originalDashCooldown);

        combatController.SetPowerUpActive(false);

        SwitchImage(0);

        isEffectActive = false;
        isPowerUpActive = false;
    }

    private IEnumerator ActivateDamagePowerUp()
    {
        isEffectActive = true;
        isPowerUpActive = true;

        int originalDamage = playerCombat.damage;

        SwitchImage(1); // Cambiar imagen correspondiente

        playerCombat.SetDamage(damagePowerUp);

        yield return new WaitForSeconds(effectDuration);

        playerCombat.ResetDamage();

        SwitchImage(1); // Cambiar imagen correspondiente

        isEffectActive = false;
        isPowerUpActive = false;
    }

    private IEnumerator ActivateHealingEffect()
    {
        if (!isEffectActive && !isPowerUpActive)
        {
            isEffectActive = true;
            isPowerUpActive = true;

            SwitchImage(2); // Hide ON_Blue, Show OFF_Blue

            playerHealthSystem.HealToMax();

            yield return new WaitForSeconds(effectDuration);

            SwitchImage(2);

            isEffectActive = false;
            isPowerUpActive = false;
        }
        else
        {
            // Curación inmediata sin activar el cooldown
            playerHealthSystem.HealToMax();
        }
    }
}
