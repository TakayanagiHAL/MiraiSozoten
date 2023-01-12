using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadControll_LobbyScene : MonoBehaviour
{

    //メニューの状態
    [SerializeField] private bool isExitWindow;

    [SerializeField] bool ExitWindowCursorState;

    // Start is called before the first frame update
    void Start()
    {
        isExitWindow = false;
        ExitWindowCursorState = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Enter()
    {
        //退室する
        if (isExitWindow&& ExitWindowCursorState)
        {
            GameObject.Find("LobbyManager").GetComponent<LobbyManager>().RoomExitAct();
        }

        //キャンセル
        else if(isExitWindow && !ExitWindowCursorState)
        {
            GameObject.Find("LobbyManager").GetComponent<LobbyManager>().RoomExitWindowClose();
        }
    }

    //戻るボタン押下
    public void Back()
    {
        isExitWindow = !isExitWindow;

        //ウィンドウ表示        
        if (isExitWindow)
        {
            //メインウィンドウからバック確認ウィンドウへ
            GameObject.Find("LobbyManager").GetComponent<LobbyManager>().RoomExitWindowPopUp();
        }
        else
        {
            GameObject.Find("LobbyManager").GetComponent<LobbyManager>().RoomExitWindowClose();
        }

        
    }

    public void PadControll_Left()
    {
        if(isExitWindow)
        {
            ExitWindowCursorState = !ExitWindowCursorState;
        }

        GameObject.Find("LobbyManager").GetComponent<LobbyManager>().ExitWindowCursorUpdate(ExitWindowCursorState);
    }

    public void PadControll_Right()
    {
        if (isExitWindow)
        {
            ExitWindowCursorState = !ExitWindowCursorState;
        }

        GameObject.Find("LobbyManager").GetComponent<LobbyManager>().ExitWindowCursorUpdate(ExitWindowCursorState);

    }
}