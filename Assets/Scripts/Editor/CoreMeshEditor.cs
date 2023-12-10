using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CoreMesh))]
public class CoreMeshEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();

        if (GUILayout.Button("Generate"))
            ((CoreMesh)target).Generate();
    }
}
