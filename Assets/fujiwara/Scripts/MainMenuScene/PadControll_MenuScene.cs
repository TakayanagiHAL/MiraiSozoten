using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PadControll_MenuScene : MonoBehaviour
{
    [SerializeField] private Sprite NormalCursor;
    [SerializeField] private Sprite SelectedCursor;

    [SerializeField] private GameObject[] Mode_Command=new GameObject[2];
    [SerializeField] private GameObject[] Multi_Command = new GameObject[2];
    [SerializeField] private GameObject[] Access_Command = new GameObject[2];
    [SerializeField] private GameObject Back_Command;

    private GameObject MenuCommandSystem;

    [SerializeField] private int[] SelectCursor = { -1,-1,-1};


    public enum ENUM_MenuState
    {
        MODE,
        MULTI,
        ACCESS,
        BACK,
    }


    //メニューの状態
    [SerializeField] private ENUM_MenuState MenuState;

    //最大コマンド数
    [SerializeField] private int CommandLimit;

    // Start is called before the first frame update
    void Start()
    {
        SelectCursor = new int[3];

        SelectCursor[0] = -1;
        SelectCursor[1] = -1;
        SelectCursor[2] = -1;
        MenuState = ENUM_MenuState.MODE;
        CommandLimit = 1;

        MenuCommandSystem = GameObject.Find("UI_Canvas");
    }

    void Update()
    {
        switch (MenuState)
        {
            case ENUM_MenuState.MODE:
                if(SelectCursor[0]==0)
                {
                    Mode_Command[0].GetComponent<Image>().sprite = SelectedCursor;
                    Mode_Command[1].GetComponent<Image>().sprite = NormalCursor;
                }
                else if(SelectCursor[0]==1)
                {
                    Mode_Command[0].GetComponent<Image>().sprite = NormalCursor;
                    Mode_Command[1].GetComponent<Image>().sprite = SelectedCursor;
                }
              
                break;

            case ENUM_MenuState.MULTI:
                if (SelectCursor[1] == 0)
                {
                    Multi_Command[0].GetComponent<Image>().sprite = SelectedCursor;
                    Multi_Command[1].GetComponent<Image>().sprite = NormalCursor;
                }
                else if (SelectCursor[1] == 1)
                {
                    Multi_Command[0].GetComponent<Image>().sprite = NormalCursor;
                    Multi_Command[1].GetComponent<Image>().sprite = SelectedCursor;
                }

                break;

            case ENUM_MenuState.ACCESS:
                if (SelectCursor[2] == 0)
                {
                    Access_Command[0].GetComponent<Image>().sprite = SelectedCursor;
                    Access_Command[1].GetComponent<Image>().sprite = NormalCursor;
                }
                else if (SelectCursor[2] == 1)
                {
                    Access_Command[0].GetComponent<Image>().sprite = NormalCursor;
                    Access_Command[1].GetComponent<Image>().sprite = SelectedCursor;
                }

                break;
        }

        
    }

    public void PadInput_Left()
    {
        if (MenuState == ENUM_MenuState.ACCESS)
        {
            CursorDecrement();
        }
    }

    public void PadInput_Right()
    {
        if (MenuState == ENUM_MenuState.ACCESS)
        {
            CursorIncrement();
        }
    }

    public void PadInput_Up()
    {
        if(MenuState==ENUM_MenuState.MODE|| MenuState == ENUM_MenuState.MULTI)
        {
            CursorDecrement();
        }
    }

    public void PadInput_Down()
    {
        if (MenuState == ENUM_MenuState.MODE || MenuState == ENUM_MenuState.MULTI)
        {
            CursorIncrement();
        }
    }

    //決定を押したとき
    public void PadInput_Enter()
    {
        Debug.Log("決定キーを押しました。");
        //ステートごとに処理が異なる
        switch (MenuState)
        {

            case ENUM_MenuState.MODE:
                Debug.Log("MODE時に決定");

                //「みんなで出航」選択
                if (SelectCursor[0]==0)
                {
                    MenuCommandSystem.GetComponent<MenuSceneUI>().Press_MultiPlay();
                    MenuState = ENUM_MenuState.MULTI;               
                }
                break;

            case ENUM_MenuState.MULTI:
                Debug.Log("MULTI時に決定");

                //「部屋を作成」選択
                if (SelectCursor[1] == 0)
                {
                    MenuCommandSystem.GetComponent<MenuSceneUI>().Press_CreateRoom();
                }

                //「部屋を検索」選択
                else if (SelectCursor[1] == 1)
                {
                    MenuCommandSystem.GetComponent<MenuSceneUI>().Press_JoinRoom();
                    MenuState = ENUM_MenuState.ACCESS;

                }
                break;

            case ENUM_MenuState.ACCESS:
                Debug.Log("ACCESS時に決定");

                //「ルームに参加」選択
                if (SelectCursor[2] == 0)
                {
                    //検索開始
                    MenuCommandSystem.GetComponent<MenuSceneUI>().Press_AccessRoom();
                }

                //「戻る」選択
                else if (SelectCursor[2] == 1)
                {
                    //マルチ選択に戻る
                    MenuCommandSystem.GetComponent<MenuSceneUI>().Press_RoomFoundCancel();
                    MenuState = ENUM_MenuState.MULTI;
                }
                break;

            case ENUM_MenuState.BACK:
                Debug.Log("BACK時に決定");
                MenuCommandSystem.GetComponent<MenuSceneUI>().Press_ErrorBackButton();
                MenuState = ENUM_MenuState.ACCESS;

                break;
        }
    }

    //戻るボタン選択
    public void PadInput_Back()
    {
        switch (MenuState)
        {
            case ENUM_MenuState.MULTI:
                MenuCommandSystem.GetComponent<MenuSceneUI>().Back_MultiToMode();

                MenuState = ENUM_MenuState.MODE;
                break;

            case ENUM_MenuState.ACCESS:
                MenuCommandSystem.GetComponent<MenuSceneUI>().Back_AccessToMulti();

                MenuState = ENUM_MenuState.MULTI;
                break;

            case ENUM_MenuState.BACK:
                MenuCommandSystem.GetComponent<MenuSceneUI>().Press_ErrorBackButton();

                MenuState = ENUM_MenuState.ACCESS;
                break;
        }
    }

    public void CursorIncrement()
    {
        switch (MenuState)
        {
            case ENUM_MenuState.MODE:
                SelectCursor[0]++;
                if (SelectCursor[0] > CommandLimit)
                {
                    SelectCursor[0] = 0;
                }
                break;

            case ENUM_MenuState.MULTI:
                SelectCursor[1]++;
                if (SelectCursor[1] > CommandLimit)
                {
                    SelectCursor[1] = 0;
                }
                break;

            case ENUM_MenuState.ACCESS:
                SelectCursor[2]++;
                if (SelectCursor[2] > CommandLimit)
                {
                    SelectCursor[2] = 0;
                }
                break;
        }
        

        
    }

    public void CursorDecrement()
    {
        switch (MenuState)
        {
            case ENUM_MenuState.MODE:
                SelectCursor[0]--;
                if (SelectCursor[0] < 0)
                {
                    SelectCursor[0] = CommandLimit;
                }
                break;

            case ENUM_MenuState.MULTI:
                SelectCursor[1]--;
                if (SelectCursor[1] < 0)
                {
                    SelectCursor[1] = CommandLimit;
                }
                break;

            case ENUM_MenuState.ACCESS:
                SelectCursor[2]--;
                if (SelectCursor[2] < 0)
                {
                    SelectCursor[2] = CommandLimit;
                }
                break;
        }

       
    }


    public void StateChange_Back()
    {
        MenuState = ENUM_MenuState.BACK;
    }
}

