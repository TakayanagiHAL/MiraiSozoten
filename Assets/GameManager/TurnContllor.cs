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

    [StrixSyncField] public Player[] players;

    [StrixSyncField] int playerVol;

    [SerializeField] int maxTurn;

    [StrixSyncField] int nowTurn =1;

    [StrixSyncField] int turnPlayer;

    // Start is called before the first frame update
    void Start()
    {
        hexagonManger = FindObjectOfType<HexagonManger>();
        turnPlayer = 0;
        uiManager = FindObjectOfType<UIManager>();

        players = FindObjectsOfType<Player>();

        for (int i = 0; i < players.Length; i++)
        {
            players[i].turnNum = i;

            playerVol = players.Length;

            players[i].CallRPCOwner(Player.RpcFunctionName.INIT_PLAYER);
        }

        uiManager.GetCanvas(CanvasName.TURN_START_UI).GetComponent<TurnStartUI>().SetTurn(nowTurn);
        uiManager.SetCanvas(CanvasName.TURN_START_UI, true);
        uiManager.GetCanvas(CanvasName.TURN_START_UI).GetComponent<TurnStartUI>().AnimStart();
    }

    // Update is called once per frame
    void Update()
    {

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
                FindObjectOfType<ResultData>().ResultStart = true;
            }
            else
            {
                Invoke("StartCraftFase", 3);
            }

        }
        else
        {
            players[turnPlayer].CallRPCOwner(Player.RpcFunctionName.SET_TURN);
            players[turnPlayer].CallRPCOwner(Player.RpcFunctionName.SET_COMAND_STATE);
        }
    }

    public void SetNextTurnPlayerRPC()
    {
        if (isLocal)
        {
            SetNextTurnPlayer();
        }
        else
        {
            Rpc(strixReplicator.ownerUid, "SetNextTurnPlayer");

        }
    }

    void StartCraftFase()
    {
        uiManager.SetCanvas(CanvasName.CRAFT_UI, true);
        Invoke("FinishCraftFase", 60);
    }

    public void FinishCraftFase()
    {
        players[0].playerCamera.SetMapCamera(true);
        uiManager.SetCanvas(CanvasName.CRAFT_UI, false);
        uiManager.SetCanvas(CanvasName.TURN_START_UI, true);
        uiManager.GetCanvas(CanvasName.TURN_START_UI).GetComponent<TurnStartUI>().SetTurn(nowTurn);
        uiManager.GetCanvas(CanvasName.TURN_START_UI).GetComponent<TurnStartUI>().AnimStart();
    }

    public void StartFirstPlayer()
    {
        players[0].CallRPCOwner(Player.RpcFunctionName.SET_TURN);
        players[0].CallRPCOwner(Player.RpcFunctionName.SET_COMAND_STATE);
        players[0].playerCamera.SetMapCamera(false);
    }
}
