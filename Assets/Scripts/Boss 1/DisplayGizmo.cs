using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayGizmo : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        if (Application.isEditor)
        {
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
            Gizmos.DrawCube(Vector3.zero, Vector3.one * 1.5f);
        }
    }
}
