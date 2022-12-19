using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandState : PlayerState
{
    public override void TurnInit(Player player, HexagonManger hexagon, TurnContllor turn, UIManager ui)
    {
        base.TurnInit(player, hexagon, turn,ui);
        player.nowState = TurnState.SELECT_COMAND;
        uiManager.SetCanvas(CanvasName.COMMAND_UI, true);
    }
    override public void TurnUpdate (Player player){

    }

}
