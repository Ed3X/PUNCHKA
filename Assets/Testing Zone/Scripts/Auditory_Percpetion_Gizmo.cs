using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Auditory_Percpetion_Gizmo : MonoBehaviour
{
    public float auditionRadius = 10f;
    public int segments = 32;
    public Color gizmoColor = Color.green;

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;

        Vector3 center = transform.position;
        float angle = 0;
        float step = Mathf.PI * 2 / segments;

        for (int i = 0; i < segments; i++)
        {
            Vector3 prevPoint = new Vector3(Mathf.Cos(angle) * auditionRadius, 0, Mathf.Sin(angle) * auditionRadius);
            angle += step;
            Vector3 curPoint = new Vector3(Mathf.Cos(angle) * auditionRadius, 0, Mathf.Sin(angle) * auditionRadius);

            Gizmos.DrawLine(center + prevPoint, center + curPoint);
        }
    }
}
