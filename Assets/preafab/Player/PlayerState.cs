using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected HexagonManger hexagonManger;

    protected TurnContllor turnContllor;

    protected UIManager uiManager;

    public virtual void TurnInit(Player player, HexagonManger hexagon, TurnContllor turn, UIManager ui) {
        hexagonManger = hexagon;
        turnContllor = turn;
        uiManager = ui;
    }

    public virtual void TurnUpdate(Player player) { }
}