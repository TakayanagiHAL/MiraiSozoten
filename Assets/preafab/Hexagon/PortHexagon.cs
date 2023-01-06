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
    string message = "勲章を獲得しますか?";

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

        player.SetWait();
        player.turnContllor.StartCraftFase();
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

        usePlayer.turnContllor.SetNextTurnPlayerRPC();

        usePlayer.uiManager.SetCanvas(CanvasName.YES_OR_NO_UI, false);

    }

    void NoEvent()
    {
        usePlayer.turnContllor.SetNextTurnPlayerRPC();

        usePlayer.uiManager.SetCanvas(CanvasName.YES_OR_NO_UI, false);

    }
}

