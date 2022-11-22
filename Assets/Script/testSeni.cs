using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class testSeni : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameScene()
    {
        SceneManager.LoadScene("game");
    }

    public void DelayGame()
    {
        Invoke("GameScene", 0.5f);
    }

    public void LetsGoGame()
    {
        testSeni[] testSeni = FindObjectsOfType<testSeni>();

        Debug.Log("senis" + testSeni.Length);

        for(int i = 0; i < testSeni.Length; i++)
        {
            testSeni[i].DelayGame();
        }
    }
}
