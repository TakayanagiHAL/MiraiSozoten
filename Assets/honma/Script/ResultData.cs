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
    [Header("ストリクスOFF にチェックを入れる")]
    public bool LocalPlay;

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
    //[SerializeField]
    //GameObject gameScene;
    [SerializeField]
    [Header("Playerのscript")]
    private List<Player> _playerScript;
    //private Player _playerScript;

    [SerializeField]
    [Header("GameSceneのPlayer")]
    private List<GameObject> _playerObject;
    //private GameObject _playerObject;

    [SerializeField]
    [Header("GameSceneのPlayerParent")]
    private List<GameObject> _playerParentList;

    [SerializeField]
    [Header("GameSceneのGameSceneオブジェクト")]
    private GameObject _gameSceneObject;

    [SerializeField]
    [Header("GameSceneのCameraオブジェクト")]
    private GameObject _gameSceneCamera;

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
    private List<Image> _phase01PlayerIconList;

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
    [SerializeField]
    private Image _phase02PlayerIcon;

    [Header("Phase03")]
    [SerializeField]
    private Image _phase03CrownImage;
    [SerializeField]
    private Image _phase03NumberImage;
    [SerializeField]
    private Text _phase03PlayerName;
    [SerializeField]
    private Text _phase03OrderCount;
    [SerializeField]
    private Image _phase03PlayerIcon;

    [Header("Phase04")]
    [SerializeField]
    private List<Text> _phase04PlayerNameList;
    [SerializeField]
    private List<Text> _phase04OrderCountList;
    [SerializeField]
    private List<Text> _phase04MoneyCountList;
    [SerializeField]
    private List<Image> _phase04PlayerIconList;

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

    [Header("\nキャラクターの画像(Assetから選択)")]
    [SerializeField]
    private List<Sprite> PlayerIconList;

    Keyboard _keyboard;

    private Transform myTransform;
    private Vector3 StartPosition;
    Vector3 EndPosition;

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

        if (!LocalPlay)
        {
            //debug用のプレイヤーを代用
            for (int i = 0; i < _playerObject.Count; i++)
            {
                _playerObject[i] = _resultPlayer01[i];
            }
        }

        //各フェーズのアクティブ初期化
        _phase01ClosingEventCanvas.SetActive(false);

        //Phase02カメラの初期化
        foreach(var camera in _phase02CameraList)
        {
            camera.SetActive(false);
        }

    
        foreach(var ResultPlayer in _resultPlayer01)
        {
            ResultPlayer.transform.GetChild(0).gameObject.SetActive(false);
        }


        myTransform = _playersPosition.transform;
        StartPosition = myTransform.position;
        EndPosition = _phase01PlayersEndPosition.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!LocalPlay)
        {
            if (!isLocal) return;    
        }
        
        if (ResultStart == false) return;
        
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
        if (!LocalPlay)
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
                    return count - 1;
                }
            }
        }
        else
        {
            //  1位にの要素番号を返す
            count = MyPlayerRank(0);
            //補正値
            return count;
        }
        return count;
    }

    private int PlayerOrderCount(int number)//***************************************************************************************************     GameSceneから
    {
        List<int> _medalList = new List<int>();
        if (!OnlyResultPlay)
        {
            _medalList.Add(_playerScript[0].medal);//   5
            _medalList.Add(_playerScript[1].medal);//   3
            _medalList.Add(_playerScript[2].medal);//   11
            _medalList.Add(_playerScript[3].medal);//   6
        }
        else
        {
            _medalList.Add(5);
            _medalList.Add(3);
            _medalList.Add(11);
            _medalList.Add(6);
        }
        //降順にソート
        _medalList.Sort((a, b) => b - a);


        int orderCount = _medalList[number];

        //number位のメダルの数を返す
        return orderCount;
    }

    //順位取得
    private int MyPlayerRank(int number)//***************************************************************************************************     GameSceneから
    {
        int cnt = PlayerOrderCount(number);//number位のメダルの数

        List<int> _medalList = new List<int>(4); 
        
        if (!OnlyResultPlay)
        {
            _medalList.Add(_playerScript[0].medal);//   5
            _medalList.Add(_playerScript[1].medal);//   3
            _medalList.Add(_playerScript[2].medal);//   11
            _medalList.Add(_playerScript[3].medal);//   6
        }
        else
        {
            _medalList.Add(5);
            _medalList.Add(3);
            _medalList.Add(11);
            _medalList.Add(6);
        }

        int rank = _medalList.IndexOf(cnt);

        return rank;
    }

    private void RankImageChange()
    {
        if(!LocalPlay)
        {
            _phase02RankCrownImage.sprite = RankCrownImageLists[strixMyEntryNumber()];
            _phase02RankNumberImage.sprite = RankNumberImageLists[strixMyEntryNumber()];

            _phase03CrownImage.sprite = RankCrownImageLists[strixMyEntryNumber()];
            _phase03NumberImage.sprite = RankNumberImageLists[strixMyEntryNumber()];
        }
        else
        {
            _phase02RankCrownImage.sprite = RankCrownImageLists[0];
            _phase02RankNumberImage.sprite = RankNumberImageLists[0];

            _phase03CrownImage.sprite = RankCrownImageLists[0];
            _phase03NumberImage.sprite = RankNumberImageLists[0];
        }
       
    }

    private void PlayerIconChange()
    {

        for (int i = 0; i < _phase01PlayerIconList.Count; i++)
        {
            _phase01PlayerIconList[i].sprite = PlayerIconList[i];
        }

        _phase02PlayerIcon.sprite = PlayerIconList[strixMyEntryNumber()];
        _phase03PlayerIcon.sprite = PlayerIconList[strixMyEntryNumber()];

        for (int i = 0; i < _phase04PlayerIconList.Count; i++)
        {
            _phase04PlayerIconList[i].sprite = _phase01PlayerIconList[MyPlayerRank(i)].sprite;
        }

    }

    //自分の名前を取得する
    private string strixMyPlayerName(int number)
    {
        string _myPlayerName = "TestPlayer";// = "Testモード";

        if (!OnlyResultPlay)
        {
           _myPlayerName = _playerScript[number].playerName;// = "Testモード";

        }
        if (!LocalPlay)
        {
            _myPlayerName = StrixNetwork.instance.selfRoomMember.GetName();
        }
            return _myPlayerName;
    }

    //ResultシーンのPlayerNameを一括変更
    private void MyPlayerNameChange()
    {
        if (!OnlyResultPlay)
        {

            for (int i = 0; i < _phase01PlayerNameTextLists.Count; i++)
            {
                _phase01PlayerNameTextLists[i].text = _playerScript[i].playerName;
            }

                _phase02PlayerName.text = strixMyPlayerName(MyPlayerRank(0));
            _phase03PlayerName.text = strixMyPlayerName(MyPlayerRank(0));

            for (int i = 0; i < _playerScript.Count; i++)
            {
                _phase04PlayerNameList[i].text = _phase01PlayerNameTextLists[MyPlayerRank(i)].text;
            }

        }
        else
        {
            _phase01PlayerNameTextLists[0].text = "Player01";
            _phase01PlayerNameTextLists[1].text = "Player02";
            _phase01PlayerNameTextLists[2].text = "Player03";
            _phase01PlayerNameTextLists[3].text = "Player04";


            _phase02PlayerName.text = strixMyPlayerName(MyPlayerRank(0));
            _phase03PlayerName.text = strixMyPlayerName(MyPlayerRank(0));

            for (int i = 0; i < _phase04PlayerNameList.Count; i++)
            {
                _phase04PlayerNameList[i].text = _phase01PlayerNameTextLists[MyPlayerRank(i)].text;
            }
        }
    }



    private void OrderCountChange()
    {
        if (!OnlyResultPlay)
        {
            for (int i = 0; i < _phase04OrderCountList.Count; i++)
            {
                _phase01OrderCountTextLists[i].text = $"{_playerScript[i].medal}";
            }

            _phase02OrderCount.text = $"{PlayerOrderCount(0)}";
            _phase03OrderCount.text = $"{PlayerOrderCount(0)}";

            for(int i=0;i<_phase04OrderCountList.Count;i++)
            {
                _phase04OrderCountList[i].text = $"{PlayerOrderCount(i)}";
            }
        }
        else
        {
            _phase01OrderCountTextLists[0].text = $"5";
            _phase01OrderCountTextLists[1].text = $"3";
            _phase01OrderCountTextLists[2].text = $"11";
            _phase01OrderCountTextLists[3].text = $"6";

            _phase02OrderCount.text = $"{PlayerOrderCount(0)}";
            _phase03OrderCount.text = $"{PlayerOrderCount(0)}";

            for (int i = 0; i < _phase04OrderCountList.Count; i++)
            {
                _phase04OrderCountList[i].text = $"{PlayerOrderCount(i)}";
            }
        }
        

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
        for(int i=0;i<_phase04MoneyCountList.Count;i++)
        {
            _phase04MoneyCountList[i].text = $"{MyPlayerMoneyCount(MyPlayerRank(i))}";
        }
        
    }

    //所定の位置に置くためにサイズなどの初期化を行う
    private void PlayerGameObjectInitialization()
    {
        //if (!LocalPlay)
        //{
        //    //debug用のプレイヤーを代用
        //    for(int i=0;i<_playerObject.Count;i++)
        //    {
        //        _playerObject[i] = _resultPlayer01[i];
        //    }
        //}
       

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

        foreach(var parent in _playerParentList)
        {
            parent.transform.DetachChildren();
        }


        for(int i=0;i< _playerObject.Count;i++)
        {
            _playerObject[i].transform.parent = _resultPlayer01[i].transform;
        }
    }


    //最初の位置決め   一度だけ呼び出す
    private void StartStep()
    {
        if (ResultStart)
        {
            if (!OnlyResultPlay)
            {
                //リザルト用のカメラに切り替えるためにオフにする
                _gameSceneCamera.SetActive(false);
            }
        }


        if (!OnlyResultPlay)
        {
            PlayerParentDetachChildren();// 恐らくGameSceneObjectとResultSceneObjectの間にPlayerが出現する
            for (int i = 0; i < _resultPlayer01.Count; i++)
            {
                _resultPlayer01[i].SetActive(true);
            }
        }
        else
        {
            //debug用のPlayerを表示する
            for (int i = 0; i < _resultPlayer01.Count; i++)
            {
                _resultPlayer01[i].SetActive(true);
                _resultPlayer01[i].transform.GetChild(1).gameObject.SetActive(true);

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
        PlayerIconChange();
    }
    float speed_time;
    private void stepPhase01()
    {
        _phaseUiList[0].SetActive(true);//    animationでtrueにした

        //船の入場座標取得
        speed_time += Time.deltaTime;

        float _distance = StartPosition.z - EndPosition.z;
        float speed = speed_time * ShipsMovingSpeed / Mathf.Abs(_distance);


        //船の入場座標更新
        myTransform.position = Vector3.Lerp(StartPosition, EndPosition, speed);//代替案

        Debug.Log("StartPosition" + StartPosition);
        Debug.Log("EndPosition" + EndPosition);
        Debug.Log("_distance" + _distance);
        //Debug.Log("speed" + speed);
        Debug.Log("myTransform.position" + myTransform.position);

        if (speed>0.99f)//このspeedは割合    ここは割と適当な条件
        {
            StartCoroutine(WaitCoroutine(1, () =>// 1秒待つ
            {
                //_phase01ClosingEventCanvas.SetActive(true);
                _nowPhase = NowPhase.Phase02;
            }));

            if (_textControllerScript.GetTextEnd())//text読み上げ終了時
            {
                //_nowPhase = NowPhase.Phase02;
            }
        }
    }
    private void stepPhase02()
    {
        _phaseUiList[0].SetActive(false);
        //Cameraを追加
        _phase02CameraList[strixMyEntryNumber()].SetActive(true);
        //CameraFocus後にイベントで_phaseUiList[1].SetActive(true);を行う

       
        if (Input.GetKeyDown(KeyCode.Return)/* || Gamepad.current.bButton.wasPressedThisFrame*/)
        {
            _nowPhase = NowPhase.Phase03;
        }
    }
    private void stepPhase03()
    {
        _phaseUiList[1].SetActive(false);
        _phaseUiList[2].SetActive(true);

        //ボタン入力処理
        if (Input.GetKeyDown(KeyCode.Return)/* || Gamepad.current.bButton.wasPressedThisFrame*/)
        {
            phase03_ResultDetailsButton();
        }
    }
    private void stepPhase04()
    {
        _phaseUiList[2].SetActive(false);
        _phaseUiList[3].SetActive(true);

        bool test = false;
        //ボタン入力処理
        if ((Input.GetKeyDown(KeyCode.Return)/*Gamepad.current.bButton.wasPressedThisFrame)*/&&!test))
        {
            phase04_RoomExitButton();
            test = true;
        }

        if (test)
        {
            if (Input.GetKeyDown(KeyCode.Return)/* || Gamepad.current.aButton.wasPressedThisFrame*/)
            {
                phase04_Information_NO_Button();
                test = false;
            }
            
        }
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
        if (!LocalPlay)
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
