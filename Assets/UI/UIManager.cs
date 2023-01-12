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
        if (flag)
        {
            switch (name)
            {
                case CanvasName.BACK_UI:
                    canvas.GetComponent<BackUI>().SetFirstButton();
                    break;
                case CanvasName.COMMAND_UI:
                    canvas.GetComponent<CommandUI>().SetFirstButton();
                    break;
                case CanvasName.DICE_UI:
                    canvas.GetComponent<DiceUI>().SetFirstButton();
                    break;
                case CanvasName.HAPPNING_UI:
                    canvas.GetComponent<happningUI>().SetFirstButton();
                    break;
                case CanvasName.YES_OR_NO_UI:
                    canvas.GetComponent<YorNUI>().SetFirstButton();
                    break;
            }
        }
    }

    public Canvas GetCanvas(CanvasName name)
    {
        return canvasDictionary.GetCanvas(name);
    }

    public GameObject GetCanvasObject(CanvasName name)
    {
        return canvasDictionary.GetCanvas(name).gameObject;
    }

    public void SetCamera(Camera camera)
    {
        foreach(CanvasPair pair in canvasDictionary.keyValuePairs)
        {
            pair.canvas.worldCamera = camera;
            pair.canvas.planeDistance = 1.0f;
        }
    }
}
