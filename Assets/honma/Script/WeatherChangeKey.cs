using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeatherChangeKey : MonoBehaviour
{

    public enum Weather
    {
        Clear,
        Rainy,
        Snowy
    }
    [Header("動画用の仮スクリプト")]

    [Header("キー入力 1:晴れ 2:雨 3:雪")]
    public Weather _weathertype;

    [SerializeField]
    [Header("天気のGameObjectを入れる")]
    private GameObject _Snowy;
    [SerializeField]
    private GameObject _Rainy;

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

        //天気の初期化
        _weathertype = Weather.Clear;

    }

    // Update is called once per frame
    void Update()
    {
        // 1キーの入力状態取得
        var Key_ten1 = _keyboard.digit1Key;
        // 1キーの入力状態取得
        var Key_ten2 = _keyboard.digit2Key;
        // 1キーの入力状態取得
        var Key_ten3 = _keyboard.digit3Key;

        if (Key_ten1.wasPressedThisFrame)
        {
            _weathertype = Weather.Clear;
        }
        if (Key_ten2.wasPressedThisFrame)
        {
            _weathertype = Weather.Rainy;
        }
        if (Key_ten3.wasPressedThisFrame)
        {
            _weathertype = Weather.Snowy;
        }

        switch (_weathertype)
        {
            case Weather.Clear:
                _Snowy.SetActive(false);
                _Rainy.SetActive(false);
                break;
            case Weather.Rainy:
                _Snowy.SetActive(false);
                _Rainy.SetActive(true);
                break;
            case Weather.Snowy:
                _Snowy.SetActive(true);
                _Rainy.SetActive(false);
                break;

        }
    }
}
