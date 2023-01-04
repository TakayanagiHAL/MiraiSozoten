using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

class SeabaseHexagon : HexagonMethod
{
    bool isMedal = false;

    int cost = 100;

    EventTrigger yesTrigger;
    EventTrigger noTrigger;

    EventTrigger.Entry yesEvent;
    EventTrigger.Entry noEvent;

    YorNUI yorNUI;
    Player usePlayer;
    string message;

    public SeabaseHexagon()
    {
        yesEvent.callback.AddListener(data => YesEvent());
        noEvent.callback.AddListener(data => NoEvent());

        yesEvent.eventID = EventTriggerType.PointerEnter;
        noEvent.eventID = EventTriggerType.PointerEnter;

        yesTrigger.triggers.Add(yesEvent);
        noTrigger.triggers.Add(noEvent);
    }

    public override void OnPassage(Player player)
    {
        player.getRVol += 2;
    }

    public override void OnReach(Player player)
    {
        if (isMedal)
        {
            yorNUI = player.uiManager.GetCanvas(CanvasName.YES_OR_NO_UI).GetComponent<YorNUI>();
            player.uiManager.SetCanvas(CanvasName.YES_OR_NO_UI,true);

            yorNUI.SetEvent(yesTrigger, noTrigger);
            yorNUI.SetText(message);

            usePlayer = player;
        }

        player.SetWait();

        //player.turnContllor.SetNextTurnPlayerRPC();
    }

    public void SetMedal()
    {
        isMedal = true;
    }

    void YesEvent()
    {
        usePlayer.medal++;

        usePlayer.money -= cost;

        isMedal = false;

        usePlayer.turnContllor.SetNextTurnPlayerRPC();

        usePlayer.uiManager.SetCanvas(CanvasName.YES_OR_NO_UI, false);

    }

    void NoEvent()
    {
        usePlayer.turnContllor.SetNextTurnPlayerRPC();

        usePlayer.uiManager.SetCanvas(CanvasName.YES_OR_NO_UI, false);
    }
}

