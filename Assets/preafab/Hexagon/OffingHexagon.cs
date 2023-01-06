using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class OffingHexagon : HexagonMethod
{
    public override void OnPassage(Player player)
    {
        player.getRVol += 2;
    }

    public override void OnReach(Player player)
    {
        player.SetWait();
        player.turnContllor.StartCraftFase();
    }
}

