using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

class SeabaseHexagon : HexagonMethod
{
    bool isMedal = false;

    int cost = 100;

    EventTrigger.Entry yesEvent;
    EventTrigger.Entry noEvent;

    YorNUI yorNUI;
    Player usePlayer;
    string message = "勲章を獲得しますか?";


    public SeabaseHexagon()
    {
        yesEvent = new EventTrigger.Entry();
        noEvent = new EventTrigger.Entry();

        yesEvent.callback.AddListener(data => YesEvent());
        noEvent.callback.AddListener(data => NoEvent());

        yesEvent.eventID = EventTriggerType.PointerDown;
        noEvent.eventID = EventTriggerType.PointerDown;
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

            yorNUI.SetEvent(yesEvent, noEvent);
            yorNUI.SetText(message);

            usePlayer = player;

            player.SetWait();

            Debug.Log("OnMedal");
        }
        else
        {
            player.SetWait();

            player.turnContllor.SetNextTurnPlayerRPC();

        }


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

