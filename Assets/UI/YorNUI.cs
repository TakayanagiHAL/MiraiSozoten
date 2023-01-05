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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetEvent(EventTrigger yes,EventTrigger no)
    {
        yesEvent = yes;
        noEvent = no;
    }

    public void SetText(string set)
    {
        message.text = set;
    }
}
