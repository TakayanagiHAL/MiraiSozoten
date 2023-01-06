using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PressButton : MonoBehaviour
{
    [SerializeField][Header("ClickLoadSceneのscriptを持ってくる")]
    private NextSceneLoad _nextSceneLoadScript;

    Keyboard _keyboard;

    // Start is called before the first frame update
    void Start()
    {
        // 現在のキーボード情報
        _keyboard = Keyboard.current;

        // キーボード接続チェック
        if (_keyboard == null)
        {
            // キーボードが接続されていないと
            // Keyboard.currentがnullになる
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Bキーの入力状態取得
        var Key_putB = _keyboard.bKey;

        if (Gamepad.current.bButton.wasPressedThisFrame||Input.GetKey(KeyCode.Return))
        {
            Debug.Log("Go");
            _nextSceneLoadScript.LoadSceneStart("Scenes/OpeningCreate");//シーン名入力
        }
    }
}
