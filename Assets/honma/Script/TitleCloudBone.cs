using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [Range(-1500.0f, 0.0f)] private float MinPosition_X;
    [SerializeField]
    [Range(0.0f, 600.0f)] private float MaxPosition_X;
    [SerializeField]
    [Range(-600.0f, 0.0f)] private float MinPosition_W;
    [SerializeField]
    [Range(0.0f, 600.0f)] private float MaxPosition_W;

    [SerializeField]     private float MinPosition_Y;
    [SerializeField]     private float MaxPosition_Y;

    

    // Start is called before the first frame update
    void Start()
    {
        _cloudArray = new GameObject[_cloudAmount];

        Spawn();
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
        
    }

    private void FixedUpdate()
    {
        

        if(_cloudArray.Length<_cloudAmount)//雲の数が総量を下回った時
        {
            float x = Random.Range(MinPosition_X, MaxPosition_X);
            float w = Random.Range(MinPosition_W, MaxPosition_W);
            float y = Random.Range(MinPosition_Y, MaxPosition_Y);

            Vector3 position = new Vector3(x, y, w);
            _cloudArray[CloudArraySerch()] = Instantiate(_cloudObject, position, Quaternion.identity);
        }
    }


    //空の要素を探し、instantiateする為の空の要素の番号を返す
    private int CloudArraySerch()
    {
        GameObject[] gameArray = _cloudArray;
        int index = 0;


        for(int i=0;i<_cloudAmount;i++)
        {
            if (gameArray[i] == null)
                index = i;
                break;
        }

        return index;
    }
}
