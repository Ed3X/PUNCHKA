using UnityEngine;

public class UILineaArma : MonoBehaviour
{
    // Referencia al GameObject del arma
    public GameObject arma;

    // Referencia al GameObject del elemento de UI
    public GameObject elementoUI;

    // Puntos de origen y final de la línea
    public Vector3 origen = new Vector3(0, 0.5f, 0);
    public Vector3 final = new Vector3(0, 0, 0);

    // Offsets para ajustar los puntos de origen y final
    public Vector3 offsetOrigen = Vector3.zero;
    public Vector3 offsetFinal = Vector3.zero;

    // Referencia al componente Line Renderer
    private LineRenderer lineRenderer;

    void Start()
    {
        // Creamos el componente Line Renderer si no existe
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        // Configuramos las propiedades del Line Renderer
        lineRenderer.startWidth = 0.02f;
        lineRenderer.endWidth = 0.02f;
        lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;
    }

    void Update()
    {
        // Si el arma o el elemento de UI no existen, salimos del Update
        if (arma == null || elementoUI == null) return;

        // Calculamos la posición final del punto de origen
        Vector3 posicionOrigenFinal = arma.transform.position + origen + offsetOrigen;

        // Calculamos la posición final del punto final
        Vector3 posicionFinalFinal = elementoUI.transform.position + final + offsetFinal;

        // Establecemos los puntos del Line Renderer
        lineRenderer.SetPosition(0, posicionOrigenFinal);
        lineRenderer.SetPosition(1, posicionFinalFinal);
    }
}
