using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ShowNormalsComponent))]

public class ShowNormalsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();

        if (GUILayout.Button("Get Mesh"))
            ((ShowNormalsComponent)target).GetMesh();
    }
}
