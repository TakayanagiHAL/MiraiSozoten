using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class PortHexagon : HexagonMethod
{
    public override void OnPassage(Player player)
    {
    }

    public override void OnReach(Player player)
    {
        player.money += player.seaResource.plastic;
        player.seaResource.plastic = 0;

        player.money += player.seaResource.ePlastic*3;
        player.seaResource.ePlastic = 0;

        player.money += player.seaResource.wood*4;
        player.seaResource.wood = 0;

        player.money += player.seaResource.steel*15;
        player.seaResource.steel = 0;

        player.money += player.seaResource.seaFood*12;
        player.seaResource.seaFood = 0;

        player.SetWait();
    }
}

