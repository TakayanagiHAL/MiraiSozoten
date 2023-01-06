using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SoftGear.Strix.Unity.Runtime;
using UnityEngine.UI;

public class LobbyManager : StrixBehaviour
{
    [SerializeField] private Sprite NormalCursor;
    [SerializeField] private Sprite SelectedCursor;

    bool isExit;

    [SerializeField] private GameObject UI_RuleSettingCanvas;
    [SerializeField] private GameObject UI_RoomExitBoard;

    GameObject gameStartButton;

    // Start is called before the first frame update
    void Start()
    {
        gameStartButton = GameObject.Find("PrivateUI_Canvas").gameObject.transform.Find("MatchConfirmButton").gameObject;

        //ホストにのみゲームスタートボタンを表示させる
        if (StrixNetwork.instance.isRoomOwner)
        {

            Instantiate(UI_RuleSettingCanvas);

        }
        gameStartButton.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (StrixNetwork.instance.isRoomOwner)
        {
            if (StrixNetwork.instance.room.GetMemberCount() > 1)
            {
                gameStartButton.SetActive(true);
            }
            else
            {
                gameStartButton.SetActive(false);

            }
        }
            
        
    }

    public void Press_GameStart()
    {
        RpcToAll(nameof(SceneChangetoGame));
    }

    [StrixRpc]
    public void SceneChangetoGame()
    {
        SceneManager.LoadScene(2);
    }

    public void RoomExitWindowPopUp()
    {
        UI_RoomExitBoard.SetActive(true);

        isExit = true;
    }

    public void RoomExitWindowClose()
    {
        UI_RoomExitBoard.SetActive(false);

        isExit = false;
    }

    //退室を行う
    public void RoomExitAct()
    {
        StrixNetwork.instance.LeaveRoom(handler: deleteRoomResult => Debug.Log("退室しました。："+ (StrixNetwork.instance.room == null)),
                            failureHandler: deleteRoomError => Debug.LogError("Could not delete room.Reason: " + deleteRoomError.cause));

        SceneManager.LoadScene("MainMenuScene");
    }

    public void ExitWindowCursorUpdate(bool state)
    {
        if(state)
        {
            UI_RoomExitBoard.transform.Find("YesButton").GetComponent<Image>().sprite = SelectedCursor;
            UI_RoomExitBoard.transform.Find("NoButton").GetComponent<Image>().sprite = NormalCursor;
        }
        else
        {
            UI_RoomExitBoard.transform.Find("YesButton").GetComponent<Image>().sprite = NormalCursor;
            UI_RoomExitBoard.transform.Find("NoButton").GetComponent<Image>().sprite = SelectedCursor;
        }
    }

}
