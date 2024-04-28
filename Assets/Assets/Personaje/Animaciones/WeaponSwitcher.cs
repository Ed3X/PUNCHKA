using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    private Animator animator;

    // Índice de las capas
    public int weapon1LayerIndex = 1;
    public int weapon2LayerIndex = 2;
    public int upperbodyLayerIndex = 3;
    public int upperbody1LayerIndex = 4;

    public GameObject weapon2; // GameObject del arma 2

    private void Start()
    {
        animator = GetComponent<Animator>();
        // Ocultar arma 2 al inicio
        weapon2.SetActive(false);
    }

    private void Update()
    {
        // Cambiar a los puños si se presiona "1"
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchToFists();
        }
        // Cambiar a "Weapon2" si se presiona "2"
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchToWeapon2();
        }
    }

    private void SwitchToFists()
    {
        // Desactivar la capa del arma 2 y upperbody1
        animator.SetLayerWeight(weapon2LayerIndex, 0);
        animator.SetLayerWeight(upperbody1LayerIndex, 0);

        // Activar la capa de los puños y upperbody
        animator.SetLayerWeight(weapon1LayerIndex, 1);
        animator.SetLayerWeight(upperbodyLayerIndex, 1);

        // Ocultar arma 2
        weapon2.SetActive(false);

        // Establecer WeaponType a 0 (puños)
        animator.SetInteger("WeaponType", 0);
    }

    private void SwitchToWeapon2()
    {
        // Desactivar la capa de los puños y upperbody
        animator.SetLayerWeight(weapon1LayerIndex, 0);
        animator.SetLayerWeight(upperbodyLayerIndex, 0);

        // Activar la capa del arma 2 y upperbody1
        animator.SetLayerWeight(weapon2LayerIndex, 1);
        animator.SetLayerWeight(upperbody1LayerIndex, 1);

        // Mostrar arma 2
        weapon2.SetActive(true);

        // Establecer WeaponType a 1 (weapon2)
        animator.SetInteger("WeaponType", 1);
    }
}
