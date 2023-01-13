using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class MoveState : PlayerState
{
    enum MState
    {
        ROLL_DICE,
        MOVE_INPUT,
        MOVING,
        MOVE_FINISH
    }

    ResourceUI resourceUI;
    ResourceEffectUI effectUI;
    DiceUI diceUI;
    ScoreUI scoreUI;
    MapIndex useDice;
    int moveVol;
    MapIndex[] movePoints;

    float speed = 5f;

    MState state;

     MapIndex[] dice = new MapIndex[]
    {
        new MapIndex(1,6),
        new MapIndex(2,7),
        new MapIndex(2,8),
        new MapIndex(2,9),
        new MapIndex(2,10),
        new MapIndex(2,11),
    };
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

    int GetKeyAllow()
    {
        int allow = 5;

        if (Input.GetKeyUp(KeyCode.Return))
        {
            if (Input.GetKey(KeyCode.W))
            {
                allow = 8;
                if (Input.GetKey(KeyCode.A))
                {
                    allow = 7;
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    allow = 9;
                }
            }
            else if (Input.GetKey(KeyCode.S))
            {
                allow = 2;
                if (Input.GetKey(KeyCode.A))
                {
                    allow = 1;
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    allow = 3;
                }
            }
        }

        return allow;
    }

    public override void TurnInit(Player player, HexagonManger hexagon, TurnContllor turn, UIManager ui)
    {
        base.TurnInit(player, hexagon, turn,ui);
        player.nowState = TurnState.PLAYER_MOVE;
        uiManager.SetCanvas(CanvasName.COMMAND_UI, false);
        uiManager.SetCanvas(CanvasName.DICE_UI, true);

        resourceUI = uiManager.GetCanvasObject(CanvasName.RESOURCE_UI).GetComponent<ResourceUI>();
        diceUI = uiManager.GetCanvasObject(CanvasName.DICE_UI).GetComponent<DiceUI>();
        effectUI = uiManager.GetCanvasObject(CanvasName.R_EFFECT_UI).GetComponent<ResourceEffectUI>();
        scoreUI = uiManager.GetCanvasObject(CanvasName.SCORE_UI).GetComponent<ScoreUI>();

        useDice = dice[player.speed / 6];
        diceUI.SetMinMax(useDice.x, useDice.y);

        moveVol = -1;
        diceUI.SetMoveVol(-1);

        state = MState.ROLL_DICE;
    }
    override public void TurnUpdate(Player player)
    {
        Debug.Log(state);
        switch (state)
        {
            case MState.ROLL_DICE:
                moveVol = diceUI.GetMoveVol();

                if (moveVol > 0)
                {
                    movePoints = new MapIndex[moveVol + 1];
                    movePoints[moveVol] = player.playerPos;
                    state = MState.MOVE_INPUT;
                }   
                break;
            case MState.MOVE_INPUT:
                MapIndex mapScale = hexagonManger.GetMapScale();

                MapIndex newPos = player.playerPos;

                bool isMove = false;
               
                int allow = GetLStickAllow();

                if (allow == 0) allow = GetKeyAllow();

                Debug.Log("Allow:" + allow);

                player.StartMoving();

                switch (allow)
                {
                    case 1:
                        if ((player.playerPos.x % 2) == 0)
                        {
                            newPos.x = player.playerPos.x - 1;
                            if (newPos.x >= 0)
                            {
                                if (hexagonManger.GetHexagon(newPos).GetHexagonType() == HexagonType.NONE) break;

                                player.playerPos = newPos;
                                moveVol--;
                                movePoints[moveVol] = player.playerPos;
                                isMove = true;
                            }
                        }
                        else
                        {
                            newPos.y = player.playerPos.y + 1;
                            if (newPos.y < mapScale.y)
                            {
                                newPos.x = player.playerPos.x - 1;
                                if (newPos.x >= 0)
                                {
                                    if (hexagonManger.GetHexagon(newPos).GetHexagonType() == HexagonType.NONE) break;

                                    player.playerPos = newPos;
                                    moveVol--;
                                    movePoints[moveVol] = player.playerPos;
                                    isMove = true;
                                }
                            }
                        }
                        break;
                    case 2:
                        newPos.y = player.playerPos.y + 1;
                        if (newPos.y < mapScale.y)
                        {
                            if (hexagonManger.GetHexagon(newPos).GetHexagonType() == HexagonType.NONE) break;

                            player.playerPos = newPos;
                            moveVol--;
                            movePoints[moveVol] = player.playerPos;
                            isMove = true;
                        }
                        break;
                    case 3:
                        if ((player.playerPos.x % 2) == 0)
                        {
                            newPos.x = player.playerPos.x + 1;
                            if (newPos.x < mapScale.x)
                            {
                                if (hexagonManger.GetHexagon(newPos).GetHexagonType() == HexagonType.NONE) break;

                                player.playerPos = newPos;
                                moveVol--;
                                movePoints[moveVol] = player.playerPos;
                                isMove = true;
                            }
                        }
                        else
                        {
                            newPos.y = player.playerPos.y + 1;
                            if (newPos.y < mapScale.y)
                            {
                                newPos.x = player.playerPos.x + 1;
                                if (newPos.x < mapScale.x)
                                {
                                    if (hexagonManger.GetHexagon(newPos).GetHexagonType() == HexagonType.NONE) break;

                                    player.playerPos = newPos;
                                    moveVol--;
                                    movePoints[moveVol] = player.playerPos;
                                    isMove = true;
                                }
                            }
                        }
                        break;
                    case 7:
                        if ((player.playerPos.x % 2) == 0)
                        {
                            newPos.y = player.playerPos.y - 1;
                            if (newPos.y >= 0)
                            {
                                newPos.x = player.playerPos.x - 1;
                                if (newPos.x >= 0)
                                {
                                    if (hexagonManger.GetHexagon(newPos).GetHexagonType() == HexagonType.NONE) break;

                                    player.playerPos = newPos;
                                    moveVol--;
                                    movePoints[moveVol] = player.playerPos;
                                    isMove = true;
                                }
                            }
                        }
                        else
                        {
                            newPos.x = player.playerPos.x - 1;
                            if (newPos.x >= 0)
                            {
                                if (hexagonManger.GetHexagon(newPos).GetHexagonType() == HexagonType.NONE) break;

                                player.playerPos = newPos;
                                moveVol--;
                                movePoints[moveVol] = player.playerPos;
                                isMove = true;
                            }
                        }
                        break;
                    case 8:
                        newPos.y = player.playerPos.y - 1;
                        if (newPos.y >= 0)
                        {
                            if (hexagonManger.GetHexagon(newPos).GetHexagonType() == HexagonType.NONE) break;

                            player.playerPos = newPos;
                            moveVol--;
                            movePoints[moveVol] = player.playerPos;
                            isMove = true;
                        }
                        break;
                    case 9:
                        if ((player.playerPos.x % 2) == 0)
                        {
                            newPos.y = player.playerPos.y - 1;
                            if (newPos.y >= 0)
                            {
                                newPos.x = player.playerPos.x + 1;
                                if (newPos.x < mapScale.x)
                                {
                                    if (hexagonManger.GetHexagon(newPos).GetHexagonType() == HexagonType.NONE) break;

                                    player.playerPos = newPos;
                                    moveVol--;
                                    movePoints[moveVol] = player.playerPos;
                                    isMove = true;
                                }
                            }
                        }
                        else
                        {
                            newPos.x = player.playerPos.x + 1;
                            if (newPos.x < mapScale.x)
                            {
                                if (hexagonManger.GetHexagon(newPos).GetHexagonType() == HexagonType.NONE) break;

                                player.playerPos = newPos;
                                moveVol--;
                                movePoints[moveVol] = player.playerPos;
                                isMove = true;
                            }
                        }
                        break;
                }

                

                player.transform.LookAt(hexagonManger.GetMapPos(player.playerPos));

                //player.transform.position = hexagonManger.GetMapPos(player.playerPos);

                diceUI.SetMoveVol(moveVol);

                if(isMove) state = MState.MOVING; 
                break;
            case MState.MOVING:
                Vector3 dir = hexagonManger.GetMapPos(player.playerPos) - player.transform.position;
                player.transform.Translate(dir / dir.magnitude*speed*Time.deltaTime,Space.World);

                if(dir.magnitude < 0.25)
                {
                    player.transform.position = hexagonManger.GetMapPos(player.playerPos);
                    if (moveVol == 0)
                    {
                        state = MState.MOVE_FINISH;
                    }
                    else
                    {
                        state = MState.MOVE_INPUT;
                    }
                }
               
                break;
            case MState.MOVE_FINISH:
                switch (diceUI.GetMoveOK())
                {
                    case MoveSelect.NO:
                        diceUI.SetMoveWAIT();
                        moveVol = 1;
                        player.playerPos = movePoints[moveVol];
                        //player.transform.position = hexagonManger.GetMapPos(player.playerPos);
                        player.transform.LookAt(hexagonManger.GetMapPos(player.playerPos));
                        diceUI.SetMoveVol(moveVol);
                        state = MState.MOVING;
                        break;
                    case MoveSelect.OK:
                        Hexagon hexagon;
                        for (int i = 0; i < movePoints.Length - 1; i++)
                        {
                            hexagon = hexagonManger.GetHexagon(movePoints[i]);

                            hexagon.OnPassage(player);
                        }
                        player.StopMoving();

                        SeaResource getResource;

                        getResource.plastic = (int)(0.45f * 100 * player.getRVol);
                        getResource.ePlastic = (int)(0.25f * 100 * player.getRVol);
                        getResource.wood = (int)(0.15f * 100 * player.getRVol);
                        getResource.steel = (int)(0.05f * 100 * player.getRVol);
                        getResource.seaFood = (int)(0.10f * 100 * player.getRVol);

                        player.seaResource = player.seaResource + getResource;

                        player.getRVol = 0;

                        if (player.seaResource.plastic +
                           player.seaResource.ePlastic +
                           player.seaResource.wood +
                           player.seaResource.steel +
                           player.seaResource.seaFood > player.resourceStack)
                        {
                            int nowRVol =
                            player.seaResource.plastic +
                            player.seaResource.ePlastic +
                            player.seaResource.wood +
                            player.seaResource.steel +
                            player.seaResource.seaFood;

                            if (nowRVol - player.resourceStack <= player.seaResource.plastic)
                            {
                                player.seaResource.plastic -= (nowRVol - player.resourceStack);
                            }
                            else
                            {
                                player.seaResource.plastic = 0;
                                nowRVol -= player.seaResource.plastic;
                                player.seaResource.ePlastic -= (nowRVol - player.resourceStack);
                            }
                        }

                        effectUI.SetAction(getResource);

                        resourceUI.SetResource(player.seaResource);

                        scoreUI.SetMoney(player.money);
                        scoreUI.SetOil(player.seaResource);

                        hexagonManger.GetHexagon(player.playerPos).OnReach(player);

                        moveVol = -1;
                        
                  

                        diceUI.SetMoveWAIT();

                        uiManager.SetCanvas(CanvasName.DICE_UI, false);
                        break;
                }
                break; 
        }
        

    }


}
