using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
[CustomEditor(typeof(BoardsGenerator))]
public class BoardsGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var tar = (BoardsGenerator)target;

        if (GUILayout.Button("Generate Board"))
        {
            tar.CreateSingleBoard();
        }

    }
}
