using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using SoftGear.Strix.Unity.Runtime;
using SoftGear.Strix.Client.Core;

public class ResultData : StrixBehaviour
{
    public enum NowPhase
    {
        Start,
        Phase01,
        Phase02,
        Phase03,
        Phase04
    }
    private NowPhase _nowPhase;

    public bool ResultStart = false;


    [SerializeField]
    [Header("Playerのscript")]
    private Player _playerScript;

    [SerializeField]
    [Header("GameSceneのPlayer")]
    private GameObject _playerObject;

    [SerializeField][Header("***リザルト状態になるまでfalse***\n")]
    private GameObject ResultSceneGameOblect;
    [SerializeField]
    private GameObject UiCanvas;

    [SerializeField][Header("01～04までのフェーズを入れる")]
    private List<GameObject> _phaseUiList;

    [SerializeField][Header("ボタンが押されると表示")]
    private GameObject _phase04InformationUi;

    //座標合わせ用
    [SerializeField][Header("ResultPlayer01のGameObject")]
    private GameObject _resultPlayer01;

    //リザルト時の入場用座標
    [SerializeField][Header("PlayersPositionのGameObject")]
    private GameObject _playersPosition;

    //Phase02以降に使う
    [SerializeField][Header("shipsPosition2のGameObject")]
    private GameObject _shipsPosition2;

    [Header("***************************\n")]

    //各フェーズで徐々にアクティブを解除していく
    [Header("Phase01")]
    [SerializeField]
    private GameObject _phase01ClosingEventCanvas;//TextWindow
    [SerializeField]
    private GameObject _phase01PlayersEndPosition;
    [SerializeField][Header("Phase01の船の速度")]
    private float ShipsMovingSpeed;

    [Header("Phase02")]
    [SerializeField]
    private GameObject _phase02FullSizePanel;
    [SerializeField]
    private Image _rankCrownImage;
    [SerializeField]
    private Image _rankNumberImage;
    [SerializeField]
    private Text _phase02PlayerName;
    [SerializeField]
    private Text _phase02OrderCount;
    [SerializeField]
    private List<Text> _phase02OrderTextLists;
   

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

        ResultStart = false;
        _nowPhase = NowPhase.Start;

        //Gameシーン時に使わないオブジェクトをfalseにする
        ResultSceneGameOblect.SetActive(false);
        UiCanvas.SetActive(false);
        _phase04InformationUi.SetActive(false);
        foreach (var ListObject in _phaseUiList)
        {
            ListObject.SetActive(false);
        }
        _shipsPosition2.SetActive(false);

        //各フェーズのアクティブ初期化
        _phase01ClosingEventCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //if (ResultStart == false)
            //return;
        switch (_nowPhase)
        {
            case NowPhase.Start:
                ResultSceneGameOblect.SetActive(true);
                StartStep();
                _nowPhase = NowPhase.Phase01;
                break;
            case NowPhase.Phase01:
                stepPhase01();
                break;
            case NowPhase.Phase02:
                stepPhase02();
                break;
            case NowPhase.Phase03:
                stepPhase03();
                break;
            case NowPhase.Phase04:
                stepPhase04();
                break;
        }
        Debug.Log("なうステップ:" + _nowPhase.ToString());
    }

    //自分が何番目に入室したプレイヤーか検索する
    private int strixMyEntryNumber()
    {
        int count = 1;

        foreach (var RoomMember in StrixNetwork.instance.sortedRoomMembers)
        {
            if (StrixNetwork.instance.selfRoomMember.GetUid() != RoomMember.GetUid())
            {
                count++;
            }
            else//  selfRoomMember.GetUid() = RoomMember.GetUid()のとき
            {
                Debug.Log("あなたは" + count + "人目です");
                return count;
            }
        }
        return -1;
    }

    //自分の名前を取得する
    private string strixMyPlayerName()
    {
        string _myPlayerName = StrixNetwork.instance.selfRoomMember.GetName();

        return _myPlayerName;
    }

    //所定の位置に置くためにサイズなどの初期化を行う
    private void PlayerGameObjectInitialization()
    {
        _playerObject.transform.localScale = _resultPlayer01.transform.localScale;
        _playerObject.transform.eulerAngles = _resultPlayer01.transform.eulerAngles;
        _playerObject.transform.position = _resultPlayer01.transform.position;

        if(strixMyEntryNumber()>1)
        {
            Transform myTransform = _resultPlayer01.transform;
            Vector3 myVector3 = myTransform.position;
            myVector3.x = myVector3.x - ((float)strixMyEntryNumber() - 1.0f);
            
            myTransform.position = myVector3;
        }
    }

    //最初の位置決め   一度だけ呼び出す
    private void StartStep()
    {
        if (!isLocal) return;

        UiCanvas.SetActive(true);
        PlayerGameObjectInitialization();
    }

    private void stepPhase01()
    {
        _phaseUiList[0].SetActive(true);

        Transform myTransform = _playersPosition.transform;
        Vector3 StartPosition = myTransform.position;
        Vector3 EndPosition = _phase01PlayersEndPosition.transform.position;

        float _distance = StartPosition.z - EndPosition.z;
        float speed = Time.deltaTime * ShipsMovingSpeed / Mathf.Abs(_distance);//  40.0fはz座標の相対距離

        myTransform.position = Vector3.Lerp(StartPosition, EndPosition, speed);

        if(speed>0.99f)//このspeedは割合    ここは割と適当な条件
        {
            _phase01ClosingEventCanvas.SetActive(true);
            var key_B = _keyboard.bKey;

            if (key_B.wasPressedThisFrame)
            {
                _nowPhase = NowPhase.Phase02;
            }
        }
    }
    private void stepPhase02()
    {
        _phaseUiList[0].SetActive(false);
        _phaseUiList[1].SetActive(true);
    }
    private void stepPhase03()
    {

    }
    private void stepPhase04()
    {

    }

}
