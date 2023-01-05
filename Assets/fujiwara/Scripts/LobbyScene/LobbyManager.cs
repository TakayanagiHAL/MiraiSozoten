using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SoftGear.Strix.Unity.Runtime;

public class LobbyManager : StrixBehaviour
{
    [SerializeField] private GameObject UI_RuleSettingCanvas;

    // Start is called before the first frame update
    void Start()
    {
        GameObject gameStartButton = GameObject.Find("PrivateUI_Canvas").gameObject.transform.Find("MatchConfirmButton").gameObject;

        //ホストにのみゲームスタートボタンを表示させる
        if(StrixNetwork.instance.isRoomOwner)
        {
            gameStartButton.SetActive(true);

            Instantiate(UI_RuleSettingCanvas);

        }
        else
        {
            gameStartButton.SetActive(false);
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Press_GameStart()
    {
        RpcToAll(nameof(SceneChangetoGame));
    }

    [StrixRpc]
    public void SceneChangetoGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}
