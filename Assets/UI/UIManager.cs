using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public enum CanvasName
{
    SCORE_UI,
    RESOURCE_UI,
    COMMAND_UI,
    DICE_UI,
    R_EFFECT_UI,
    HAPPNING_UI,
    YES_OR_NO_UI,
    CRAFT_UI,
    BACK_UI,
    TURN_START_UI
}

[Serializable]
public struct CanvasPair
{
    public CanvasName canvasName;
    public Canvas canvas;
}

[Serializable]
public class CanvasDictionary
{
    public List<CanvasPair> keyValuePairs;

    public Canvas GetCanvas(CanvasName name)
    {
        for (int i = 0; i < keyValuePairs.Count; i++)
        {
            if (name == keyValuePairs[i].canvasName)
            {
                return keyValuePairs[i].canvas;
            }
        }
        return null;
    }
}

public class UIManager : MonoBehaviour
{
    [SerializeField] CanvasDictionary canvasDictionary;

    // Start is called before the first frame update
    void Start()
    {
        SetCanvas(CanvasName.DICE_UI, false);
        SetCanvas(CanvasName.HAPPNING_UI, false);
        SetCanvas(CanvasName.YES_OR_NO_UI, false);
        SetCanvas(CanvasName.BACK_UI, false);
        SetCanvas(CanvasName.CRAFT_UI, false);
        SetCanvas(CanvasName.TURN_START_UI, false);
        SetCanvas(CanvasName.COMMAND_UI, false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCanvas(CanvasName name,bool flag)
    {
        Canvas canvas = canvasDictionary.GetCanvas(name);
        canvas.enabled = flag;
    }

    public Canvas GetCanvas(CanvasName name)
    {
        return canvasDictionary.GetCanvas(name);
    }

    public GameObject GetCanvasObject(CanvasName name)
    {
        return canvasDictionary.GetCanvas(name).gameObject;
    }
}
