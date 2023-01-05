using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningManager : MonoBehaviour
{
    [SerializeField] TextController text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // テキスト終了フラグが立ったらシーン終了
        if (text.GetTextEnd() == true)
        {
            OpeningEnd();
        }
    }

    // シーン変更用の関数
    void OpeningEnd()
    {

    }
}
