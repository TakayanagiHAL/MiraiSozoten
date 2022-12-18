using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class HappningState : PlayerState
{
    enum HState
    {
        SELECT_HAPPNING,
        EXE_HAPPNING
    }

    HappningUI happningUI;

    int useHappning;

    HState state;

    public override void TurnInit(Player player, HexagonManger hexagon, TurnContllor turn, UIManager ui)
    {
        Debug.Log("HAPPEN");
        base.TurnInit(player, hexagon, turn,ui);

        happningUI = uiManager.GetCanvas(CanvasName.HAPPNING_UI).GetComponent<HappningUI>();

        uiManager.SetCanvas(CanvasName.HAPPNING_UI, true);

        useHappning = -1;
    }
    override public void TurnUpdate(Player player)
    {
        switch (state)
        {
            case HState.SELECT_HAPPNING:
                useHappning = happningUI.GetHappen();
                if (useHappning >= 0)
                {
                    state = HState.EXE_HAPPNING;

                    Debug.Log(useHappning);
                }
                break;

            case HState.EXE_HAPPNING:
                switch(useHappning)
                {
                    case 0:
                        turnContllor.players[0/*Random.Range(0, 5)*/].CallRPCOwner(Player.RpcFunctionName.RANDAM_TELEPORT);
                        player.SetWait();
                        uiManager.SetCanvas(CanvasName.HAPPNING_UI, false);
                        turnContllor.SetNextTurnPlayerRPC();
                        happningUI.SetSelect();
                        break;
                     case 1:
                        //int nq = Random.Range(0, 5);
                        //turnContllor.players[nq].CallRPCOwner(Player.RpcFunctionName.RANDAM_TELEPORT);
                        //while (true)
                        //{
                        //    int ne = Random.Range(0, 5);
                        //    if (nq != ne)
                        //    {
                        //        nq = ne; break;
                        //    }
                        //}
                        //turnContllor.players[nq].CallRPCOwner(Player.RpcFunctionName.RANDAM_TELEPORT);
                        player.SetWait();
                        uiManager.SetCanvas(CanvasName.HAPPNING_UI, false);
                        turnContllor.SetNextTurnPlayerRPC();
                        happningUI.SetSelect();
                        break;
                     case 2:
                        //int n = 0;
                        //n = Random.Range(0, 5);
                        //for(int i = 0; i < 4; i++)
                        //{
                        //    if (i == n) continue;

                        //    turnContllor.players[i].CallRPCOwner(Player.RpcFunctionName.RANDAM_TELEPORT);
                        //}
                        player.SetWait();
                        uiManager.SetCanvas(CanvasName.HAPPNING_UI, false);
                        turnContllor.SetNextTurnPlayerRPC();
                        happningUI.SetSelect();
                        break;
                     case 3:
                        //for(int i = 0; i < 4;i++)
                        //{
                        //    turnContllor.players[i].CallRPCOwner(Player.RpcFunctionName.RANDAM_TELEPORT);
                        //}
                        player.SetWait();
                        uiManager.SetCanvas(CanvasName.HAPPNING_UI, false);
                        turnContllor.SetNextTurnPlayerRPC();
                        happningUI.SetSelect();
                        break;
                     case 4:
                        player.SetWait();
                        player.AddSeaResouce(GetResourceVol(player.searchPower));
                        uiManager.SetCanvas(CanvasName.HAPPNING_UI, false);
                        turnContllor.SetNextTurnPlayerRPC();
                        happningUI.SetSelect();
                        break;
                     case 5:
                        player.SetWait();
                        player.AddSeaResouce(GetResourceVol(player.searchPower));
                        uiManager.SetCanvas(CanvasName.HAPPNING_UI, false);
                        turnContllor.SetNextTurnPlayerRPC();
                        happningUI.SetSelect();
                        break;
                     case 6:
                        player.SetWait();
                        uiManager.SetCanvas(CanvasName.HAPPNING_UI, false);
                        turnContllor.SetNextTurnPlayerRPC();
                        happningUI.SetSelect();
                        break;
                }
                break;
        }
    }

    int GetResourceVol(int search)
    {
        int vol = 0;
        float per = Random.Range(0.0f, 100.0f);
        switch (search)
        {
            case 0:
                if (per < 2.0f)
                {
                    vol = 8;
                }else if(per < 9.0f)
                {
                    vol = 5;
                }else if(per < 24.5f)
                {
                    vol = 3;
                }else if(per < 40.0f)
                {
                    vol = 1;
                }else if (per < 55.5f)
                {
                    vol = -1;
                }else if (per < 71.0f)
                {
                    vol = -3;
                }else if (per < 86.5f)
                {
                    vol = -5;
                }else
                {
                    vol = -8;
                }
                break;
            case 1:
                if (per < 2.0f)
                {
                    vol = 8;
                }
                else if (per < 14.0f)
                {
                    vol = 5;
                }
                else if (per < 35.0f)
                {
                    vol = 3;
                }
                else if (per < 70.0f)
                {
                    vol = 1;
                }
                else if (per < 83.0f)
                {
                    vol = -1;
                }
                else if (per < 92.0f)
                {
                    vol = -3;
                }
                else if (per < 98.0f)
                {
                    vol = -5;
                }
                else
                {
                    vol = -8;
                }
                break;
            case 2:
                if (per < 2.0f)
                {
                    vol = 8;
                }
                else if (per < 15.0f)
                {
                    vol = 5;
                }
                else if (per < 41.0f)
                {
                    vol = 3;
                }
                else if (per < 75.0f)
                {
                    vol = 1;
                }
                else if (per < 88.0f)
                {
                    vol = -1;
                }
                else if (per < 96.0f)
                {
                    vol = -3;
                }
                else if (per < 100.0f)
                {
                    vol = -5;
                }
                else
                {
                    vol = -8;
                }
                break;
            case 3:
                if (per < 5.0f)
                {
                    vol = 8;
                }
                else if (per < 20.0f)
                {
                    vol = 5;
                }
                else if (per < 47.0f)
                {
                    vol = 3;
                }
                else if (per < 80.0f)
                {
                    vol = 1;
                }
                else if (per < 93.0f)
                {
                    vol = -1;
                }
                else if (per < 100.0f)
                {
                    vol = -3;
                }
                else if (per < 86.5f)
                {
                    vol = -5;
                }
                else
                {
                    vol = -8;
                }
                break;
            case 4:
                if (per < 8.0f)
                {
                    vol = 8;
                }
                else if (per < 25.0f)
                {
                    vol = 5;
                }
                else if (per < 55.0f)
                {
                    vol = 3;
                }
                else if (per < 85.0f)
                {
                    vol = 1;
                }
                else if (per < 95.0f)
                {
                    vol = -1;
                }
                else if (per < 100.0f)
                {
                    vol = -3;
                }
                else if (per < 86.5f)
                {
                    vol = -5;
                }
                else
                {
                    vol = -8;
                }
                break;
            case 5:
                if (per < 10.0f)
                {
                    vol = 8;
                }
                else if (per < 30.0f)
                {
                    vol = 5;
                }
                else if (per < 65.0f)
                {
                    vol = 3;
                }
                else if (per < 90.0f)
                {
                    vol = 1;
                }
                else if (per < 98.5f)
                {
                    vol = -1;
                }
                else if (per < 100.0f)
                {
                    vol = -3;
                }
                else if (per < 86.5f)
                {
                    vol = -5;
                }
                else
                {
                    vol = -8;
                }
                break;
            case 6:
                if (per < 15.0f)
                {
                    vol = 8;
                }
                else if (per < 45.0f)
                {
                    vol = 5;
                }
                else if (per < 80.0f)
                {
                    vol = 3;
                }
                else if (per < 95.0f)
                {
                    vol = 1;
                }
                else if (per < 100.0f)
                {
                    vol = -1;
                }
                else if (per < 71.0f)
                {
                    vol = -3;
                }
                else if (per < 86.5f)
                {
                    vol = -5;
                }
                else
                {
                    vol = -8;
                }
                break;
            case 7:
                if (per < 26.0f)
                {
                    vol = 8;
                }
                else if (per < 62.0f)
                {
                    vol = 5;
                }
                else if (per < 88.0f)
                {
                    vol = 3;
                }
                else if (per < 100.0f)
                {
                    vol = 1;
                }
                else if (per < 55.5f)
                {
                    vol = -1;
                }
                else if (per < 71.0f)
                {
                    vol = -3;
                }
                else if (per < 86.5f)
                {
                    vol = -5;
                }
                else
                {
                    vol = -8;
                }
                break;
            case 8:
                if (per < 40.0f)
                {
                    vol = 8;
                }
                else if (per < 75.0f)
                {
                    vol = 5;
                }
                else if (per < 100.0f)
                {
                    vol = 3;
                }
                else if (per < 40.0f)
                {
                    vol = 1;
                }
                else if (per < 55.5f)
                {
                    vol = -1;
                }
                else if (per < 71.0f)
                {
                    vol = -3;
                }
                else if (per < 86.5f)
                {
                    vol = -5;
                }
                else
                {
                    vol = -8;
                }
                break;
            case 9:
                if (per < 60.0f)
                {
                    vol = 8;
                }
                else if (per < 100.0f)
                {
                    vol = 5;
                }
                else if (per < 24.5f)
                {
                    vol = 3;
                }
                else if (per < 40.0f)
                {
                    vol = 1;
                }
                else if (per < 55.5f)
                {
                    vol = -1;
                }
                else if (per < 71.0f)
                {
                    vol = -3;
                }
                else if (per < 86.5f)
                {
                    vol = -5;
                }
                else
                {
                    vol = -8;
                }
                break;
        }
        return vol;
    }
}
