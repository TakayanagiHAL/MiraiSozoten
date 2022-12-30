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
        //プレイモードコマンドを非表示
        transform.Find("PlayModeGroup").gameObject.SetActive(false);

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
        //プレイモードコマンドを非表示
        transform.Find("MultiPlayGroup").gameObject.SetActive(false);

        //マルチプレイコマンドを表示
        transform.Find("RoomIDInputGroup").gameObject.SetActive(true);
    }

    //「キャンセル」押下
    public void Press_MultiPlayCancell()
    {
        //プレイモードコマンドを非表示
        transform.Find("MultiPlayGroup").gameObject.SetActive(false);

        //マルチプレイコマンドを表示
        transform.Find("PlayModeGroup").gameObject.SetActive(true);
    }

    //部屋番号入力後、「参加」押下
    public void Press_AccessRoom()
    {
        //接続開始（参加）
        Manager.GetComponent<F_StrixConnect>().Connect(false);

    }



}
