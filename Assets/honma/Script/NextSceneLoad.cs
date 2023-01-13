using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class NextSceneLoad : MonoBehaviour
{
    [SerializeField][Header("ロード画面中に表示するUI")]
    private GameObject BlackBoardPanel;
    
    [SerializeField][Header("ロード画面中に表示するSlider")]
    private Slider _nowLodingSlider;

    [SerializeField][Header("テキスト演出")]
    private Text _loadingText;

    [SerializeField][Header("ピリオド描画カウンタ(大きいと遅くなる)")]
    private int _textCnt;

    [SerializeField]
    [Header("画像のPOP演出用")]
    private Image _imageTexture;

    [SerializeField][Header("画像のPOP高さ")]
    private int _imagePopHeight;

    private AsyncOperation _asyncOperation;

    private int _cnt = 0;

    private string _nextSceneName;
    private string _period2 = "";
    private string _period3 = "";

    private Vector3 _startPos;
    private Vector3 _endPos;

    public void LoadSceneStart(string SceneName)
    {
        _nextSceneName = SceneName;

        //ロード用のUI表示
        BlackBoardPanel.SetActive(true);

        //シーン読み込みを開始するコルーチン
        StartCoroutine(NowLoadScene());
    }

    IEnumerator NowLoadScene()
    {
        // シーンの読み込みをする  LoadSceneAsyncで現状の動作を問題なく行う（ここではロードしながらスライダーを動かしている）
        _asyncOperation = SceneManager.LoadSceneAsync(_nextSceneName);

        //　読み込みが終わるまで進捗状況をスライダーの値に反映させる
        while (!_asyncOperation.isDone)
        {
            //  _asyncOperation.progress でシーンの読み込み状況を取得する
            _nowLodingSlider.value = _asyncOperation.progress;

            //  ピリオドの2個目3個目の点滅用の計算
            if (_cnt % (_textCnt * 3) == 0) _period2 = "";
            if (_cnt % (_textCnt * 3) == 0) _period3 = "";

            if ((_cnt - _textCnt) % (_textCnt * 3) == 0) _period2 = ".";
            if (((_cnt - _textCnt * 2) % (_textCnt * 3)) == 0) _period3 = ".";

            _loadingText.text = $"Now Loding.{_period2}{_period3}";
            _cnt++;

            //  FlagのPOP UP演出
            Transform _transform = _imageTexture.transform;
            float _lerp = _nowLodingSlider.value * _nowLodingSlider.value * _nowLodingSlider.value;//    3次曲線上に動かす

            //座標更新
            _transform.position = Vector3.Lerp(_startPos, _endPos, _lerp);

            
            yield return null;
        }
    }

    private void Start()
    {
        //座標の初期化
        _startPos = _imageTexture.transform.position;
        _endPos = _imageTexture.transform.position;
        _endPos.y = _endPos.y + _imagePopHeight;

        BlackBoardPanel.SetActive(false);
    }

    //  確認用
    /*private void Update()
    {
        //  ピリオドの2個目3個目の点滅用の計算
        if (_cnt % (_textCnt * 3) == 0) _period2 = "";
        if (_cnt % (_textCnt * 3) == 0) _period3 = "";//

        if ((_cnt - _textCnt) % (_textCnt * 3) == 0) _period2 = ".";
        if (((_cnt - _textCnt * 2) % (_textCnt * 3)) == 0) _period3 = ".";

        _loadingText.text = $"Now Loding.{_period2}{_period3}";
        _cnt++;


        //  FlagのPOP UP演出
        Transform _transform = _imageTexture.transform;
        float _lerp = _nowLodingSlider.value * _nowLodingSlider.value * _nowLodingSlider.value;//    3次曲線上に動かす

        //座標更新
        _transform.position = Vector3.Lerp(_startPos, _endPos, _lerp);
    }*/
}
