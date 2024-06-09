using UnityEngine;

public class BuildingHandler : MonoBehaviour
{
    [SerializeField] private float sphereCastRadius = 10f; // Ajusta el radio según sea necesario
    [SerializeField] private float sphereCastLength = 100f; // Ajusta la longitud según sea necesario

    private void Update()
    {
        Camera mainCamera = Camera.main;
        GameObject player = GameObject.FindGameObjectWithTag("BuildingPlayerHandler");

        if (mainCamera != null && player != null)
        {
            Vector3 start = mainCamera.transform.position;
            Vector3 direction = player.transform.position - start;
            RaycastHit[] hits = Physics.SphereCastAll(start, sphereCastRadius, direction, sphereCastLength);

            float distanceToPlayer = Vector3.Distance(start, player.transform.position);

            foreach (RaycastHit hit in hits)
            {
                GameObject hitObject = hit.collider.gameObject;

                // Calcular la distancia desde el punto de impacto hasta la cámara
                float distanceToCamera = Vector3.Distance(start, hit.point);

                // Solo afectar a los objetos que estén más cerca de la cámara que el jugador
                if (distanceToCamera < distanceToPlayer)
                {
                    // Verificar si el objeto impactado es el jugador
                    if (hitObject.CompareTag("BuildingPlayerHandler"))
                    {
                        continue;
                    }

                    ObjectFader fader = hitObject.GetComponent<ObjectFader>();

                    if (fader != null)
                    {
                        fader.DoFade = true;
                    }

                    // Visualizar el rayo
                    Debug.DrawLine(start, hit.point, Color.red);
                }
            }
        }
    }
}
