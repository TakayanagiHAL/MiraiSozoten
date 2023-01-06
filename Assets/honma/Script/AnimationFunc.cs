using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationFunc : MonoBehaviour
{
    [SerializeField]
    private ResultData _resultDataScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Phase01StartEvent()
    {
        _resultDataScript.phase01Start();
    }

    void GameSceneFalseEvent()
    {
        _resultDataScript.gameSceneFalse();
    }

    void Phase02CameraFocusEvent()
    {
        _resultDataScript.phase02CameraFocusEnd();
    }
}
