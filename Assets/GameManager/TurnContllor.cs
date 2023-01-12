using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SoftGear.Strix.Unity.Runtime;

public enum TurnState
{
    TURN_WAIT,
    SELECT_COMAND,
    PLAYER_MOVE,
    HAPPNING_EVENT,
    MAP_VIEW
}

public class TurnContllor : StrixBehaviour
{

    HexagonManger hexagonManger;

    UIManager uiManager;

    OnotherUI onotherUI;

    [SerializeField] ResultData result;

    public Player[] players;
    [StrixSyncField]
    int playerVol;

    [SerializeField] int maxTurn;

    [StrixSyncField]
    public int nowTurn =1;
    [StrixSyncField]
    public int turnPlayer =0;

    RenderTexture[] renderTextures;

    int thisPlayer = 0;

    // Start is called before the first frame update
    void Start()
    {
        hexagonManger = FindObjectOfType<HexagonManger>();
        turnPlayer = 0;

        onotherUI = FindObjectOfType<OnotherUI>();

        Player[] p = FindObjectsOfType<Player>();
        players = new Player[p.Length];
        playerVol = players.Length;

        renderTextures = new RenderTexture[4];


        for (int i = 0; i < players.Length; i++)
        {
            renderTextures[i] = new RenderTexture(1920, 1080, 16);
            players[i] = p[i];
            players[i].turnNum = i;

            players[i].CallRPCOwner(Player.RpcFunctionName.INIT_PLAYER);

            players[i].playerCamera.SetRenderTexture(renderTextures[i]);

            players[i].uiManager.SetCamera(players[i].playerCamera.GetCamera());

            players[i].playerCamera.SetMapCamera(true);
            players[i].uiManager.SetCanvas(CanvasName.TURN_START_UI, true);
            players[i].uiManager.GetCanvas(CanvasName.TURN_START_UI).GetComponent<TurnStartUI>().SetTurn(nowTurn);
            players[i].uiManager.GetCanvas(CanvasName.TURN_START_UI).GetComponent<TurnStartUI>().AnimStart();
        }


        onotherUI.SetTurnTexture(renderTextures[0]);

       
    }

    // Update is called once per frame
    void Update()
    {
        onotherUI.SetTurnTexture(renderTextures[turnPlayer]);
    }

    [StrixRpc]
    public void SetNextTurnPlayer()
    {

            turnPlayer++;
        
        if (turnPlayer % playerVol == 0)
        {
  
                turnPlayer = 0;
                nowTurn++;
            
            if (nowTurn > maxTurn)
            {
                result.ResultStart = true;
            }
            else
            {
                players[0].playerCamera.SetMapCamera(true);
                players[0].uiManager.SetCanvas(CanvasName.TURN_START_UI, true);
                players[0].uiManager.GetCanvas(CanvasName.TURN_START_UI).GetComponent<TurnStartUI>().SetTurn(nowTurn);
                players[0].uiManager.GetCanvas(CanvasName.TURN_START_UI).GetComponent<TurnStartUI>().AnimStart();
            }
        }
        else
        {
            if (isLocal)
            {
                players[turnPlayer].CallRPCOwner(Player.RpcFunctionName.SET_TURN);
                players[turnPlayer].CallRPCOwner(Player.RpcFunctionName.SET_COMAND_STATE);
            }
            }
    }

    public void SetNextTurnPlayerRPC()
    {
        Invoke("SetNextTurnPlayer",2);
    }



    public void StartCraftFase()
    {
        Invoke("CallCraft", 2.0f);
       
    }

    void CallCraft()
    {
        players[turnPlayer].uiManager.SetCanvas(CanvasName.CRAFT_UI, true);
        players[turnPlayer].uiManager.GetCanvas(CanvasName.CRAFT_UI).GetComponent<CraftUI>().StartCraft();
        Invoke("FinishCraftFase", 15);
    }

    public void FinishCraftFase()
    {
        players[turnPlayer].uiManager.SetCanvas(CanvasName.CRAFT_UI, false);
        SetNextTurnPlayer();
       
    }

    public void StartFirstPlayer()
    {
        players[0].CallRPCOwner(Player.RpcFunctionName.SET_TURN);
        players[0].CallRPCOwner(Player.RpcFunctionName.SET_COMAND_STATE);
        players[0].playerCamera.SetMapCamera(false);
    }
}
