using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : PlayerState
{
    public override void TurnInit(Player player)
    {
        player.nowState = TurnState.PLAYER_MOVE;
        player.comandCanvas.gameObject.SetActive(false);
    }
    override public void TurnUpdate(Player player)
    {
        if (player.moveVol < 0)
        {
            player.ThrowDice();
        }
        else if (player.moveVol == 0)
        {
            player.GetMoveScores();

            player.turnContllor.SetNextTurnPlayerRPC();

            player.SetWait();
        }
        else
        {
            player.MoveHexagon(); 
        }

    }

}
