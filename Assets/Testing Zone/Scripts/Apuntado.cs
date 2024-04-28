using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    void Update()
    {
        // Obtener la posición del ratón en el mundo
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, transform.position);
        float distance;
        if (plane.Raycast(ray, out distance))
        {
            Vector3 targetPoint = ray.GetPoint(distance);

            // Calcular la dirección hacia la que el jugador debe rotar
            Vector3 direction = targetPoint - transform.position;
            direction.y = 0f; // Mantener la rotación en el plano horizontal

            // Rotar el jugador hacia la dirección del ratón
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
            }
        }
    }
}
