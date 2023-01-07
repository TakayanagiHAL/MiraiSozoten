using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnStartUI : MonoBehaviour
{
    [SerializeField] Text text;
    int turn = 1;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AnimEnd()
    {
        GetComponent<Animator>().SetBool("isStart", false);
        GetComponent<Canvas>().enabled = false;
        FindObjectOfType<TurnContllor>().StartFirstPlayer();
    }

    public void AnimStart()
    {
        GetComponent<Animator>().SetBool("isStart", true);
    }

    public void SetTurn(int t)
    {
        turn = t;
        text.text = "É^Å[Éì" + turn.ToString();
    }
}
