using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

public class EditorHexagonMark : EditorWindow
{
    [MenuItem("Window/Hexagon")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(EditorHexagonMark));
    }

    private void OnGUI()
    {
    }
}

#endif