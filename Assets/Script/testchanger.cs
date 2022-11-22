using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testchanger : MonoBehaviour
{
    [SerializeField] testSeni seni;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoScene()
    {
        seni.DelayGame();
    }

    public void LetsGo()
    {
        testchanger[] testchangers = FindObjectsOfType<testchanger>();


        for (int i = 0; i < testchangers.Length; i++)
        {
            testchangers[i].GoScene();
        }
    }
}
