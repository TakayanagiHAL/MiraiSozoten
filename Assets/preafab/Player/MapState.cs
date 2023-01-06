using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapState : PlayerState
{
    BackUI backUI;
    float zoomSpeed = 5.0f;
    float moveSpeed = 5.0f;
    public override void TurnInit(Player player, HexagonManger hexagon, TurnContllor turn, UIManager ui)
    {
        base.TurnInit(player, hexagon, turn,ui);
        player.nowState = TurnState.MAP_VIEW;
        uiManager.SetCanvas(CanvasName.COMMAND_UI, false);
        uiManager.SetCanvas(CanvasName.BACK_UI, true);
        backUI = uiManager.GetCanvas(CanvasName.BACK_UI).GetComponent<BackUI>();
        player.playerCamera.SetMapCamera(true);
    }
    override public void TurnUpdate (Player player){
        Vector3 delta = new Vector3(Time.deltaTime * moveSpeed, Time.deltaTime * zoomSpeed, Time.deltaTime * moveSpeed);

        if (Gamepad.current == null)
        {
            if (Input.GetKey(KeyCode.W))
            {
                delta.y *= 1.0f;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                delta.y *= -1.0f;
            }
            else
            {
                delta.y *= 0.0f;
            }
            if (Input.GetKey(KeyCode.A))
            {
                delta.x *= -1.0f;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                delta.x *= 1.0f;
            }
            else
            {
                delta.x *= 0.0f;
            }
            if (Input.GetKey(KeyCode.Q))
            {
                delta.z *= 1.0f;
            }
            else if (Input.GetKey(KeyCode.E))
            {
                delta.z *= -1.0f;
            }
            else
            {
                delta.z *= 0.0f;
            }
        }
        else
        {
            delta.x *= Gamepad.current.leftStick.x.ReadValue();
            delta.y *= Gamepad.current.rightStick.y.ReadValue();
            delta.z *= Gamepad.current.leftStick.y.ReadValue();
        }

        player.playerCamera.MapCameraMove(delta);

        if (Gamepad.current != null)
        {
            if (Gamepad.current.aButton.wasPressedThisFrame)
            {
                player.nextState = TurnState.SELECT_COMAND;

                player.playerCamera.ResetMapCamera();
                player.playerCamera.SetMapCamera(false);
                uiManager.SetCanvas(CanvasName.COMMAND_UI, true);
                uiManager.SetCanvas(CanvasName.BACK_UI, false);
            }
        }
        if (backUI.IsBack())
        {
            backUI.SetBack(false);

            player.nextState = TurnState.SELECT_COMAND;

            player.playerCamera.ResetMapCamera();
            player.playerCamera.SetMapCamera(false);
            uiManager.SetCanvas(CanvasName.COMMAND_UI, true);
            uiManager.SetCanvas(CanvasName.BACK_UI, false);
        }
    }

}
