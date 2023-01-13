using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BackUI : MonoBehaviour
{
    bool setBack =false;
    [SerializeField] GameObject firstButton;
    public void SetFirstButton()
    {
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsBack() { return setBack; }

    public void SetBack(bool back) { setBack = back; }
}
