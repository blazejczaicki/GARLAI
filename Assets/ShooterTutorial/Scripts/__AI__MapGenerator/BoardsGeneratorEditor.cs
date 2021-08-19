using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
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
#endif