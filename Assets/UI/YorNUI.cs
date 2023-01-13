using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class YorNUI : MonoBehaviour
{
    [SerializeField] Button buttonYes;
    [SerializeField] Button buttonNo;
    [SerializeField] Text message;
    EventTrigger yesEvent;
    EventTrigger noEvent;

    [SerializeField] GameObject firstButton;

    public void SetFirstButton()
    {
        EventSystem.current.SetSelectedGameObject(firstButton);
    }
    // Start is called before the first frame update
    void Start()
    {
        yesEvent = buttonYes.gameObject.AddComponent<EventTrigger>();

        noEvent = buttonNo.gameObject.AddComponent<EventTrigger>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetEvent(EventTrigger.Entry yes, EventTrigger.Entry no)
    {
        yesEvent.triggers.Clear();
        noEvent.triggers.Clear();

        yesEvent.triggers.Add(yes);
        noEvent.triggers.Add(no);
    }

    public void SetText(string set)
    {
        message.text = set;
    }
}
