using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGizmos : MonoBehaviour
{
    public Color GizColor = Color.blue;

    public float Radius = .2f;

    private void OnDrawGizmos()
    {
        Gizmos.color = GizColor;
        Gizmos.DrawSphere(transform.position, Radius);
    }
}
