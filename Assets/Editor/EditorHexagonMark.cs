using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorHexagonMark : EditorWindow
{
    [MenuItem("Window/Others/Hexagon")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(EditorHexagonMark));
    }

    private void OnGUI()
    {
        if (GUILayout.Button("SpriteSet"))
        {
            // ヒエラルキーの全てのGameObjectを取得する
            Hexagon[] allObject = Resources.FindObjectsOfTypeAll<Hexagon>();


            // 取得したオブジェクトの中から並び替えるオブジェクトと同じ物を全て削除する
            foreach (Hexagon obj in allObject)
            {
                Debug.Log(obj.name);
                obj.SetSprite();
                
            }
        }
    }
}
