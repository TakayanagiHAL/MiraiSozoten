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
    public float plastic;
    public float ePlastic;
    public float wood;
    public float steel;
    public float seaFood;
}

public class Player : StrixBehaviour
{
    //表示パラメータ
    string playerName;
    [StrixSyncField]
    public float money = 1000;
    [StrixSyncField]
    int shipLevel;
    public MapIndex playerPos;
    int speed;

    float resourceStack = 1200;
    float getPower = 100;

    //資源
    [StrixSyncField]
    public SeaResource seaResource;

    //内部パラメータ
    [SerializeField] Text scoreUI;
    [SerializeField] Text diceUI;

    [StrixSyncField]
    public int moveVol;

    public Canvas comandCanvas;

    [StrixSyncField]
    public TurnState nowState;

    [StrixSyncField]
    public TurnState nextState;

    HexagonManger hexagonManger;

    public TurnContllor turnContllor;

    [StrixSyncField]
    public int turnNum;

    [StrixSyncField]
    public bool isTurn;

    MapIndex[] movePoints;

    PlayerState playerState;

    //ストリクス
    [StrixSyncField]
    UID ownerUid;

    public int getRVol = 0;

    public enum RpcFunctionName
    {
        INIT_PLAYER,
        TRHOW_DICE,
        MOVE_HEXAGON,
        GET_MOVE_SCORES,
        SET_TURN,
        SET_WAIT,
        SET_MOVE_STATE,
        SET_COMAND_STATE
    }

