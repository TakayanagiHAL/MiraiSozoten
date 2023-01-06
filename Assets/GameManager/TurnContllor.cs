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

    public Player[] players;
    [StrixSyncField]
    int playerVol;

    [SerializeField] int maxTurn;

    [StrixSyncField]
    int nowTurn =1;
    [StrixSyncField]
    int turnPlayer =0;
    [StrixSyncField]
    RenderTexture[] renderTextures;

    int thisPlayer = 0;

    // Start is called before the first frame update
    void Start()
    {
        hexagonManger = FindObjectOfType<HexagonManger>();
        turnPlayer = 0;
        uiManager = FindObjectOfType<UIManager>();

        onotherUI = FindObjectOfType<OnotherUI>();

        Player[] p = FindObjectsOfType<Player>();
        players = new Player[p.Length];
        playerVol = players.Length;

        renderTextures = new RenderTexture[4];

        foreach(var RoomMember in StrixNetwork.instance.sortedRoomMembers)
        {
            if(StrixNetwork.instance.selfRoomMember.GetUid() != RoomMember.GetUid())
            {
                thisPlayer++;
            }
            else
            {
                break;
            }
        }

        for(int i = 0; i < 4; i++)
        {
            renderTextures[i] = new RenderTexture(1920, 1080, 16);
        }

        for (int i = 0; i < players.Length; i++)
        {
            players[p[i].turnNum] = p[i];         
        }
        players[thisPlayer].CallRPCOwner(Player.RpcFunctionName.INIT_PLAYER);

        players[thisPlayer].playerCamera.SetRenderTexture(renderTextures[thisPlayer]);

        uiManager.SetCamera(players[thisPlayer].playerCamera.GetCamera());

        uiManager.GetCanvas(CanvasName.TURN_START_UI).GetComponent<TurnStartUI>().SetTurn(nowTurn);
        uiManager.SetCanvas(CanvasName.TURN_START_UI, true);
        uiManager.GetCanvas(CanvasName.TURN_START_UI).GetComponent<TurnStartUI>().AnimStart();
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
                FindObjectOfType<ResultData>().ResultStart = true;
            }
            else
            {
                Invoke("StartCraftFase", 1);
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
        uiManager.GetCanvas(CanvasName.CRAFT_UI).GetComponent<CraftUI>().StartCraft();
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
