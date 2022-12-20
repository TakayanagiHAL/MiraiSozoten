using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleCloudBone : MonoBehaviour
{
    [SerializeField] [Header("preafabの雲のGameObjectを入れる")]
    private GameObject _cloudObject;

    [SerializeField] [Header("何も入れない")]
    private GameObject[] _cloudArray;

    [SerializeField][Header("雲の生成数")]
    private int _cloudAmount;

    [Header("----------------------")]
    [SerializeField]
    [Range(-1200.0f, -600.0f)] private float MinPosition_X;
    [SerializeField]
    [Range(-1100.0f, -500.0f)] private float MaxPosition_X;
    
    [SerializeField]
    [Range(-1500.0f, 0.0f)] private float MinPosition_W;
    [SerializeField]
    [Range(0.0f, 1500.0f)] private float MaxPosition_W;

    [SerializeField]     private float MinPosition_Y;
    [SerializeField]     private float MaxPosition_Y;

    //ki-
    Keyboard keyboard;
    int putA = 0;

    // Start is called before the first frame update
    void Start()
    {
        _cloudArray = new GameObject[_cloudAmount];

        Spawn();

        // 現在のキーボード情報
        keyboard = Keyboard.current;
        // キーボード接続チェック
        if (keyboard == null)
        {
            // キーボードが接続されていないと
            // Keyboard.currentがnullになる
            return;
        }
    }

    private void Spawn()
    {
        for (int i = 0; i < _cloudAmount; i++)
        {

            float x = Random.Range(MinPosition_X, MaxPosition_X);
            float w = Random.Range(MinPosition_W, MaxPosition_W);

            float y = Random.Range(MinPosition_Y, MaxPosition_Y);

            Vector3 position = new Vector3(x, y, w);

            _cloudArray[i] = Instantiate(_cloudObject, position, Quaternion.identity, this.gameObject.transform);
        }

    }

    // Update is called once per frame
    void Update()
    {
        // 1キーの入力状態取得
        var Key_a = keyboard.aKey;
        
        if (Key_a.wasPressedThisFrame)
        {
            Destroy(_cloudArray[putA]);
            putA += 1;
            if(putA>=2)
            {
                float x = Random.Range(MinPosition_X, MaxPosition_X);
                float w = Random.Range(MinPosition_W, MaxPosition_W);

                float y = Random.Range(MinPosition_Y, MaxPosition_Y);

                Vector3 position = new Vector3(x, y, w);
                _cloudArray[putA-2] = Instantiate(_cloudObject, position, Quaternion.identity, this.gameObject.transform);
            }
           
        }
    }

    //private void FixedUpdate()
    //{
    //    // 1キーの入力状態取得
    //    var Key_b = keyboard.bKey;
    //    Debug.Log("雲の総量がAmount以下のとき"+_cloudArray.Length);

    //    if(CloudArraySerch()>0)//雲の数が総量を下回った時
    //    {
    //        //int a = CloudArraySerch();
            
    //        float x = Random.Range(MinPosition_X, MaxPosition_X);
    //        float w = Random.Range(MinPosition_W, MaxPosition_W);
    //        float y = Random.Range(MinPosition_Y, MaxPosition_Y);

    //        Vector3 position = new Vector3(x, y, w);
    //        //Debug.Log("雲のi:"+ CloudArraySerch());
    //        _cloudArray[CloudArraySerch()] = Instantiate(_cloudObject, position, Quaternion.identity, this.gameObject.transform);
    //    }
    //}


    ////空の要素を探し、instantiateする為の空の要素の番号を返す
    //private int CloudArraySerch()
    //{
    //    GameObject[] gameArray = _cloudArray;
    //    int index = _cloudAmount + 1;


    //    for (int i = 0; i <= _cloudAmount; i++)
    //    {
    //        if (gameArray[i] == null)
    //            return i;
    //    }


    //    return index;
    //}
}