    Dictionary<RpcFunctionName, string> rpcFunctions = new Dictionary<RpcFunctionName, string>()
    {
        {RpcFunctionName.INIT_PLAYER , "Init"},
        {RpcFunctionName.TRHOW_DICE , "ThrowDice"},
        {RpcFunctionName.MOVE_HEXAGON , "MoveHexagon"},
        {RpcFunctionName.GET_MOVE_SCORES , "GetMoveScores"},
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
                    playerState.TurnInit(this);
                    break;
                case TurnState.SELECT_COMAND:
                    playerState = new CommandState();
                    playerState.TurnInit(this);
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

    public void SetTurnPlayer(bool turn)
    {
        if (turn)
        {

        }
        else
        {

        }
    }

    int GetLStickAllow()
    {
        int allow = 5;

        if (Gamepad.current == null) return 0;
        if (Gamepad.current.bButton.wasPressedThisFrame)
        {


            if (Gamepad.current.leftStick.y.ReadValue() > 0.1)
            {
                allow += 3;
            }
            else if (Gamepad.current.leftStick.y.ReadValue() < -0.1)
            {
                allow -= 3;
            }

            if (Gamepad.current.leftStick.x.ReadValue() > 0.25)
            {
                allow += 1;
            }
            else if (Gamepad.current.leftStick.x.ReadValue() < -0.25)
            {
                allow -= 1;
            }
        }
        return allow;
    }

    [StrixRpc]
    public void ThrowDice()
    {
        moveVol = Random.RandomRange(1, 6);

        movePoints = new MapIndex[moveVol];
    }

    [StrixRpc]
    public void MoveHexagon()
    {
        MapIndex mapScale = hexagonManger.GetMapScale();

        int newPos;

        if (Input.GetKeyDown(KeyCode.W))
        {
            newPos = playerPos.y - 1;
            if (newPos >= 0)
            {
                playerPos.y = newPos;
                moveVol--;
                movePoints[moveVol] = playerPos;
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            newPos = playerPos.x - 1;
            if (newPos >= 0)
            {
                playerPos.x = newPos;
                moveVol--;
                movePoints[moveVol] = playerPos;
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            newPos = playerPos.y + 1;
            if (newPos < mapScale.y)
            {
                playerPos.y = newPos;
                moveVol--;
                movePoints[moveVol] = playerPos;
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            newPos = playerPos.x + 1;
            if (newPos < mapScale.x)
            {
                playerPos.x = newPos;
                moveVol--;
                movePoints[moveVol] = playerPos;
            }
        }
        int allow = GetLStickAllow();

        Debug.Log("Allow:" + allow);
        
        switch (allow)
        {
            case 1:
                if ((playerPos.x % 2) == 0)
                {
                    newPos = playerPos.x - 1;
                    if (newPos >= 0)
                    {
                        playerPos.x = newPos;
                        moveVol--;
                        movePoints[moveVol] = playerPos;
                    }
                }
                else
                {
                    newPos = playerPos.y + 1;
                    if (newPos < mapScale.y)
                    {
                        newPos = playerPos.x - 1;
                        if (newPos >= 0)
                        {
                            playerPos.x = newPos;
                            playerPos.y += 1;
                            moveVol--;
                            movePoints[moveVol] = playerPos;
                        }
                    }
                }
                break;
            case 2:
                newPos = playerPos.y + 1;
                if (newPos < mapScale.y)
                {
                    playerPos.y = newPos;
                    moveVol--;
                    movePoints[moveVol] = playerPos;
                }
                break;
            case 3:
                if ((playerPos.x % 2) == 0)
                {
                    newPos = playerPos.x + 1;
                    if (newPos < mapScale.x)
                    {
                        playerPos.x = newPos;
                        moveVol--;
                        movePoints[moveVol] = playerPos;
                    }
                }
                else
                {
                    newPos = playerPos.y + 1;
                    if (newPos < mapScale.y)
                    {
                        newPos = playerPos.x + 1;
                        if (newPos < mapScale.x)
                        {
                            playerPos.x = newPos;
                            playerPos.y += 1;
                            moveVol--;
                            movePoints[moveVol] = playerPos;
                        }
                    }
                }
                break;
            case 7:
                if ((playerPos.x % 2) == 0)
                {
                    newPos = playerPos.y - 1;
                    if (newPos >= 0)
                    {
                        newPos = playerPos.x - 1;
                        if (newPos >= 0)
                        {
                            playerPos.x = newPos;
                            playerPos.y -= 1;
                            moveVol--;
                            movePoints[moveVol] = playerPos;
                        }
                    }
                }
                else
                {
                    newPos = playerPos.x - 1;
                    if (newPos >= 0)
                    {
                        playerPos.x = newPos;
                        moveVol--;
                        movePoints[moveVol] = playerPos;
                    }
                }
                break;
            case 8:
                newPos = playerPos.y - 1;
                if (newPos >= 0)
                {
                    playerPos.y = newPos;
                    moveVol--;
                    movePoints[moveVol] = playerPos;
                }
                break;
            case 9:
                if ((playerPos.x % 2) == 0)
                {
                    newPos = playerPos.y - 1;
                    if (newPos >= 0)
                    {
                        newPos = playerPos.x + 1;
                        if (newPos < mapScale.x)
                        {
                            playerPos.x = newPos;
                            playerPos.y -= 1;
                            moveVol--;
                            movePoints[moveVol] = playerPos;
                        }
                    }
                }
                else
                {
                    newPos = playerPos.x + 1;
                    if (newPos < mapScale.x)
                    {
                        playerPos.x = newPos;
                        moveVol--;
                        movePoints[moveVol] = playerPos;
                    }
                }
                break;

        }



        diceUI.text = moveVol.ToString();

        Vector2 pos = hexagonManger.GetMapPos(playerPos);

        transform.position = new Vector3(pos.x, pos.y, 0);
    }

    [StrixRpc]
    public void GetMoveScores()
    {
        Hexagon hexagon;
        for (int i = 0; i < movePoints.Length; i++)
        {
            hexagon = hexagonManger.GetHexagon(movePoints[i]);
            hexagon.OnPassage(this);
        }

        seaResource.plastic += 0.45f * 100 * getRVol;
        seaResource.ePlastic += 0.25f * 100 * getRVol;
        seaResource.wood += 0.15f * 100 * getRVol;
        seaResource.steel += 0.05f * 100 * getRVol;
        seaResource.seaFood += 0.1f * 100 * getRVol;

        getRVol = 0;

        hexagonManger.GetHexagon(playerPos).OnReach(this);

        Debug.Log(seaResource);
        moveVol = -1;
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
