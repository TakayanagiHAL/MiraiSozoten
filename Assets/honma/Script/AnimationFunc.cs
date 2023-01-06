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

    void Phase01Start()
    {
        _resultDataScript.phase01Start();
    }

    void GameSceneFalse()
    {
        _resultDataScript.gameSceneFalse();
    }
}
