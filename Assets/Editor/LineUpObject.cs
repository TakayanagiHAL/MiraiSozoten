using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LineUpObject : EditorWindow
{
    private GameObject prefab;
    private int numX = 1;
    private int numZ = 1;
    private float intervalX = 1;
    private float intervalZ = 1;

    private float positionOffsetX = 0.0f;
    private float positionOffsetY = 0.0f;
    private float positionOffsetZ = 0.0f;

    // メニューに項目を追加
    [MenuItem("Window/Others/LineUpObject")]
    static void Init() // ウィンドウを作成
    {
        GetWindow<LineUpObject>(true, "LineUpObject");
    }

    // 表示するUI
    void OnGUI()
    {
        try
        {
            prefab = EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), true) as GameObject;


            GUILayout.Label("X : ", EditorStyles.boldLabel);
            numX = int.Parse(EditorGUILayout.TextField("num", numX.ToString()));
            intervalX = float.Parse(EditorGUILayout.TextField("interval", intervalX.ToString()));

            GUILayout.Label("Z : ", EditorStyles.boldLabel);
            numZ = int.Parse(EditorGUILayout.TextField("num", numZ.ToString()));
            intervalZ = float.Parse(EditorGUILayout.TextField("interval", intervalZ.ToString()));

            GUILayout.Label("PositionOffset : ", EditorStyles.boldLabel);
            positionOffsetX = float.Parse(EditorGUILayout.TextField("X", positionOffsetX.ToString()));
            positionOffsetY = float.Parse(EditorGUILayout.TextField("Y", positionOffsetX.ToString()));
            positionOffsetZ = float.Parse(EditorGUILayout.TextField("Z", positionOffsetZ.ToString()));


            GUILayout.Label("", EditorStyles.boldLabel);
            if (GUILayout.Button("Create")) Create();
        }
        catch (System.FormatException) { }
    }

    // 起動した時の処理
    void OnEnable()
    {
        if (Selection.gameObjects.Length > 0) prefab = Selection.gameObjects[0];
    }

    // prefabのオブジェクトを入れかえた時
    void OnSelectionChange()
    {
        if (Selection.gameObjects.Length > 0) prefab = Selection.gameObjects[0];
        Repaint();
    }

    // Createボタンを押した時に並べ替える
    private void Create()
    {
        if (prefab == null) return;   // prefabに何も入っていない場合はreutrn
        DeleteAllSerectPrefab();      // prefabと同じオブジェクトをヒエラルキーから探し出して全て削除する

        int count = 0;
        Vector3 pos;
        pos.y = positionOffsetY;

        pos.x = (-(numX - 1) * intervalX / 2) + positionOffsetX;
        for (int X = 0; X < numX; X++)
        {
            pos.z = (-(numZ - 1) * intervalZ / 2) + positionOffsetZ;
            for (int Z = 0; Z < numZ; Z++)
            {
                GameObject obj = PrefabUtility.InstantiatePrefab(prefab) as GameObject;

                /*上下をずらして並べる*/
                if (X % 2 == 0)
                {
                    obj.transform.position = new Vector3(pos.x, pos.y, pos.z);
                }
                else if (X % 2 == 1)
                {
                    obj.transform.position = new Vector3(pos.x, pos.y, pos.z - (intervalZ * 0.5f));
                }
                obj.name = prefab.name + count++;
                Undo.RegisterCreatedObjectUndo(obj, "LineUpObject");

                // objのインデックスコンポーネントにポジションを入力
                Hexagon hex = obj.GetComponent<Hexagon>();                
                var hsxSerialFeald = new SerializedObject(hex);
                hex.SetMapindex(X, Z);                

                hsxSerialFeald.ApplyModifiedProperties();
                pos.z -= intervalZ;
            }
            pos.x += intervalX;
        }
    }

    // 並び替えるオブジェクトと同じ種類のヒエラルキーのオブジェクトを全て削除する
    private void DeleteAllSerectPrefab()
    {
        // ヒエラルキーの全てのGameObjectを取得する
        Object[] allObject = Resources.FindObjectsOfTypeAll(typeof(GameObject));


        // 取得したオブジェクトの中から並び替えるオブジェクトと同じ物を全て削除する
        foreach (GameObject obj in allObject)
        {
            // 同じ物かどうかを判定
            if (obj.name.Contains(prefab.name))
            {
                Debug.Log(obj.name);
                DestroyImmediate(obj);
            }
        }
    }
}
