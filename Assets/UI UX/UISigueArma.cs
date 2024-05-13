using UnityEngine;

public class UISigueArma : MonoBehaviour
{
    // Referencia al GameObject del arma
    public GameObject arma;

    // Vector3 que define el offset del elemento de UI respecto al arma
    public Vector3 offset = new Vector3(0, 0.5f, 0);

    void Update()
    {
        // Si el arma no existe, salimos del Update
        if (arma == null) return;

        // Calculamos la posición final del elemento de UI
        Vector3 posicionFinal = arma.transform.position + offset;

        // Actualizamos la posición del elemento de UI
        transform.position = posicionFinal;

        // Mantenemos la rotación del elemento de UI sin cambios
        transform.rotation = Quaternion.identity;
    }
}