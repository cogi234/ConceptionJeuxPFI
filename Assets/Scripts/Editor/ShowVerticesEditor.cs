using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ShowVerticesComponent))]

public class ShowVerticesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();

        if (GUILayout.Button("Get Mesh"))
            ((ShowVerticesComponent)target).GetMesh();
    }
}
