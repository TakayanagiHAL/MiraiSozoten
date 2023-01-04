using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using SoftGear.Strix.Unity.Runtime;
using SoftGear.Strix.Client.Core;

public class ResultData : StrixBehaviour
{
    public enum nowPhase
    {
        Phase01,
        Phase02,
        Phase03,
        Phase04
    }

    [SerializeField][Header("***リザルト状態になるまでfalse***\n")]
    private GameObject ResultSceneGameOblect;
    [SerializeField]
    private GameObject UiCanvas;

    [SerializeField][Header("01～04までのフェーズを入れる")]
    private List<GameObject> _phaseUiList;

    [SerializeField][Header("ボタンが押されると表示")]
    private GameObject _phase04InformationUi;

    [Header("***************************\n")]


    [SerializeField][Header("Playerのscript")]
    private Player _playerScript;

    [SerializeField][Header("GameSceneのPlayer")]
    private Player _playerObject;

    //座標合わせ用
    [SerializeField][Header("kari okiのゲームオブジェクト")]
    private GameObject _playerNo01;

    private GameObject _playerNo02;
    private GameObject _playerNo03;
    private GameObject _playerNo04;


    private int count = -1;
    Keyboard _keyboard;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //最初の位置決め
    private void StartPosition()
    {
        if (!isLocal) return;

        PlayerGameObjectInitialization();

    }

    private int MyEntryNumber()
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

    //所定の位置に置くためにサイズなどの初期化を行う
    private void PlayerGameObjectInitialization()
    {
        _playerObject.transform.localScale = _playerNo01.transform.localScale;
        _playerObject.transform.eulerAngles = _playerNo01.transform.eulerAngles;
        _playerObject.transform.position = _playerNo01.transform.position;

        if(MyEntryNumber()>1)
        {
            Transform myTransform = _playerNo01.transform;
            Vector3 myVector3 = myTransform.position;
            myVector3.x = myVector3.x - ((float)MyEntryNumber() - 1.0f);
            
            myTransform.position = myVector3;
        }
    }

    private void stepPhase01()
    {

    }
    private void stepPhase02()
    {

    }
    private void stepPhase03()
    {

    }
    private void stepPhase04()
    {

    }

}
