using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoDrawer : MonoBehaviour
{
    // Il colore del gizmo
    public Color gizmoColor = Color.red;

    // La posizione del punto in cui disegnare il gizmo
    public Vector3 gizmoPosition = Vector3.zero;

    // La dimensione del gizmo
    public float gizmoSize = 0.5f;

    public GizmoDrawer(Color color, Vector3 position, float size)
    {
        gizmoColor = color;
        gizmoPosition = position;
        gizmoSize = size;
    }
}
