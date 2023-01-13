using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CommandUI : MonoBehaviour
{
    [SerializeField] GameObject firstButton;
    public void SetFirstButton()
    {
        EventSystem.current.SetSelectedGameObject(firstButton);
    }
    // Start is called before the first frame update
    void Start()
    {
        SetFirstButton();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
