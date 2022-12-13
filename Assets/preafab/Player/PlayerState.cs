using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected HexagonManger hexagonManger;

    protected TurnContllor turnContllor;

    public virtual void TurnInit(Player player, HexagonManger hexagon, TurnContllor turn) {
        hexagonManger = hexagon;
        turnContllor = turn;
    }

    public virtual void TurnUpdate(Player player) { }
}