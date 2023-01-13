using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    //部屋の作成・検索モード
    private bool RoomMode;

    //ルームIDインプットフィールド
    public InputField RoomIDInputField;

    //ルームID保存変数
    public double RoomID;


    // Start is called before the first frame update
    void Start()
    {
        RoomID = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //入力フィールド更新
    public void InputRoomID()
    {
        string input = RoomIDInputField.text;

        //数字以外が入った場合終了
        int temp = 1;
        if (!int.TryParse(input, out temp))
        {
            return;
        }

        //文字列を値に変換
        int ans = int.Parse(input);
        RoomID = ans;
    }

    public void SceneChangetoGame()
    {
        SceneManager.LoadScene("OnlineLobbyScene");
    }

    

    


    //ルームIDを取得（ゲッタ）
    public double GetRoomID() { return RoomID; }
}
