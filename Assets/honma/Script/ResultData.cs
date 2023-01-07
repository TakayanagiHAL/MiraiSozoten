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
                    return count;
                }
            }
        }
        return count;
    }

    private int PlayerOrderCount(int number)//***************************************************************************************************     GameSceneから
    {
        List<int> _medalList = new List<int>();
        if (!OnlyResultPlay)
        {
            _medalList.Add(_playerScript[0].medal);
            _medalList.Add(_playerScript[1].medal);
            _medalList.Add(_playerScript[2].medal);
            _medalList.Add(_playerScript[3].medal);
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

        //number位のメダルの数を返す0~3
        return orderCount;
    }

    //順位取得
    private int MyPlayerRank(int number)//***************************************************************************************************     GameSceneから
    {
        //  PlayerOrderCount(3)   =   5
        number = PlayerOrderCount(number);//number位のメダルの数

        List<int> _medalList = new List<int>(4); 
        
        if (!OnlyResultPlay)
        {
            _medalList.Add(_playerScript[0].medal);
            _medalList.Add(_playerScript[1].medal);
            _medalList.Add(_playerScript[2].medal);
            _medalList.Add(_playerScript[3].medal);
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

        int rank = _medalList.IndexOf(number);
        
        //number位を入力するとその人が何位にいるか分かる

        return rank;

        //MyPlayerRank(2)は2を返す
        //MyPlayerRank(1)は1を返す

    }

    private void RankImageChange()
    {
        _phase02RankCrownImage.sprite = RankCrownImageLists[MyPlayerRank(0)];//1位
        _phase02RankNumberImage.sprite = RankNumberImageLists[MyPlayerRank(0)];
        _phase03CrownImage.sprite = RankCrownImageLists[MyPlayerRank(0)];
        _phase03NumberImage.sprite = RankNumberImageLists[MyPlayerRank(0)];
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
           
            _phase01PlayerNameTextLists[0].text = _playerScript[0].playerName;
            _phase01PlayerNameTextLists[1].text = _playerScript[1].playerName;
            _phase01PlayerNameTextLists[2].text = _playerScript[2].playerName;
            _phase01PlayerNameTextLists[3].text = _playerScript[3].playerName;


            _phase02PlayerName.text = strixMyPlayerName(MyPlayerRank(0));
            _phase03PlayerName.text = strixMyPlayerName(MyPlayerRank(0));

           
            _phase04PlayerNameList[0].text = _playerScript[MyPlayerRank(0)].playerName;
            _phase04PlayerNameList[1].text = _playerScript[MyPlayerRank(1)].playerName;
            _phase04PlayerNameList[2].text = _playerScript[MyPlayerRank(2)].playerName;
            _phase04PlayerNameList[3].text = _playerScript[MyPlayerRank(3)].playerName;

        }
        else
        {
            _phase01PlayerNameTextLists[0].text = "Player01";
            _phase01PlayerNameTextLists[1].text = "Player02";
            _phase01PlayerNameTextLists[2].text = "Player03";
            _phase01PlayerNameTextLists[3].text = "Player04";


            _phase02PlayerName.text = strixMyPlayerName(MyPlayerRank(0));
            _phase03PlayerName.text = strixMyPlayerName(MyPlayerRank(0));


            _phase04PlayerNameList[0].text = _phase01PlayerNameTextLists[MyPlayerRank(0)].text;
            _phase04PlayerNameList[1].text = _phase01PlayerNameTextLists[MyPlayerRank(1)].text;
            _phase04PlayerNameList[2].text = _phase01PlayerNameTextLists[MyPlayerRank(2)].text;
            _phase04PlayerNameList[3].text = _phase01PlayerNameTextLists[MyPlayerRank(3)].text;
        }
    }



    private void OrderCountChange()
    {
        // PlayerがListじゃない場合
        //_phase01OrderCountTextLists[strixMyEntryNumber() - 1].text = $"{MyPlayerOrderCount()}";
        //_phase02OrderCount.text = $"{MyPlayerOrderCount()}";
        //_phase03OrderCount.text = $"{MyPlayerOrderCount()}";
        //_phase04OrderCountList[MyPlayerRank() - 1].text = $"{MyPlayerOrderCount()}";

        if (!OnlyResultPlay)
        {
            _phase01OrderCountTextLists[0].text = $"{_playerScript[0].medal}";
            _phase01OrderCountTextLists[1].text = $"{_playerScript[1].medal}";
            _phase01OrderCountTextLists[2].text = $"{_playerScript[2].medal}";
            _phase01OrderCountTextLists[3].text = $"{_playerScript[3].medal}";

            _phase02OrderCount.text = $"{PlayerOrderCount(0)}";
            _phase03OrderCount.text = $"{PlayerOrderCount(0)}";

            _phase04OrderCountList[MyPlayerRank(0)].text = $"{PlayerOrderCount(0)}";
            _phase04OrderCountList[MyPlayerRank(1)].text = $"{PlayerOrderCount(1)}";
            _phase04OrderCountList[MyPlayerRank(2)].text = $"{PlayerOrderCount(2)}";
            _phase04OrderCountList[MyPlayerRank(3)].text = $"{PlayerOrderCount(3)}";
        }
        else
        {
            _phase01OrderCountTextLists[0].text = $"5";
            _phase01OrderCountTextLists[1].text = $"3";
            _phase01OrderCountTextLists[2].text = $"11";
            _phase01OrderCountTextLists[3].text = $"6";

            _phase02OrderCount.text = $"{PlayerOrderCount(0)}";
            _phase03OrderCount.text = $"{PlayerOrderCount(0)}";

            _phase04OrderCountList[MyPlayerRank(0)].text = $"{PlayerOrderCount(0)}";
            _phase04OrderCountList[MyPlayerRank(1)].text = $"{PlayerOrderCount(1)}";
            _phase04OrderCountList[MyPlayerRank(2)].text = $"{PlayerOrderCount(2)}";
            _phase04OrderCountList[MyPlayerRank(3)].text = $"{PlayerOrderCount(3)}";
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
        _phase04MoneyCountList[MyPlayerRank(0)].text = $"{MyPlayerMoneyCount(MyPlayerRank(0))}";
        _phase04MoneyCountList[MyPlayerRank(1)].text = $"{MyPlayerMoneyCount(MyPlayerRank(1))}";
        _phase04MoneyCountList[MyPlayerRank(2)].text = $"{MyPlayerMoneyCount(MyPlayerRank(2))}";
        _phase04MoneyCountList[MyPlayerRank(3)].text = $"{MyPlayerMoneyCount(MyPlayerRank(3))}";

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
    }

    private void stepPhase01()
    {
        _phaseUiList[0].SetActive(true);//    animationでtrueにした
        
        //船の入場座標取得
       
        float _distance = StartPosition.z - EndPosition.z;
        float speed = Time.deltaTime * ShipsMovingSpeed / Mathf.Abs(_distance);

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

       
        if (Input.GetKeyDown(KeyCode.Return) || Gamepad.current.bButton.wasPressedThisFrame)
        {
            _nowPhase = NowPhase.Phase03;
        }
    }
    private void stepPhase03()
    {
        _phaseUiList[1].SetActive(false);
        _phaseUiList[2].SetActive(true);

        //ボタン入力処理
        if (Input.GetKeyDown(KeyCode.Return) || Gamepad.current.bButton.wasPressedThisFrame)
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
        if ((Input.GetKeyDown(KeyCode.Return) || Gamepad.current.bButton.wasPressedThisFrame)&& !test)
        {
            phase04_RoomExitButton();
            test = true;
        }

        if (test)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Gamepad.current.aButton.wasPressedThisFrame)
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
