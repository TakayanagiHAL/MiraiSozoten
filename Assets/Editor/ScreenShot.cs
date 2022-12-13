using UnityEditor;
using UnityEngine;
using System.IO;

namespace djkusuha.Utility
{
    /// <summary>
    /// Unityエディタ上からGameビューのスクリーンショットを撮るEditor拡張
    /// </summary>
    public class CaptureScreenshotFromEditor : Editor
    {
        /// <summary>
        /// キャプチャを撮る
        /// </summary>
        /// <remarks>
        /// Edit > CaptureScreenshot に追加。
        /// HotKeyは Ctrl + Alt + F12。
        /// </remarks>
        [MenuItem("Edit/CaptureScreenshot %&F12")]
        private static void CaptureScreenshot()
        {
            // 現在時刻からファイル名を決定
            var filename = System.DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".png";
            //var filepath = "Assets/Capture/" + filename;
            var filepath = "Capture/" + filename;

            // Captureフォルダが無かったらフォルダを作成
            if(!Directory.Exists("Capture"))
            {
                Directory.CreateDirectory("Capture");
                AssetDatabase.Refresh();
            }

            //if (AssetDatabase.IsValidFolder("Assets/Capture") == false)
            //{
            //    AssetDatabase.CreateFolder("Assets", "Capture");
            //}

            // キャプチャを撮る
#if UNITY_2017_1_OR_NEWER
            ScreenCapture.CaptureScreenshot(filepath); // ← GameViewにフォーカスがない場合、この時点では撮られない
#else
            Application.CaptureScreenshot(filepath); // ← GameViewにフォーカスがない場合、この時点では撮られない
#endif
            // GameViewを取得してくる
            var assembly = typeof(EditorWindow).Assembly;
            var type = assembly.GetType("UnityEditor.GameView");
            var gameview = EditorWindow.GetWindow(type);
            // GameViewを再描画
            gameview.Repaint();

            Debug.Log("ScreenShot: " + filename);
        }
    }
}