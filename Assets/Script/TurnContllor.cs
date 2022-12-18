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

}

public class TurnContllor : StrixBehaviour
{

    HexagonManger hexagonManger;

    [SerializeField] Player[] players;

    [SerializeField] int playerVol;

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
        if(turnPlayer % playerVol == 0)
        {
            turnPlayer = 0;
            nowTurn++;
        }
        players[turnPlayer].CallRPCOwner(Player.RpcFunctionName.SET_TURN);
        players[turnPlayer].CallRPCOwner(Player.RpcFunctionName.SET_COMAND_STATE);
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
}
