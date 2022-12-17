using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class HappningState : PlayerState
{
    enum HState
    {
        SELECT_HAPPNING,
        EXE_HAPPNING
    }

    HappningUI happningUI;

    int useHappning;

    HState state;

    public override void TurnInit(Player player, HexagonManger hexagon, TurnContllor turn, UIManager ui)
    {
        base.TurnInit(player, hexagon, turn,ui);

        happningUI = uiManager.GetCanvas(CanvasName.HAPPNING_UI).GetComponent<HappningUI>();

        uiManager.SetCanvas(CanvasName.HAPPNING_UI, true);

        useHappning = -1;
    }
    override public void TurnUpdate(Player player)
    {
        switch (state)
        {
            case HState.SELECT_HAPPNING:
                useHappning = happningUI.GetHappen();
                if (useHappning >= 0)
                {
                    state = HState.EXE_HAPPNING;
                }
                break;

            case HState.EXE_HAPPNING:
                switch(useHappning)
                {
                    case 0:
                    break;
                     case 1:
                    break;
                     case 2:
                    break;
                     case 3:
                    break;
                     case 4:
                    break;
                     case 5:
                    break;
                     case 6:
                    break;
                }
                break;
        }
    }


}
