using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Events;
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

    [Header("リザルト開始時にGameSceneの方でTrueにする")]
    public bool ResultStart;

    [Header("【デバッグチェック】")]
    [Header("ストリクスOFF ＆ デバッグ時にチェックを入れる")]
    public bool LocalTestDebug;
    [Header("Gameシーン未接続状態のときにチェックを入れる(シーン遷移もなし)")]
    public bool OnlyResultPlay;

    [Header("【シーン名の入力】")]
    [Header("メインメニューシーン名を入力")]
    [SerializeField]
    private string MainMenuScene;
    [Header("ロビーマッチングシーン名を入力")]
    [SerializeField]
    private string LobbyMatchingScene;

    [Header("=====GameSceneから入れる=====")]
    [SerializeField]
    GameObject gameScene;
    [SerializeField]
    [Header("Playerのscript")]
    private List<Player> _playerScript;
    //private Player _playerScript;

    [SerializeField]
    [Header("GameSceneのPlayer")]
    private List<GameObject> _playerObject;
    //private GameObject _playerObject;

    [SerializeField]
    [Header("GameSceneのGameSceneオブジェクト")]
    private GameObject _gameSceneObject;

    [Header("==============================\n")]

    [SerializeField]
    [Header("Phase01ClosingEventCanvasのscript")]
    private TextController _textControllerScript;

    [SerializeField]
    [Header("ClickLoadSceneのscript")]
    private NextSceneLoad _nextSceneLoadScript;

    [SerializeField][Header("***リザルト状態になるまでfalse***\n")]
    private GameObject AnimationObject;

    [SerializeField]
    private GameObject ResultSceneGameOblect;
    [SerializeField]
    private GameObject UiCanvas;

    [SerializeField] [Header("01～04までのフェーズを入れる")]
    private List<GameObject> _phaseUiList;

    //座標合わせ用
    [SerializeField] [Header("ResultPlayer01のGameObject")]
    private List<GameObject> _resultPlayer01;
    //private GameObject _resultPlayer01;

    //リザルト時の入場用座標
    [SerializeField] [Header("PlayersPositionのGameObject")]
    private GameObject _playersPosition;

    [Header("***************************\n")]

    //各フェーズで徐々にアクティブを解除していく
    [Header("Phase01")]
    [SerializeField]
    private GameObject _phase01ClosingEventCanvas;//TextWindow
    [SerializeField]
    private GameObject _phase01PlayersEndPosition;
    [SerializeField] [Header("Phase01の船の速度")]
    private float ShipsMovingSpeed;
    [SerializeField]
    private List<Text> _phase01OrderCountTextLists;
    [SerializeField]
    private List<Text> _phase01PlayerNameTextLists;
    [SerializeField]
    private List<GameObject> _debugShipList;


    [Header("Phase02")]
    [SerializeField]
    private Image _phase02RankCrownImage;
    [SerializeField]
    private Image _phase02RankNumberImage;
    [SerializeField]
    private Text _phase02PlayerName;
    [SerializeField]
    private Text _phase02OrderCount;
    [SerializeField]
    private List<Text> _phase02OrderTextList;
    [SerializeField]
    private List<GameObject> _phase02CameraList;

    [Header("Phase03")]
    [SerializeField]
    private Image _phase03CrownImage;
    [SerializeField]
    private Image _phase03NumberImage;
    [SerializeField]
    private Text _phase03PlayerName;
    [SerializeField]
    private Text _phase03OrderCount;

    [Header("Phase04")]
    [SerializeField]
    private List<Text> _phase04PlayerNameList;
    [SerializeField]
    private List<Text> _phase04OrderCountList;
    [SerializeField]
    private List<Text> _phase04MoneyCountList;
    [SerializeField]
    [Header("ボタンが押されると表示")]
    private GameObject _phase04InformationUi;
    [SerializeField]
    private Button _stayButton;
    [SerializeField]
    private Button _exitButton;

    [Header("\n順位の画像(Assetから選択)")]
    [SerializeField]
    private List<Sprite> RankCrownImageLists;
    [SerializeField]
    private List<Sprite> RankNumberImageLists;

    Keyboard _keyboard;

    private List<int> _medalList;

     //Start is called before the first frame update
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
        AnimationObject.SetActive(false);
        ResultSceneGameOblect.SetActive(false);
        UiCanvas.SetActive(false);
        _phase04InformationUi.SetActive(false);
        foreach (var ListObject in _phaseUiList)
        {
            ListObject.SetActive(false);
        }

        //各フェーズのアクティブ初期化
        _phase01ClosingEventCanvas.SetActive(false);

        //Phase02カメラの初期化
        foreach(var camera in _phase02CameraList)
        {
            camera.SetActive(false);
        }

        //デバッグ用の船のオブジェクト
        foreach (var debugShip in _debugShipList)
        {
            debugShip.SetActive(false);
            debugShip.transform.GetChild(0).gameObject.SetActive(false);
        }

        foreach(var ResultPlayer in _resultPlayer01)
        {
            ResultPlayer.transform.GetChild(0).gameObject.SetActive(false);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!LocalTestDebug)
        {
            if (!isLocal) return;

            if (ResultStart == false) return;
        }

        switch (_nowPhase)
        {
            case NowPhase.Start:
                StartStep();
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
        int count = 3;
        if (!LocalTestDebug)
        {
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
        }
        return count;
    }
    //自分の順位取得
    private int MyPlayerRank(int number)//***************************************************************************************************     GameSceneから
    {
        _medalList[0] = _playerScript[0].medal;
        _medalList[1] = _playerScript[1].medal;
        _medalList[2] = _playerScript[2].medal;
        _medalList[3] = _playerScript[3].medal;

        //降順にソート
        _medalList.Sort((a, b) => b - a);


        int rank = _medalList.IndexOf(number-1);

        return rank;
    }

    private void RankImageChange()
    {
        _phase02RankCrownImage.sprite = RankCrownImageLists[MyPlayerRank(1) - 1];
        _phase02RankNumberImage.sprite = RankNumberImageLists[MyPlayerRank(1) - 1];
        _phase03CrownImage.sprite = RankCrownImageLists[MyPlayerRank(1) - 1];
        _phase03NumberImage.sprite = RankNumberImageLists[MyPlayerRank(1) - 1];
    }

    //自分の名前を取得する
    private string strixMyPlayerName(int rank)
    {
        string _myPlayerName = _playerScript[rank].playerName;// = "Testモード";

        if (!LocalTestDebug)
        {
            _myPlayerName = StrixNetwork.instance.selfRoomMember.GetName();
        }
            return _myPlayerName;
    }

    //ResultシーンのPlayerNameを一括変更
    private void MyPlayerNameChange()
    {
        //_phase01PlayerNameTextLists[strixMyEntryNumber() - 1].text = strixMyPlayerName();
        _phase01PlayerNameTextLists[MyPlayerRank(1)].text = _playerScript[MyPlayerRank(1)].playerName;//
        _phase01PlayerNameTextLists[MyPlayerRank(2)].text = _playerScript[MyPlayerRank(2)].playerName;
        _phase01PlayerNameTextLists[MyPlayerRank(3)].text = _playerScript[MyPlayerRank(3)].playerName;
        _phase01PlayerNameTextLists[MyPlayerRank(4)].text = _playerScript[MyPlayerRank(4)].playerName;


        _phase02PlayerName.text = strixMyPlayerName(1);
        _phase03PlayerName.text = strixMyPlayerName(1);

        //_phase04PlayerNameList[MyPlayerRank() - 1].text = strixMyPlayerName();
        _phase04PlayerNameList[MyPlayerRank(1)].text = _playerScript[MyPlayerRank(1)].playerName;
        _phase04PlayerNameList[MyPlayerRank(2)].text = _playerScript[MyPlayerRank(2)].playerName;
        _phase04PlayerNameList[MyPlayerRank(3)].text = _playerScript[MyPlayerRank(3)].playerName;
        _phase04PlayerNameList[MyPlayerRank(4)].text = _playerScript[MyPlayerRank(4)].playerName;


    }

    private int MyPlayerOrderCount(int number)//***************************************************************************************************     GameSceneから
    {
        _medalList[0] = _playerScript[0].medal;
        _medalList[1] = _playerScript[1].medal;
        _medalList[2] = _playerScript[2].medal;
        _medalList[3] = _playerScript[3].medal;

        //降順にソート
        _medalList.Sort((a, b) => b - a);


        int rank = _medalList[number - 1];

        return rank;
    }

    private void OrderCountChange()
    {
        // PlayerがListじゃない場合
        //_phase01OrderCountTextLists[strixMyEntryNumber() - 1].text = $"{MyPlayerOrderCount()}";
        //_phase02OrderCount.text = $"{MyPlayerOrderCount()}";
        //_phase03OrderCount.text = $"{MyPlayerOrderCount()}";
        //_phase04OrderCountList[MyPlayerRank() - 1].text = $"{MyPlayerOrderCount()}";

        _phase01OrderCountTextLists[0].text = $"{MyPlayerOrderCount(0)}";
        _phase01OrderCountTextLists[1].text = $"{MyPlayerOrderCount(1)}";
        _phase01OrderCountTextLists[2].text = $"{MyPlayerOrderCount(2)}";
        _phase01OrderCountTextLists[3].text = $"{MyPlayerOrderCount(3)}";

        _phase02OrderCount.text = $"{MyPlayerOrderCount(0)}";
        _phase03OrderCount.text = $"{MyPlayerOrderCount(0)}";

        _phase04OrderCountList[MyPlayerRank(0)].text = $"{MyPlayerOrderCount(0)}";
        _phase04OrderCountList[MyPlayerRank(1)].text = $"{MyPlayerOrderCount(1)}";
        _phase04OrderCountList[MyPlayerRank(2)].text = $"{MyPlayerOrderCount(2)}";
        _phase04OrderCountList[MyPlayerRank(3)].text = $"{MyPlayerOrderCount(3)}";

    }

    private int MyPlayerMoneyCount(int number)//***************************************************************************************************     GameSceneから
    {
        int myMoney = 1234;
        
        if (!OnlyResultPlay)
        {
            myMoney = _playerScript[number].money;
        }
        return myMoney;
    }

    private void MoneyCountChange()
    {
        _phase04MoneyCountList[MyPlayerRank(1)].text = $"{MyPlayerMoneyCount(MyPlayerRank(1))}";

        _phase04MoneyCountList[MyPlayerRank(2)].text = $"{MyPlayerMoneyCount(2)}";
        _phase04MoneyCountList[MyPlayerRank(3)].text = $"{MyPlayerMoneyCount(3)}";
        _phase04MoneyCountList[MyPlayerRank(4)].text = $"{MyPlayerMoneyCount(4)}";

    }

    //他のプレイヤーの名前、勲章の数、コインの数を取得していく
    private void OtherPlayers()
    {
        int rank = _playerScript[0].medal;
    }

    //所定の位置に置くためにサイズなどの初期化を行う
    private void PlayerGameObjectInitialization()
    {
        if (!LocalTestDebug)
        {
            //debug用のプレイヤーを代用
        }
       

       
        for(int i=0;i<_playerObject.Count;i++)
        {
            _playerObject[i].transform.localScale = _resultPlayer01[i].transform.localScale;
            _playerObject[i].transform.eulerAngles = _resultPlayer01[i].transform.eulerAngles;
            _playerObject[i].transform.position = _resultPlayer01[i].transform.position;

            //Transform myTrans = _playerObject[i].transform;
            //Vector3 myVector3 = myTrans.position;
            //myVector3.x = myVector3.x - (i * (-1.0f));
            //myTrans.position = myVector3;
        }

        //if(strixMyEntryNumber()>1)
        //{
        //    Transform myTransform = _resultPlayer01.transform;
        //    Vector3 myVector3 = myTransform.position;
        //    myVector3.x = myVector3.x - ((float)strixMyEntryNumber() - 1.0f);
            
        //    myTransform.position = myVector3;
        //}
    }

    //Playerの階層をGameSceneから外す
    private void PlayerParentDetachChildren()
    {
        //Transform _playerParent = _playerObject.transform.parent;


        for(int i=0;i<_playerObject.Count;i++)
        {
            GameObject gameObjectPaerent = _playerObject[i].transform.parent.gameObject;//playerの一つ上のオブジェクトを取得
            gameObjectPaerent.transform.DetachChildren();
        }
    }


    //最初の位置決め   一度だけ呼び出す
    private void StartStep()
    {
        if (!LocalTestDebug)
        {
            
        }

        if (!OnlyResultPlay)
        {
            PlayerParentDetachChildren();// 恐らくGameSceneObjectとResultSceneObjectの間にPlayerが出現する
            
        }
        else
        {
            foreach (var debugShip in _debugShipList)
            {
                debugShip.SetActive(true);
                debugShip.transform.GetChild(0).gameObject.SetActive(true);
            }
            
            for (int i = 0; i < _resultPlayer01.Count; i++)
            {
               _resultPlayer01[i].transform.GetChild(0).gameObject.SetActive(true);

            }
        }

        AnimationObject.SetActive(true);
        UiCanvas.SetActive(true);
        gameSceneFalse();//この関数でGameシーンをオフにする  **********************************************************************************************:
        ResultSceneGameOblect.SetActive(true);
        PlayerGameObjectInitialization();
        RankImageChange();
        MyPlayerNameChange();
        OrderCountChange();
        MoneyCountChange();
    }

    private void stepPhase01()
    {
        _phaseUiList[0].SetActive(true);//    animationでtrueにした
        
        //船の入場座標取得
        Transform myTransform = _playersPosition.transform;
        Vector3 StartPosition = myTransform.position;
        Vector3 EndPosition = _phase01PlayersEndPosition.transform.position;

        float _distance = StartPosition.z - EndPosition.z;
        float speed = Time.deltaTime * ShipsMovingSpeed / Mathf.Abs(_distance);

        //船の入場座標更新
        myTransform.position = Vector3.Lerp(StartPosition, EndPosition, speed);

        if(speed>0.99f)//このspeedは割合    ここは割と適当な条件
        {
            StartCoroutine(WaitCoroutine(1, () =>// 1秒待つ
            {
                _phase01ClosingEventCanvas.SetActive(true);
            }));

            if (_textControllerScript.GetTextEnd())//text読み上げ終了時
            {
                _nowPhase = NowPhase.Phase02;
            }
        }
    }
    private void stepPhase02()
    {
        _phaseUiList[0].SetActive(false);
        //Cameraを追加
        _phase02CameraList[strixMyEntryNumber() - 1].SetActive(true);
        //CameraFocus後にイベントで_phaseUiList[1].SetActive(true);を行う

        var key_B = _keyboard.bKey;

        if (key_B.wasPressedThisFrame)
        {
            _nowPhase = NowPhase.Phase03;
        }
    }
    private void stepPhase03()
    {
        _phaseUiList[1].SetActive(false);
        _phaseUiList[2].SetActive(true);

        //ボタン入力処理
    }
    private void stepPhase04()
    {
        _phaseUiList[2].SetActive(false);
        _phaseUiList[3].SetActive(true);

        //ボタン入力処理
    }


    //*****     Button   *****
    public void phase03_ResultDetailsButton()
    {
        _nowPhase = NowPhase.Phase04;
    }

    public void phase04_RoomExitButton()
    {
        _phase04InformationUi.SetActive(true);
        //Information時に入力されないようにボタン無効化
        _exitButton.enabled = false;
        _stayButton.enabled = false;
    }

    public void phase04_RoomStayButton()
    {
        if (!OnlyResultPlay)
        {
            //ルームマッチングシーンに遷移
            //ルームマッチングシーンにレプリカのついたオブジェクトを持ち込まないように破棄する or 自分の情報だけを持ち込むようにする
            _nextSceneLoadScript.LoadSceneStart(LobbyMatchingScene);//シーン名入力
        }
    }

    public void phase04_Information_YES_Button()
    {
        if (!LocalTestDebug)
        {
            //ルームから抜けて、メインメニューシーンに遷移
            StrixNetwork.instance.LeaveRoom(handler: deleteRoomResult => Debug.Log("退室しました。：" + (StrixNetwork.instance.room == null)),
                            failureHandler: deleteRoomError => Debug.LogError("Could not delete room.Reason: " + deleteRoomError.cause));
        }
        if(!OnlyResultPlay)
        {
            _nextSceneLoadScript.LoadSceneStart(MainMenuScene);
        }
        
    }

    public void phase04_Information_NO_Button()
    {
        _phase04InformationUi.SetActive(false);
        //Information解除でphase04のボタン有効化
        _exitButton.enabled = true;
        _stayButton.enabled = true;
    }


    //*****     アニメーション     *****
    public void phase01Start()
    {
        _nowPhase = NowPhase.Phase01;
    }

    //アニメーションのイベントで丁度良いタイミングでGameシーンをオフにする
    public void gameSceneFalse()
    {
        if (!OnlyResultPlay)
        {
            _gameSceneObject.SetActive(false);
        }   
    }

    //Cameraズーム後にPhase02を進行させる
    public void phase02CameraFocusEnd()
    {
        _phaseUiList[1].SetActive(true);
    }

    //*****     コルーチン       *****
    private IEnumerator WaitCoroutine(float waitSeconds, UnityAction callback)
    {
        yield return new WaitForSeconds(waitSeconds);
        callback?.Invoke();
    }

}
