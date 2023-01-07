using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

class PortHexagon : HexagonMethod
{

    EventTrigger.Entry yesEvent;
    EventTrigger.Entry noEvent;

    YorNUI yorNUI;
    Player usePlayer;
    string message = "資源を換金しますか?";

    public PortHexagon()
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
    }

    public override void OnReach(Player player)
    {
        usePlayer = player;

        yorNUI = player.uiManager.GetCanvas(CanvasName.YES_OR_NO_UI).GetComponent<YorNUI>();
        player.uiManager.SetCanvas(CanvasName.YES_OR_NO_UI, true);

        yorNUI.SetEvent(yesEvent, noEvent);
        yorNUI.SetText(message);
    }

    void YesEvent()
    {
        usePlayer.money += usePlayer.seaResource.plastic;
        usePlayer.seaResource.plastic = 0;

        usePlayer.money += usePlayer.seaResource.ePlastic * 3;
        usePlayer.seaResource.ePlastic = 0;
 
        usePlayer.money += usePlayer.seaResource.wood * 4;
        usePlayer.seaResource.wood = 0;

        usePlayer.money += usePlayer.seaResource.steel * 15;
        usePlayer.seaResource.steel = 0;

        usePlayer.money += usePlayer.seaResource.seaFood * 12;
        usePlayer.seaResource.seaFood = 0;

        usePlayer.SetWait();
        usePlayer.turnContllor.StartCraftFase();

        usePlayer.uiManager.SetCanvas(CanvasName.YES_OR_NO_UI, false);

    }

    void NoEvent()
    {
        usePlayer.SetWait();
        usePlayer.turnContllor.StartCraftFase();

        usePlayer.uiManager.SetCanvas(CanvasName.YES_OR_NO_UI, false);

    }
}

