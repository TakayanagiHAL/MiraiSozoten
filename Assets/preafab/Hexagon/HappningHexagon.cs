using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class HappningHexagon : HexagonMethod
{
    public override void OnPassage(Player player)
    {
        
    }

    public override void OnReach(Player player)
    {
        player.nextState = TurnState.HAPPNING_EVENT;
    }
}

