using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class FileNumDisplay : EditorWindow
{
    private const string REMOVE_STR = "Assets";

    private static readonly int mRemoveCount = REMOVE_STR.Length;
    private static Color mColor = new Color(0.0f, 0.635f, 0.635f, 1); // 文字背景色
    private static Color mTextColor = new Color(1.0f, 1.0f, 1.0f, 1); // 文字背景色

    private static bool isDisp = true;  // 表示/非表示のフラグ

    // メニューに項目を追加
    [MenuItem("Window/Others/FileNumDisplay")]
    static void Init() // ウィンドウを作成
    {
        GetWindow<FileNumDisplay>(true, "FileNumDisplay");
    }

    // 表示するUI
    void OnGUI()
    {
        try
        {
            // ON/OFFチェックボックス表示
            isDisp = EditorGUILayout.Toggle("ON/OFF", isDisp);

            // 文字背景色設定項目を追加
            mColor = EditorGUILayout.ColorField("BackColor", mColor);

            // 文字背景色設定項目を追加
            mTextColor = EditorGUILayout.ColorField("TextColor", mTextColor);
        }
        catch (System.FormatException) { }
    }

    // エディタ起動時・コンパイル時に呼び出す
    [InitializeOnLoadMethod]
    private static void Example()
    {
        EditorApplication.projectWindowItemOnGUI += OnGUI;
    }

    // ファイル数表示
    private static void OnGUI(string guid, Rect selectionRect)
    {
        if (isDisp) // 切り替えを作る (いつかきっと)
        {
            var dataPath = Application.dataPath;
            var startIndex = dataPath.LastIndexOf(REMOVE_STR);
            var dir = dataPath.Remove(startIndex, mRemoveCount);
            var path = dir + AssetDatabase.GUIDToAssetPath(guid);

            if (!Directory.Exists(path))
            {
                return;
            }

            var fileCount = Directory
                .GetFiles(path)
                .Where(c => !c.EndsWith(".meta"))
                .Count();

            var dirCount = Directory
                .GetDirectories(path)
                .Length;

            var count = fileCount + dirCount;

            if (count == 0)
            {
                return;
            }

            var text = count.ToString();
            var label = EditorStyles.label;
            var content = new GUIContent(text);
            var width = label.CalcSize(content).x + 1.0f;

            var pos = selectionRect;
            pos.x = pos.xMax - width;
            pos.width = width;
            pos.yMin++;

            // GUIStyleを複製 （こうしないと全体の設定が書き変わる）
            GUIStyle style = new GUIStyle(EditorStyles.label);
            style.normal.textColor = mTextColor;

            var color = GUI.color;
            GUI.color = mColor;
            GUI.DrawTexture(pos, EditorGUIUtility.whiteTexture);
            GUI.color = color;
            GUI.Label(pos, text, style);
        }
    }
}