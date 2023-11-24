using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayGizmo : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        if (Application.isEditor)
            Gizmos.DrawSphere(transform.position, 0.25f);
    }
}
