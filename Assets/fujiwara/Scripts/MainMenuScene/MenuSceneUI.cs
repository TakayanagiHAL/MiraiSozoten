using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSceneUI : MonoBehaviour
{
    [SerializeField] private GameObject Manager;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //「マルチプレイ」押下
    public void Press_MultiPlay()
    {
        //マルチプレイコマンドを表示
        transform.Find("MultiPlayGroup").gameObject.SetActive(true);
    }

    //「部屋を作成」押下
    public void Press_CreateRoom()
    {
        //接続開始（作成）
        Manager.GetComponent<F_StrixConnect>().Connect(true);
        
    }

    //「部屋を検索」押下
    public void Press_JoinRoom()
    {

        //マルチプレイコマンドを表示
        transform.Find("RoomIDInputGroup").gameObject.SetActive(true);
    }

    //検索ウィンドウで、「キャンセル」押下
    public void Press_MultiPlayCancell()
    {
        //プレイモードコマンドを非表示
        transform.Find("MultiPlayGroup").gameObject.SetActive(false);
    }

    //部屋番号入力後、「参加」押下
    public void Press_AccessRoom()
    {
        //接続開始（参加）
        Manager.GetComponent<F_StrixConnect>().Connect(false);

    }


    //部屋番号入力後、「参加」押下
    public void Press_RoomFoundCancel()
    {

        //マルチプレイコマンドを表示
        transform.Find("RoomIDInputGroup").gameObject.SetActive(false);
    }

    /// <summary>
    /// ルーム検索失敗時処理
    /// </summary>
    public void RoomAccessError()
    {
        transform.Find("RoomIDInputGroup").gameObject.SetActive(false);
        transform.Find("RoomAccessErrorGroup").gameObject.SetActive(true);

        GameObject.Find("MainMenuManager").GetComponent<PadControll_MenuScene>().StateChange_Back();
    }

    /// <summary>
    /// ルーム検索失敗ウィンドウで「戻る」を押したとき
    /// </summary>
    public void Press_ErrorBackButton()
    {
        transform.Find("RoomAccessErrorGroup").gameObject.SetActive(false);
        transform.Find("RoomIDInputGroup").gameObject.SetActive(true);
    }

    /// <summary>
    /// 検索ウィンドウからマルチプレイウィンドウに戻る
    /// </summary>
    public void Back_AccessToMulti()
    {
        transform.Find("RoomIDInputGroup").gameObject.SetActive(false);
    }

    /// <summary>
    /// マルチプレイウィンドウからモード選択ウィンドウに戻る
    /// </summary>
    public void Back_MultiToMode()
    {

        transform.Find("MultiPlayGroup").gameObject.SetActive(false);
    }

}
