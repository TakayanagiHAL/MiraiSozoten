using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class happningUI : MonoBehaviour
{
    enum HappenState
    {
        EVENT_SELECT,
            WAIT
    }


    [SerializeField] GameObject selectUI;
    [SerializeField] RectTransform allowPos;

    HappenState state = HappenState.EVENT_SELECT;

    int useHappen = -1;

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
        switch (state)
        {
            case HappenState.EVENT_SELECT:
                selectUI.active = true;
                allowPos.localPosition = new Vector3(allowPos.localPosition.x, 300+(-90 * Random.Range(0, 7)), allowPos.localPosition.z);
                useHappen = -1;
                break;
            case HappenState.WAIT:
                break;
        }
    }

    public void SelectStop()
    {
        state = HappenState.WAIT;
        useHappen = Random.Range(0, 7);
        useHappen = 0;
        selectUI.active = false;
    }

    public int GetHappen()
    {
        int b = -1;
        if (useHappen > -1)
        {
            b = useHappen;
            useHappen = -1;
        }
        return b;
    }

    public void SetSelect()
    {
        state = HappenState.EVENT_SELECT;
    }
}
