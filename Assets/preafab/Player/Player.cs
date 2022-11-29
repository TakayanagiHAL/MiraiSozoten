using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using SoftGear.Strix.Unity.Runtime;
using SoftGear.Strix.Client.Core;

[System.Serializable]
public struct SeaResource
{
    public int plastic;
    public int ePlastic;
    public int wood;
    public int steel;
    public int seaFood;
}

public class Player : StrixBehaviour
{
    //表示パラメータ
    string playerName;
    [StrixSyncField]
    public int money = 1000;
    [StrixSyncField]
    int shipLevel;

    public List<Item> items;
    public MapIndex playerPos;
    int speed;

    public int resourceStack = 1200;
    int getPower = 100;

    int getDepth;
    int searchPower;

    //クラフト対象
    int dieselEngine;
    int shipBody;
    int whaleMouse;
    int crane;
    int sonar;

    //資源
    [StrixSyncField]
    public SeaResource seaResource;

    //内部パラメータ
    [SerializeField] Text scoreUI;
    public Text diceUI;

    [StrixSyncField]
    public int moveVol;

    public Canvas comandCanvas;

    [StrixSyncField]
    public TurnState nowState;

    [StrixSyncField]
    public TurnState nextState;

    HexagonManger hexagonManger;

    TurnContllor turnContllor;

    [StrixSyncField]
    public int turnNum;

    [StrixSyncField]
    public bool isTurn;

    public MapIndex[] movePoints;

    PlayerState playerState;

    //ストリクス
    [StrixSyncField]
    UID ownerUid;

    public int getRVol = 0;

    public enum RpcFunctionName
    {
        INIT_PLAYER,
        SET_TURN,
        SET_WAIT,
        SET_MOVE_STATE,
        SET_COMAND_STATE
    }

    Dictionary<RpcFunctionName, string> rpcFunctions = new Dictionary<RpcFunctionName, string>()
    {
        {RpcFunctionName.INIT_PLAYER , "Init"},
        {RpcFunctionName.SET_TURN, "SetTurn"},
        { RpcFunctionName.SET_WAIT, "SetWait"},
        {RpcFunctionName.SET_MOVE_STATE, "SetMoveState"},
        {RpcFunctionName.SET_COMAND_STATE , "SetComandState" }
    };
    // Start is called before the first frame update
    void Start()
    {
        seaResource.ePlastic = 75;
        seaResource.plastic = 150;
        seaResource.seaFood = 0;
        seaResource.steel = 15;
        seaResource.wood = 45;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocal) return;

        if (nowState != nextState)
        {
            switch (nextState)
            {
                case TurnState.PLAYER_MOVE:
                    playerState = new MoveState();
                    playerState.TurnInit(this,hexagonManger,turnContllor);
                    break;
                case TurnState.SELECT_COMAND:
                    playerState = new CommandState();
                    playerState.TurnInit(this, hexagonManger, turnContllor);
                    break;
            }
            nowState = nextState;
        }

        if (isTurn) playerState.TurnUpdate(this);
       
    }

    [StrixRpc]
    public void Init()
    {
        hexagonManger = FindObjectOfType<HexagonManger>();

        turnContllor = FindObjectOfType<TurnContllor>();

        nowState = TurnState.TURN_WAIT;
        nextState = TurnState.TURN_WAIT;

        SetWait();

        playerPos.x = 0;
        playerPos.y = 0;
        moveVol = -1;
    }

    public void CallRPCOwner(RpcFunctionName fName,params object[] param)
    {
        if (isLocal)
        {
            Invoke(rpcFunctions[fName], 0);
        }
        else
        {
            Rpc(strixReplicator.ownerUid, rpcFunctions[fName], param);

            Debug.Log(fName + "Called");
        }

    }

    public void CallRPCAll(RpcFunctionName fName, params object[] param)
    {
        RpcToAll(rpcFunctions[fName], param);
        Debug.Log(fName + "Called");
    }

    public void CallRPCRoomOwner(RpcFunctionName fName, params object[] param)
    {
        RpcToRoomOwner(rpcFunctions[fName], param);
        Debug.Log(fName + "Called");
    }

    [StrixRpc]
    public void SetTurn()
    {
        isTurn = true;
        this.transform.GetChild(0).gameObject.SetActive(true);
    }

    [StrixRpc]
    public void SetWait()
    {
        isTurn = false;
        this.transform.GetChild(0).gameObject.SetActive(false);
        nextState = TurnState.TURN_WAIT;
    }

    [StrixRpc]
    public void SetMoveState()
    {
        nextState = TurnState.PLAYER_MOVE;
    }

    [StrixRpc]
    public void SetComandState()
    {
        nextState = TurnState.SELECT_COMAND;

        Debug.Log("SetComandState");
    }

    public TurnState GetNextState()
    {
        return nextState;
    }
}
