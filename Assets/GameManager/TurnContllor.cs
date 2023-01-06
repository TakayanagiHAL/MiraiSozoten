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
    HAPPNING_EVENT

}

public class TurnContllor : StrixBehaviour
{

    HexagonManger hexagonManger;

    UIManager uiManager;

    public Player[] players;

    [SerializeField] int playerVol;

    [SerializeField] int maxTurn;

    int nowTurn;

    int turnPlayer;

    // Start is called before the first frame update
    void Start()
    {
        hexagonManger = FindObjectOfType<HexagonManger>();
        turnPlayer = 0;

        players = FindObjectsOfType<Player>();

        for(int i = 0; i < players.Length; i++)
        {
            players[i].turnNum = i;

            playerVol = players.Length;

            players[i].CallRPCOwner(Player.RpcFunctionName.INIT_PLAYER);
        }

        players[0].CallRPCOwner(Player.RpcFunctionName.SET_TURN);
        players[0].CallRPCOwner(Player.RpcFunctionName.SET_COMAND_STATE);
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
            StartCraftFase();
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
    }

    public void FinishCraftFase()
    {
        uiManager.SetCanvas(CanvasName.CRAFT_UI, false);
        players[turnPlayer].CallRPCOwner(Player.RpcFunctionName.SET_TURN);
        players[turnPlayer].CallRPCOwner(Player.RpcFunctionName.SET_COMAND_STATE);
    }
}
