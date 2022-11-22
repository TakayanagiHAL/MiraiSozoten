using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveState : PlayerState
{
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

    public override void TurnInit(Player player, HexagonManger hexagon, TurnContllor turn)
    {
        base.TurnInit(player, hexagon, turn);
        player.nowState = TurnState.PLAYER_MOVE;
        player.comandCanvas.gameObject.SetActive(false);
    }
    override public void TurnUpdate(Player player)
    {
        if (player.moveVol < 0)
        {
            player.moveVol = Random.RandomRange(1, 6);

            player.movePoints = new MapIndex[player.moveVol];
        }
        else if (player.moveVol == 0)
        {
            Hexagon hexagon;
            for (int i = 0; i < player.movePoints.Length; i++)
            {
                hexagon = hexagonManger.GetHexagon(player.movePoints[i]);
                hexagon.OnPassage(player);
            }

            player.seaResource.plastic  += (int)(0.45f * 100 * player.getRVol);
            player.seaResource.ePlastic += (int)(0.25f * 100 * player.getRVol);
            player.seaResource.wood     += (int)(0.15f * 100 * player.getRVol);
            player.seaResource.steel    += (int)(0.05f * 100 * player.getRVol);
            player.seaResource.seaFood  += (int)(0.10f * 100 * player.getRVol);

            player.getRVol = 0;

            if(player.seaResource.plastic  +
               player.seaResource.ePlastic + 
               player.seaResource.wood     + 
               player.seaResource.steel    + 
               player.seaResource.seaFood  > player.resourceStack)
            {
                int nowRVol =
                player.seaResource.plastic +
                player.seaResource.ePlastic +
                player.seaResource.wood +
                player.seaResource.steel +
                player.seaResource.seaFood;

                if(nowRVol- player.resourceStack <= player.seaResource.plastic)
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

            hexagonManger.GetHexagon(player.playerPos).OnReach(player);

            player.moveVol = -1;

            turnContllor.SetNextTurnPlayerRPC();

            player.SetWait();
        }
        else
        {
            MapIndex mapScale = hexagonManger.GetMapScale();

            int newPos;

            if (Input.GetKeyDown(KeyCode.W))
            {
                newPos = player.playerPos.y - 1;
                if (newPos >= 0)
                {
                    player.playerPos.y = newPos;
                    player.moveVol--;
                    player.movePoints[player.moveVol] = player.playerPos;
                }
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                newPos = player.playerPos.x - 1;
                if (newPos >= 0)
                {
                    player.playerPos.x = newPos;
                    player.moveVol--;
                    player.movePoints[player.moveVol] = player.playerPos;
                }
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                newPos = player.playerPos.y + 1;
                if (newPos < mapScale.y)
                {
                    player.playerPos.y = newPos;
                    player.moveVol--;
                    player.movePoints[player.moveVol] = player.playerPos;
                }
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                newPos = player.playerPos.x + 1;
                if (newPos < mapScale.x)
                {
                    player.playerPos.x = newPos;
                    player.moveVol--;
                    player.movePoints[player.moveVol] = player.playerPos;
                }
            }
            int allow = GetLStickAllow();

            Debug.Log("Allow:" + allow);

            switch (allow)
            {
                case 1:
                    if ((player.playerPos.x % 2) == 0)
                    {
                        newPos = player.playerPos.x - 1;
                        if (newPos >= 0)
                        {
                            player.playerPos.x = newPos;
                            player.moveVol--;
                            player.movePoints[player.moveVol] = player.playerPos;
                        }
                    }
                    else
                    {
                        newPos = player.playerPos.y + 1;
                        if (newPos < mapScale.y)
                        {
                            newPos = player.playerPos.x - 1;
                            if (newPos >= 0)
                            {
                                player.playerPos.x = newPos;
                                player.playerPos.y += 1;
                                player.moveVol--;
                                player.movePoints[player.moveVol] = player.playerPos;
                            }
                        }
                    }
                    break;
                case 2:
                    newPos = player.playerPos.y + 1;
                    if (newPos < mapScale.y)
                    {
                        player.playerPos.y = newPos;
                        player.moveVol--;
                        player.movePoints[player.moveVol] = player.playerPos;
                    }
                    break;
                case 3:
                    if ((player.playerPos.x % 2) == 0)
                    {
                        newPos = player.playerPos.x + 1;
                        if (newPos < mapScale.x)
                        {
                            player.playerPos.x = newPos;
                            player.moveVol--;
                            player.movePoints[player.moveVol] = player.playerPos;
                        }
                    }
                    else
                    {
                        newPos = player.playerPos.y + 1;
                        if (newPos < mapScale.y)
                        {
                            newPos = player.playerPos.x + 1;
                            if (newPos < mapScale.x)
                            {
                                player.playerPos.x = newPos;
                                player.playerPos.y += 1;
                                player.moveVol--;
                                player.movePoints[player.moveVol] = player.playerPos;
                            }
                        }
                    }
                    break;
                case 7:
                    if ((player.playerPos.x % 2) == 0)
                    {
                        newPos = player.playerPos.y - 1;
                        if (newPos >= 0)
                        {
                            newPos = player.playerPos.x - 1;
                            if (newPos >= 0)
                            {
                                player.playerPos.x = newPos;
                                player.playerPos.y -= 1;
                                player.moveVol--;
                                player.movePoints[player.moveVol] = player.playerPos;
                            }
                        }
                    }
                    else
                    {
                        newPos = player.playerPos.x - 1;
                        if (newPos >= 0)
                        {
                            player.playerPos.x = newPos;
                            player.moveVol--;
                            player.movePoints[player.moveVol] = player.playerPos;
                        }
                    }
                    break;
                case 8:
                    newPos = player.playerPos.y - 1;
                    if (newPos >= 0)
                    {
                        player.playerPos.y = newPos;
                        player.moveVol--;
                        player.movePoints[player.moveVol] = player.playerPos;
                    }
                    break;
                case 9:
                    if ((player.playerPos.x % 2) == 0)
                    {
                        newPos = player.playerPos.y - 1;
                        if (newPos >= 0)
                        {
                            newPos = player.playerPos.x + 1;
                            if (newPos < mapScale.x)
                            {
                                player.playerPos.x = newPos;
                                player.playerPos.y -= 1;
                                player.moveVol--;
                                player.movePoints[player.moveVol] = player.playerPos;
                            }
                        }
                    }
                    else
                    {
                        newPos = player.playerPos.x + 1;
                        if (newPos < mapScale.x)
                        {
                            player.playerPos.x = newPos;
                            player.moveVol--;
                            player.movePoints[player.moveVol] = player.playerPos;
                        }
                    }
                    break;

            }



            player.diceUI.text = player.moveVol.ToString();

            player.transform.position = hexagonManger.GetMapPos(player.playerPos);
        }

    }


}
