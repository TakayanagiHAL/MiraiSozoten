using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandState : PlayerState
{
    public override void TurnInit(Player player)
    {
        player.nowState = TurnState.SELECT_COMAND;
        player.comandCanvas.gameObject.SetActive(true);
    }
    override public void TurnUpdate (Player player){

    }

}
