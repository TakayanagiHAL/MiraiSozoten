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

    public bool ResultStart;

    [Header("=====GameSceneから入れる=====")]
    [SerializeField]
    [Header("Playerのscript")]
    private Player _playerScript;

    [SerializeField]
    [Header("GameSceneのPlayer")]
    private GameObject _playerObject;

    [Header("==============================\n")]

    [SerializeField]
    [Header("ClickLoadSceneのscript")]
    private NextSceneLoad _nextSceneLoadScript;

    [SerializeField] [Header("***リザルト状態になるまでfalse***\n")]
    private GameObject ResultSceneGameOblect;
    [SerializeField]
    private GameObject UiCanvas;

    [SerializeField] [Header("01～04までのフェーズを入れる")]
    private List<GameObject> _phaseUiList;

    //座標合わせ用
    [SerializeField] [Header("ResultPlayer01のGameObject")]
    private GameObject _resultPlayer01;

    //リザルト時の入場用座標
    [SerializeField] [Header("PlayersPositionのGameObject")]
    private GameObject _playersPosition;

    //Phase02以降に使う
    [SerializeField] [Header("shipsPosition2のGameObject")]
    private GameObject _shipsPosition2;

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
    private List<Text> _phase02OrderTextLists;
    [SerializeField]
    private GameObject _phase02PlayerPosition;

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
        if (!isLocal) return;

        if (ResultStart == false) return;

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
    //自分の順位取得
    private int MyPlayerRank()//***************************************************************************************************     GameSceneから
    {
        int rank = 1;

        return rank;
    }

    private void RankImageChange()
    {
        _phase02RankCrownImage.sprite = RankCrownImageLists[MyPlayerRank() - 1];
        _phase02RankNumberImage.sprite = RankNumberImageLists[MyPlayerRank() - 1];
        _phase03CrownImage.sprite = RankCrownImageLists[MyPlayerRank() - 1];
        _phase03NumberImage.sprite = RankNumberImageLists[MyPlayerRank() - 1];
    }

    //自分の名前を取得する
    private string strixMyPlayerName()
    {
        string _myPlayerName = StrixNetwork.instance.selfRoomMember.GetName();

        return _myPlayerName;
    }

    //ResultシーンのPlayerNameを一括変更
    private void MyPlayerNameChange()
    {
        _phase01PlayerNameTextLists[strixMyEntryNumber() - 1].text = strixMyPlayerName();
        _phase02PlayerName.text = strixMyPlayerName();
        _phase03PlayerName.text = strixMyPlayerName();
        _phase04PlayerNameList[MyPlayerRank()-1].text = strixMyPlayerName();
    }

    private int MyPlayerOrderCount()//***************************************************************************************************     GameSceneから
    {
        int myOrder = _playerScript.medal;//要確認
        return myOrder;
    }

    private void OrderCountChange()
    {
        _phase01OrderCountTextLists[strixMyEntryNumber() - 1].text = $"{MyPlayerOrderCount()}";
        _phase02OrderCount.text = $"{MyPlayerOrderCount()}";
        _phase03OrderCount.text = $"{MyPlayerOrderCount()}";
        _phase04OrderCountList[MyPlayerRank() - 1].text = $"{MyPlayerOrderCount()}";
    }

    private int MyPlayerMoneyCount()//***************************************************************************************************     GameSceneから
    {
        int myMoney = _playerScript.money;
        return myMoney;
    }

    private void MoneyCountChange()
    {
        _phase04MoneyCountList[MyPlayerRank() - 1].text = $"{MyPlayerMoneyCount()}";
    }

    //他のプレイヤーの名前、勲章の数、コインの数を取得していく
    private void OtherPlayers()
    {

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

    //Playerの階層をGameSceneから外す
    private void PlayerParentDetachChildren()
    {
        Transform _playerParent = _playerObject.transform.parent;//playerの一つ上のオブジェクトを取得
        _playerParent.DetachChildren();
    }

    //最初の位置決め   一度だけ呼び出す
    private void StartStep()
    {
        PlayerParentDetachChildren();
        UiCanvas.SetActive(true);
        PlayerGameObjectInitialization();
        RankImageChange();
        MyPlayerNameChange();
        OrderCountChange();
        MoneyCountChange();
    }

    private void stepPhase01()
    {
        _phaseUiList[0].SetActive(true);
        
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
        _shipsPosition2.SetActive(true);

        //船の座標取得
        Transform myTransform = _playersPosition.transform;
        Vector3 myVector3 = _phase02PlayerPosition.transform.position;
        //座標更新
        myTransform.position = myVector3;
        
        //プレイヤー情報
        

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

    public void phase03_ResultDetailsButton()
    {
        _nowPhase = NowPhase.Phase04;
    }

    public void phase04_RoomExitButton()
    {
        _phase04InformationUi.SetActive(true);
        //Information時に入力されないようにボタン無効化
        _exitButton.interactable = false;
        _stayButton.interactable = false;
    }

    public void phase04_RoomStayButton()
    {
        //ルームマッチングシーンに遷移
        //ルームマッチングシーンにレプリカのついたオブジェクトを持ち込まないように破棄する or 自分の情報だけを持ち込むようにする
        _nextSceneLoadScript.LoadSceneStart("ルームマッチング？Scene");//シーン名入力
    }

    public void phase04_Information_YES_Button()
    {
        //ルームから抜けて、メインメニューシーンに遷移
        _nextSceneLoadScript.LoadSceneStart("メインメニューScene");//シーン名入力
    }

    public void phase04_Information_NO_Button()
    {
        _phase04InformationUi.SetActive(false);
        //Information解除でphase04のボタン有効化
        _exitButton.interactable = true;
        _stayButton.interactable = true;
    }
}
